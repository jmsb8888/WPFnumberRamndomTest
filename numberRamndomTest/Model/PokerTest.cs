﻿using MathNet.Numerics.Distributions;
using numberRamndomTest.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRandomTest
{
    internal class PokerTest
    {
        List<double> RiData = new List<double>();
        double EstimationError;
        Dictionary<string, double> ResultData = new Dictionary<string, double>();
        ObservableCollection<FormatTablePoker> formatTablePokers = new ObservableCollection<FormatTablePoker>();
        public PokerTest(List<double> RiData, double EstimationError)
        {
            this.RiData = RiData;
            this.EstimationError = EstimationError;
        }
        public Dictionary<string, double> GetResults()
        {
            return this.ResultData;
        }
        public ObservableCollection<FormatTablePoker> GetTablePoker()
        {
            return this.formatTablePokers;
        }
        public Boolean TestPoker()
        {
            Console.WriteLine("Cantidad de datos en prueba 1" + RiData.Count);
            ResultData.Add("Cantidad de datos: ", RiData.Count);
            List<string> stringNumbers = RiData.Select(x => x.ToString("F5")).ToList();
            List<int> listOfResults = CalculateHand(stringNumbers);
            List<double> Ei = CalculateEi();
            List<double> operation = CalculateOperation(Ei, listOfResults);
            double[] probabilities = [0.3024, 0.504, 0.108, 0.072, 0.009, 0.0045, 0.0001];
            string[] hands = ["Todos diferentes", "Un Par", "Dos Pares", "Una Tercia", "Una Terca Y Un Par", "4 Cartas Iguales", "5 Cartas Iguales"];
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
            double ErrorsSum = operation.Sum();
            double valueChi = ChiSquared.InvCDF(6, 1 - EstimationError);
            ResultData.Add("Error total: ", ErrorsSum);
            ResultData.Add("Valor Chi2 Imverso: ", valueChi);
            Boolean isValid = ErrorsSum<= valueChi;
            return isValid;
        }

        private List<int> CalculateHand(List<string> stringNumbers)
        {
            List<int> listOfResults = new List<int>(new int[7]);
            foreach (string number in stringNumbers)
            {
                string decimalPart = number.Contains(".") ? number.Substring(number.IndexOf(".") + 1) : "00000";
                int allDifferent = 0;
                int OnePar = 0;
                int TwoPar = 0;
                int Onetrhee = 0;
                int OneFour = 0;
                int OneFive = 0;
                List<int> individualResults = new List<int>();
                foreach (char digit in decimalPart)
                {
                    int count = 0;
                    count = decimalPart.Count(c => c == digit);
                    if (!individualResults.Contains((int)Char.GetNumericValue(digit)))
                    {
                        individualResults.Add((int)Char.GetNumericValue(digit));
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
                        }
                        if (OnePar == 2)
                        {
                            OnePar = 0;
                            TwoPar = 1;
                        }
                    }
                }
                if (OnePar != 0 && Onetrhee != 0)
                {
                    listOfResults[4]++;
                }
                else if (OnePar == 0 && TwoPar == 0 && Onetrhee == 0 && OneFour == 0 && OneFive == 0)
                {
                    listOfResults[0]++;
                }
                else
                {
                    listOfResults[1] += OnePar;
                    listOfResults[2] += TwoPar;
                    listOfResults[3] += Onetrhee;
                    listOfResults[5] += OneFour;
                    listOfResults[6] += OneFive;
                }
            }
            foreach (var kvp in listOfResults)
            {
                Console.WriteLine($" Valor: {kvp}");
            }
            return listOfResults;
        }
            

        private List<double> CalculateEi()
        {
            double AllDifferent = 0.3024;
            double OnePair = 0.5040;
            double TwoPair = 0.1080;
            double ThreeOfAKind = 0.0720;
            double FullHouse = 0.0090;
            double FourOfAKind = 0.0045;
            double FiveOfAKind = 0.0001;
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
        private List<double> CalculateOperation(List<double> Ei, List<int> listOfResults)
        {
            List<double> operation = new List<double>();
            for (int i = 0; i < Ei.Count; i++)
            {
                double factor = Math.Pow(10, 5);
                double result = Math.Pow(Ei[i] - listOfResults[i], 2) / Ei[i];
                double aux = Math.Truncate(result * factor) / factor;
                operation.Add(aux);
            }
            foreach (double result in operation)
            {
                Console.WriteLine("DDDDD " + result);
            }
            return operation;
        }
    }
}
