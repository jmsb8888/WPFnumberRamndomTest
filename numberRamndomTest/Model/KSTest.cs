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
        List<double> RiData = new List<double>();
        double EstimationError;
        int NumberIntervals;
        Dictionary<string, double> ResultData = new Dictionary<string, double>();
        ObservableCollection<FormatTableKS> FormatTableKS = new ObservableCollection<FormatTableKS>();
        
        public KSTest(List<double> RiData, double EstimationError, int NumberIntervals)
        {
            this.RiData = RiData;
            this.EstimationError = EstimationError;
            this.NumberIntervals = NumberIntervals <= 0 ? (int)Math.Ceiling(Math.Sqrt(RiData.Count)) : NumberIntervals;
            this.NumberIntervals = this.NumberIntervals>=280 ? 280 : this.NumberIntervals;
        }

        public Dictionary<string, double> GetResults()
        {
            return this.ResultData;
        }
        public ObservableCollection<FormatTableKS> GetTableKS()
        {
            return this.FormatTableKS;
        }
        private double CalculateValueKS()
        {
            double value = 0;
           
            if ( this.RiData.Count > 0 && this.RiData.Count <= 50 )
            {
                /*string relativePath = @"../../../Resources/KSTable.csv";
                string absolutePath = Path.GetFullPath(relativePath);
                FileHandler read = new FileHandler(absolutePath);*/
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "numberRamndomTest.Resources.KSTable.csv";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "KSTable.csv");
                    using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
                string pathh = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "KSTable.csv");
                FileHandler read = new FileHandler(pathh);
                List<ModelKS> modelKs = new List<ModelKS>();
                modelKs = read.ReadKsTableCsv();
                foreach ( ModelKS model in modelKs)
                {
                    if(model.Quantity == RiData.Count)
                    {
                        value = model.GetValue(EstimationError);
                        break;
                    }
                }
            }
            else if( this.RiData.Count > 50)
            {
                ModelKS modelKs = new ModelKS();
                value = modelKs.CalculateError(EstimationError, this.RiData.Count);
            }
            return value;
        }
        public Boolean testKS()
        {
            double minData = RiData.Min();
            double maxData = RiData.Max();
            ResultData.Add("Cantidad de datos: ", RiData.Count);
            ResultData.Add("Cantidad de intervalos: ", NumberIntervals);
            ResultData.Add("Dato minimo: ", minData);
            ResultData.Add("Dato Maximo: ", maxData );
            Dictionary<Tuple<double, double>, int> IntervalFrecuency;
            IntervalFrecuency = CalculateFrecuency(CreateIntervals(minData, maxData, NumberIntervals), NumberIntervals);

            List<double> cumulativeFrecuency = CalculateCumulativeFrecuency(IntervalFrecuency);
            List<double> cumulativeProbability = CalculateCumulativeProbability(cumulativeFrecuency);
            List<double> ExpectedCumulativeFrequency = CalculateExpectedCumulativeFrequency();
            List<double> ExpectedCumulativeProbability = CalculateExpectedCumulativeProbability(ExpectedCumulativeFrequency);
            List<double> ObservedDifference = CalculateObservedDifference(ExpectedCumulativeProbability, cumulativeProbability);

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
            double difMax = ObservedDifference.Max();
            double valueKS = CalculateValueKS();
            Boolean isValid = difMax < valueKS;
            ResultData.Add("Error Maximo: ", difMax);
            ResultData.Add("Valor KS: ", valueKS);
            return isValid;
        }
        private List<Tuple<double, double>> CreateIntervals(double minData, double maxData, int numberOfIntervals)
        {
            double withInterval = (maxData - minData) / numberOfIntervals;
            double upperLimit = minData + withInterval;
            List<Tuple<double, double>> intervals = new List<Tuple<double, double>>();
            intervals.Add(Tuple.Create(minData, upperLimit));
            for (int i = 1; i < numberOfIntervals; i++)
            {
                double lowerLimit = upperLimit;
                upperLimit = lowerLimit + withInterval;
                intervals.Add(Tuple.Create(lowerLimit, upperLimit));
            }
            return intervals;
        }


        private Dictionary<Tuple<double, double>, int> CalculateFrecuency(List<Tuple<double, double>> intervals, int numberOfIntervals)
        {
            Dictionary<Tuple<double, double>, int> IntervalFrecuency = new Dictionary<Tuple<double, double>, int>();
            foreach (var interval in intervals)
            {
                double lowerBound = interval.Item1;
                double upperBound = interval.Item2;
                int frecuency = RiData.Count(x => x >= lowerBound && x < upperBound);
                IntervalFrecuency[interval] = frecuency;
            }
            return IntervalFrecuency;
        }

        private List<double> CalculateCumulativeFrecuency(Dictionary<Tuple<double, double>, int> IntervalFrecuency)
        {
            List<double> cumulativeFrecuency = new List<double>();
            int accumulator = 0;
            foreach (var kvp in IntervalFrecuency)
            {
                accumulator += kvp.Value;
                cumulativeFrecuency.Add(accumulator);
            }
            return cumulativeFrecuency;
        }
        private List<double> CalculateCumulativeProbability(List<double> cumulativeFrecuency)
        {
            List<double> cumulativeProbability = new List<double>();
            foreach (double frecuencyA in cumulativeFrecuency)
            {
                
                double aux = frecuencyA / (double)RiData.Count;
                cumulativeProbability.Add(aux);
            }
            return cumulativeProbability;
        }
        private List<double> CalculateExpectedCumulativeFrequency()
        {
            List<double> ExpectedCumulativeFrequency = new List<double>();
            double withIntervals= (double)RiData.Count / (double)NumberIntervals;
            double accumulator = 0;
            for (int i = 0; i < NumberIntervals; i++)
            {
                accumulator += withIntervals;
                ExpectedCumulativeFrequency.Add(accumulator);
            }
            return ExpectedCumulativeFrequency;
        }
        private List<double> CalculateExpectedCumulativeProbability(List<double> ExpectedCumulativeFrequency)
        {
            List<double> ExpectedCumulativeProbability = new List<double>();
            foreach (double frecuency in ExpectedCumulativeFrequency)
            {
                double aux = frecuency / (double)RiData.Count;
                ExpectedCumulativeProbability.Add(aux);
            }
            return ExpectedCumulativeProbability;
        }
        private List<double> CalculateObservedDifference(List<double> ExpectedCumulativeProbability, List<double> cumulativeProbability)
        {
            List<double> ObservedDifference = new List<double>();
            for (int i = 0; i < ExpectedCumulativeProbability.Count; i++)
            {
                double aux = Math.Abs(cumulativeProbability[i] - ExpectedCumulativeProbability[i]);
                ObservedDifference.Add(aux);
            }
            return ObservedDifference;
        }
    }
}