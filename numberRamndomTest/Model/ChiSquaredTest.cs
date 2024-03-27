using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;
using numberRamndomTest.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ModelRandomTest
{
    internal class ChiSquaredTest
    {
        //Datos a Probar
        List<double> RiData = new List<double>();
        //Resultados de la prueba
        Dictionary<string, double> ResultData = new Dictionary<string, double>();
        //Parametros de error y numero de intervalos 
        double EstimationError;
        int numberIntervals;
        //Formato de Devolución de los  resultados para su impresión 
        ObservableCollection<FormatTableChiSquare> formatTableChiSquare = new ObservableCollection<FormatTableChiSquare>();
        public ChiSquaredTest(List<double> RiData, double EstimationError, int numberIntervals)
        {
            this.RiData = RiData;
            this.EstimationError = EstimationError;
            //Si no existe un numero valido de intervalos se calcula uno
            this.numberIntervals = numberIntervals <= 0 ? (int)Math.Ceiling(Math.Sqrt(RiData.Count)) : numberIntervals;
            this.numberIntervals = this.numberIntervals >= 280 ? 280 : this.numberIntervals;
        }
        //Extracion de los resultados
        public Dictionary<string, double> GetResults()
        {
            return this.ResultData;
        }
        //Extraccion de los datos de las tablas
        public ObservableCollection<FormatTableChiSquare> GetTableChiSquares()
        {
            return this.formatTableChiSquare;
        }
        //Realización de la prueba
        public Boolean testChiSquarer()
        {
            //cálculo del mínimo y el máximo
            double minData = RiData.Min();
            double maxData = RiData.Max();
            //agregar valores a los resultados
            ResultData.Add("Cantidad de datos: ", RiData.Count);
            ResultData.Add("Cantidad de intervalos: ", numberIntervals);
            //Calculo de los intervalos y las frecuencias de cada uno de estos.
            Dictionary<Tuple<double, double>, int> IntervalFrecuency = CalculateFrecuency(CreateIntervals(minData, maxData, numberIntervals), numberIntervals);
            //Calculo de la suma de los errores
            double resultChiSquarer = calculateChiSquarer(IntervalFrecuency, numberIntervals);
            //calculo del valor de Chi para comparar
            double ErrorChiSquarer = ChiSquared.InvCDF(numberIntervals - 1, 1 - EstimationError);
            //Agregar los valores a la lista de resultados
            ResultData.Add("Error Total: ", resultChiSquarer);
            ResultData.Add("Valor de CHI inv: ", ErrorChiSquarer);
            //Resultado del Test
            Boolean isValid = ErrorChiSquarer > resultChiSquarer;
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
            //Primer intervalo
            intervals.Add(Tuple.Create(minData, upperLimit));
            //Creacion de los demas intervalos y adicion a la lista de intervalos
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
        //Calculo del valor de chi cuadrado a partir de las frecuencias de cada intervalo
        private double calculateChiSquarer(Dictionary<Tuple<double, double>, int> IntervalFrecuency, int numberOfIntervals)
        {
            //Frecuencia esperada para cada intervalo
            double expectedFrecuency = (double)RiData.Count / numberOfIntervals;
            //valores de chi cuadrado para cada intervalo
            List<double> chiSquarer = new List<double>();
            int index = 1;
            foreach (var kvp in IntervalFrecuency)
            {
                //calculo del valor de chi cuadrado para un intervalo
                double difference = Math.Pow(kvp.Value - expectedFrecuency, 2) / expectedFrecuency;
                //adicion a la estructura de datos que guarda los resultados de la operacion
                FormatTableChiSquare newRow = new FormatTableChiSquare
                {
                    Index = index++,
                    beginning = kvp.Key.Item1,
                    End = kvp.Key.Item2,
                    ObtainedFrequency = kvp.Value,
                    ExpectedFrequency = expectedFrecuency,
                    CHiSquarer = difference
                };
                formatTableChiSquare.Add(newRow);
                //adiciona a la lista de valores de chi cuadrado
                chiSquarer.Add(difference);
            }
            //Calcula la suma de los valores de chi cuadrado
            double resultChiSquarer = chiSquarer.Sum();
            return resultChiSquarer;
        }

    }
}
