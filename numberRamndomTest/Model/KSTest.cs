using MathNet.Numerics.Statistics;
using numberRamndomTest.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Xml.Linq;

namespace ModelRandomTest
{
    internal class KSTest
    {
        //Lista de los datos a probar
        List<double> RiData = new List<double>();
        //Valores de error y numero de intervalos
        double EstimationError;
        int NumberIntervals;
        //Resultados de la prueba
        Dictionary<string, double> ResultData = new Dictionary<string, double>();
        //Observable que contendra los resultados de tabla de la prueba KS
        ObservableCollection<FormatTableKS> FormatTableKS = new ObservableCollection<FormatTableKS>();
        
        public KSTest(List<double> RiData, double EstimationError, int NumberIntervals)
        {
            this.RiData = RiData;
            this.EstimationError = EstimationError;
            this.NumberIntervals = NumberIntervals <= 0 ? (int)Math.Ceiling(Math.Sqrt(RiData.Count)) : NumberIntervals;
            this.NumberIntervals = this.NumberIntervals>=280 ? 280 : this.NumberIntervals;
        }
        //Extracción de resultados
        public Dictionary<string, double> GetResults()
        {
            return this.ResultData;
        }
        //Extracción de los datos de la tabla
        public ObservableCollection<FormatTableKS> GetTableKS()
        {
            return this.FormatTableKS;
        }
        //Realiza la lectura del valor de la tabla KS desde un archivo csv según la cantidad de datos a probar
        private double CalculateValueKS()
        {
            double value = 0;
           //Comprueba si la tabla tiene un valor fijo para la cantidad de datos a probar
           //Es decir si son mas de 0  y 50 o menos
            if ( this.RiData.Count > 0 && this.RiData.Count <= 50 )
            {
                /*string relativePath = @"../../../Resources/KSTable.csv";
                string absolutePath = Path.GetFullPath(relativePath);
                FileHandler read = new FileHandler(absolutePath);*/
                //extrae el CSV de la compilación del proyecto
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "numberRamndomTest.Resources.KSTable.csv";
                //Realiza la busqueda del CSV
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "KSTable.csv");
                    using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
                //Extrae la ruta del csv
                string pathh = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "KSTable.csv");
                //Objeto que leera el archivo
                FileHandler read = new FileHandler(pathh);
                //lista para guardar los datos del csv en objetos ModelKS
                List<ModelKS> modelKs = new List<ModelKS>();
                //lectura del csv
                modelKs = read.ReadKsTableCsv();
                //Busca el valor adecuado según  la cantidad de datos a probar y el error estimado
                foreach ( ModelKS model in modelKs)
                {
                    if(model.Quantity == RiData.Count)
                    {
                        value = model.GetValue(EstimationError);
                        break;
                    }
                }
            }//En caso de ser mas de 50 datos invoca el metodo que realiza la respectiva operación para determinar el valor ks según la cantidad de datos y el error estimado
            else if( this.RiData.Count > 50)
            {
                ModelKS modelKs = new ModelKS();
                value = modelKs.CalculateError(EstimationError, this.RiData.Count);
            }
            return value;
        }
        //Realiza la prueba KS
        public Boolean testKS()
        {
            //Determina el máximo y el mínimo de los datos
            double minData = RiData.Min();
            double maxData = RiData.Max();
            //adiciona los valores a la lista de resultados
            ResultData.Add("Cantidad de datos: ", RiData.Count);
            ResultData.Add("Cantidad de intervalos: ", NumberIntervals);
            ResultData.Add("Dato Mínimo: ", minData);
            ResultData.Add("Dato Máximo: ", maxData );
            //Calculo de los intervalos y las frecuencias de cada uno de estos
            Dictionary<Tuple<double, double>, int> IntervalFrecuency;
            IntervalFrecuency = CalculateFrecuency(CreateIntervals(minData, maxData, NumberIntervals), NumberIntervals);
            //Calculo de la frecuencia acomulada
            List<double> cumulativeFrecuency = CalculateCumulativeFrecuency(IntervalFrecuency);
            //Calculo de la probabilidad
            List<double> cumulativeProbability = CalculateCumulativeProbability(cumulativeFrecuency);
            //Calculo de la Frecuencia esperada
            List<double> ExpectedCumulativeFrequency = CalculateExpectedCumulativeFrequency();
            //Calculo de la Probabilidad esperada
            List<double> ExpectedCumulativeProbability = CalculateExpectedCumulativeProbability(ExpectedCumulativeFrequency);
            //Calculo de la diferencia entre las probabilidades esperadas y las probabilidades obtenidas 
            List<double> ObservedDifference = CalculateObservedDifference(ExpectedCumulativeProbability, cumulativeProbability);
            //creacion de los datos para agregar a la tabla
            int count = 1;
            for (int i = 0; i < cumulativeFrecuency.Count; i++)
            {
                KeyValuePair<Tuple<double, double>, int> par = IntervalFrecuency.ElementAt(i);
                Tuple<double, double> interval = par.Key;
                FormatTableKS newRow = new FormatTableKS
                {
                    Index = count++,
                    Beginning = interval.Item1,
                    End = interval.Item2,
                    ObtainedFrequency = par.Value,
                    AcomulatedObtainedFrecuency = cumulativeFrecuency[i],
                    ObtainedProbability = cumulativeProbability[i],
                    AcomulatedExpectedFrequency = ExpectedCumulativeFrequency[i],
                    ExpectedProbability = ExpectedCumulativeProbability[i],
                    Difference = ObservedDifference[i],
                };
                FormatTableKS.Add(newRow);
            }
            //Diferencia maxima hallada
            double difMax = ObservedDifference.Max();
            //Valor de KS
            double valueKS = CalculateValueKS();
            //Resultado de la prueba
            Boolean isValid = difMax < valueKS;
            ResultData.Add("Error Máximo: ", difMax);
            ResultData.Add("Valor KS: ", valueKS);
            return isValid;
        }
        //Creación de los intervalos, segun el dato mínimo y el máximo asi como la cantidad de intervalos
        private List<Tuple<double, double>> CreateIntervals(double minData, double maxData, int numberOfIntervals)
        {
            //Tamaño de cada intervalo
            double withInterval = (maxData - minData) / numberOfIntervals;
            //limite del primer intervalo
            double upperLimit = minData + withInterval;
            //Lista de intervalos
            List<Tuple<double, double>> intervals = new List<Tuple<double, double>>();
            //Creacion de los demas intervalos y adicion a la lista de intervalos
            intervals.Add(Tuple.Create(minData, upperLimit));
            for (int i = 1; i < numberOfIntervals; i++)
            {
                double lowerLimit = upperLimit;
                upperLimit = lowerLimit + withInterval;
                intervals.Add(Tuple.Create(lowerLimit, upperLimit));
            }
            return intervals;
        }

        //Calculo de la frecuencia que aparecen los datos en cada intervalos, apartir de una lista de intervalos previamente creada
        private Dictionary<Tuple<double, double>, int> CalculateFrecuency(List<Tuple<double, double>> intervals, int numberOfIntervals)
        {
            //Cada dato del diccionario tiene una tupla que son los limites del intervalo y un valor que sera la frecuencia
            Dictionary<Tuple<double, double>, int> IntervalFrecuency = new Dictionary<Tuple<double, double>, int>();
            //Para casa intervalo se contara los datos a probar que estan en su rango y se adicionara al diccionario creado
            foreach (var interval in intervals)
            {
                double lowerBound = interval.Item1;
                double upperBound = interval.Item2;
                int frecuency = RiData.Count(x => x >= lowerBound && x < upperBound);
                IntervalFrecuency[interval] = frecuency;
            }
            return IntervalFrecuency;
        }
        //Calculo de las frecuencias acomuladas
        private List<double> CalculateCumulativeFrecuency(Dictionary<Tuple<double, double>, int> IntervalFrecuency)
        {
            //Lista de resultados 
            List<double> cumulativeFrecuency = new List<double>();
            //Acomulador de las frecuancias que se van iterando
            int accumulator = 0;
            //itera las Frecuencias y las adiciona a las anteriores mediante el acomulador, ademas la agrega a la lista de resultados
            foreach (var kvp in IntervalFrecuency)
            {
                accumulator += kvp.Value;
                cumulativeFrecuency.Add(accumulator);
            }
            return cumulativeFrecuency;
        }
        //Calcula de  la Probabilidad  en funcion de la frecuencia acomulada obtenida
        private List<double> CalculateCumulativeProbability(List<double> cumulativeFrecuency)
        {
            //Lista de resultados
            List<double> cumulativeProbability = new List<double>();
            //Itera las frecuencias y las divide en el total de datos, ademas las agrega a la lista de resultados
            foreach (double frecuencyA in cumulativeFrecuency)
            {
                
                double aux = frecuencyA / (double)RiData.Count;
                cumulativeProbability.Add(aux);
            }
            return cumulativeProbability;
        }
        //Calcula la frecuencia acomulada esperada
        private List<double> CalculateExpectedCumulativeFrequency()
        {
            //Lista de resultados
            List<double> ExpectedCumulativeFrequency = new List<double>();
            //Ancho del intervalo
            double withIntervals= (double)RiData.Count / (double)NumberIntervals;
            //Acomula la suma de los valores que se espera lleve la frecuencia acomulada, en cada intervalo, ademas los adiciona a la lista de resultados
            double accumulator = 0;
            for (int i = 0; i < NumberIntervals; i++)
            {
                accumulator += withIntervals;
                ExpectedCumulativeFrequency.Add(accumulator);
            }
            return ExpectedCumulativeFrequency;
        }
        //Calcula la Porbabilidad acomulada que se espera en funcion de la Frecuencia esperada
        private List<double> CalculateExpectedCumulativeProbability(List<double> ExpectedCumulativeFrequency)
        {
            //Lista de resultados
            List<double> ExpectedCumulativeProbability = new List<double>();
            //Divide la frecuencia acomulada esperada en la cantidad de datos para determinar la probabilidad esperada y las va adicionando a la lista de resultados
            foreach (double frecuency in ExpectedCumulativeFrequency)
            {
                double aux = frecuency / (double)RiData.Count;
                ExpectedCumulativeProbability.Add(aux);
            }
            return ExpectedCumulativeProbability;
        }
        //Realiza el calculo de la diferencia obtenida entre los valores de probabilidad esperados y los obtenidos 
        private List<double> CalculateObservedDifference(List<double> ExpectedCumulativeProbability, List<double> cumulativeProbability)
        {
            //lista de resultados
            List<double> ObservedDifference = new List<double>();
            //Itera la lista de probabilidades esperas y obtenidas basandose en que ambas listas deben por defincion ser iguales
            //y  realiza la resta entre ellas para luego obtener el valor absoluto
            for (int i = 0; i < ExpectedCumulativeProbability.Count; i++)
            {
                double aux = Math.Abs(cumulativeProbability[i] - ExpectedCumulativeProbability[i]);
                ObservedDifference.Add(aux);
            }
            return ObservedDifference;
        }
    }
}