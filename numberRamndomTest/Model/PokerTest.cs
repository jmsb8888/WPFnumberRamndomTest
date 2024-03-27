using MathNet.Numerics.Distributions;
using numberRamndomTest.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ModelRandomTest
{
    internal class PokerTest
    {
        //Lista de valores a probar
        List<double> RiData = new List<double>();
        //Error estimado
        double EstimationError;
        //Lista de resultados
        Dictionary<string, double> ResultData = new Dictionary<string, double>();
        //Valores para la tabla
        ObservableCollection<FormatTablePoker> formatTablePokers = new ObservableCollection<FormatTablePoker>();
        public PokerTest(List<double> RiData, double EstimationError)
        {
            this.RiData = RiData;
            this.EstimationError = EstimationError;
        }
        //Extraer resultados
        public Dictionary<string, double> GetResults()
        {
            return this.ResultData;
        }
        //Obtener Valores para imprimir en la tabla de Poker
        public ObservableCollection<FormatTablePoker> GetTablePoker()
        {
            return this.formatTablePokers;
        }
        //Realizar Preuba de Poker
        public Boolean TestPoker()
        {
            //Adicionar total de datos a la lista de resultados
            ResultData.Add("Cantidad de datos: ", RiData.Count);
            //Convertir los datos de decimal a string para iterar sus digitos
            List<string> stringNumbers = RiData.Select(x => x.ToString("F5")).ToList();
            //Calcular los tipos de mano que hay en los datos
            List<int> listOfResults = CalculateHand(stringNumbers);
            //Calcular la probabilidad con respecto a la cantidad de datos
            List<double> Ei = CalculateEi();
            //Realizar calculo del error
            List<double> operation = CalculateOperation(Ei, listOfResults);
            //Probababilidades asociadas a cada tipo de mano
            double[] probabilities = [0.3024, 0.504, 0.108, 0.072, 0.009, 0.0045, 0.0001];
            //tipos de mano disponibles
            string[] hands = ["Todos diferentes", "Un Par", "Dos Pares", "Una Tercia", "Una Tercia Y Un Par", "4 Cartas Iguales", "5 Cartas Iguales"];
            //Establecer valores de la tabla, teniendo en cuenta que por definición todas las listas tienen el mismo tamaño 
            for (int i = 0; i < listOfResults.Count; i++)
            {
                FormatTablePoker poker = new FormatTablePoker
                {
                    Hand = hands[i],
                    ObservedQuantity = listOfResults[i],
                    Probability = probabilities[i],
                    ExpectedProbability = Ei[i],
                    Result = operation[i]
                };
                formatTablePokers.Add(poker);
            }
            //Calculo de la suma del error
            double ErrorsSum = operation.Sum();
            //calculo del valor de chi inv
            double valueChi = ChiSquared.InvCDF(6, 1 - EstimationError);
            //Adicionar datos a la lista de resultados
            ResultData.Add("Error total: ", ErrorsSum);
            ResultData.Add("Valor Chi2 Inverso: ", valueChi);
            //Resultado de la prueba
            Boolean isValid = ErrorsSum<= valueChi;
            return isValid;
        }
        //Realia el calculo de las manos, presentes en cada uno de los numeros que se han convertido a string
        private List<int> CalculateHand(List<string> stringNumbers)
        {
            //Lista de resultados
            List<int> listOfResults = new List<int>(new int[7]);
            foreach (string number in stringNumbers)
            {
                //se establece el separador que indica el decimal como .
                string cnumber = number.Replace(",", ".");
                //Se extrae la parte decimal, si hace falta numeros se comleta los 5 digitos con ceros
                string decimalPart = cnumber.Contains(".") ? cnumber.Substring(cnumber.IndexOf(".") + 1) : "00000";
                //valor inicial de cada cantidad de manos
                int allDifferent = 0;
                int OnePar = 0;
                int TwoPar = 0;
                int Onetrhee = 0;
                int OneFour = 0;
                int OneFive = 0;
                //resultado de manos para cada numero
                List<int> individualResults = new List<int>();
                //itera cada digito de cada numero
                foreach (char digit in decimalPart)
                {
                    int count = 0;
                    //cuenta las veces que esta el digito
                    count = decimalPart.Count(c => c == digit);
                    //si el tipo de digito osea ese numer digito no ha sido contado lo agrega
                    if (!individualResults.Contains((int)Char.GetNumericValue(digit)))
                    {
                        /*if(count == 5)
                        {
                            MessageBox.Show("dato de 5 " + number);
                        }*/
                        individualResults.Add((int)Char.GetNumericValue(digit));
                        //establece segun el contador que tipo de mano es
                        switch (count)
                        {
                            case 2:
                                OnePar++;
                                break;
                            case 3:
                                Onetrhee++;
                                break;
                            case 4:
                                OneFour++;
                                break;
                            case 5:
                                OneFive++;
                                break;
                        }//Si tiene dos pares lo asigana al tipo de mano correspondiente en lugar de que asigne un numero de mas en la mano de 1 par
                        if (OnePar == 2)
                        {
                            OnePar = 0;
                            TwoPar = 1;
                        }
                    }
                }//revisa salveddades para cuando son 4 cartas iguales
                if (OnePar != 0 && Onetrhee != 0)
                {
                    listOfResults[4]++;
                }//Revisa si todas las cartas fueron iguales
                else if (OnePar == 0 && TwoPar == 0 && Onetrhee == 0 && OneFour == 0 && OneFive == 0)
                {
                    listOfResults[0]++;
                }
                else
                {//asigana las manos restantes
                    listOfResults[1] += OnePar;
                    listOfResults[2] += TwoPar;
                    listOfResults[3] += Onetrhee;
                    listOfResults[5] += OneFour;
                    listOfResults[6] += OneFive;
                }
            }
            return listOfResults;
        }
            
        //Calculo del valor Ei
        private List<double> CalculateEi()
        {
            //Valores de probabilidad establecidos para cada mano
            double AllDifferent = 0.3024;
            double OnePair = 0.5040;
            double TwoPair = 0.1080;
            double ThreeOfAKind = 0.0720;
            double FullHouse = 0.0090;
            double FourOfAKind = 0.0045;
            double FiveOfAKind = 0.0001;
            //Calcula el valor Ei en base a la cantidad de numeros a probar
            List<double> Ei = new List<double>
            {
                AllDifferent * RiData.Count,
                OnePair * RiData.Count,
                TwoPair * RiData.Count,
                ThreeOfAKind * RiData.Count,
                FullHouse * RiData.Count,
                FourOfAKind * RiData.Count,
                FiveOfAKind * RiData.Count
            };
            return Ei;
        }
        //realiza el calculo del error por cada tipo de mano
        private List<double> CalculateOperation(List<double> Ei, List<int> listOfResults)
        {
            //resultados para el error
            List<double> operation = new List<double>();
            //Para cada tipo de mano calcula la diferencia entre Ei y la cantidad de datos por mano
            //y la eleva al cuadrado a continuación divide
            //en el valor de Ei
            for (int i = 0; i < Ei.Count; i++)
            {
                double factor = Math.Pow(10, 5);
                double result = Math.Pow(Ei[i] - listOfResults[i], 2) / Ei[i];
                double aux = Math.Truncate(result * factor) / factor;
                operation.Add(aux);
            }
            return operation;
        }
    }
}
