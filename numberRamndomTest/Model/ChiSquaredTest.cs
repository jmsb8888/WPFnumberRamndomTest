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
        List<double> RiData = new List<double>();
        Dictionary<string, double> ResultData = new Dictionary<string, double>();
        double EstimationError;
        int numberIntervals;
        ObservableCollection<FormatTableChiSquare> formatTableChiSquare = new ObservableCollection<FormatTableChiSquare>();
        public ChiSquaredTest(List<double> RiData, double EstimationError, int numberIntervals)
        {
            this.RiData = RiData;
            this.EstimationError = EstimationError;
            this.numberIntervals = numberIntervals <= 0 ? (int)Math.Ceiling(Math.Sqrt(RiData.Count)) : numberIntervals;
            this.numberIntervals = this.numberIntervals >= 280 ? 280 : this.numberIntervals;
        }
        public Dictionary<string, double> GetResults()
        {
            return this.ResultData;
        }
        public ObservableCollection<FormatTableChiSquare> GetTableChiSquares()
        {
            return this.formatTableChiSquare;
        }
        public Boolean testChiSquarer()
        {
            double minData = RiData.Min();
            double maxData = RiData.Max();
            ResultData.Add("Cantidad de datos: ", RiData.Count);
            ResultData.Add("Cantidad de intervalos: ", numberIntervals);
            Dictionary<Tuple<double, double>, int> IntervalFrecuency = CalculateFrecuency(CreateIntervals(minData, maxData, numberIntervals), numberIntervals);
            double resultChiSquarer = calculateChiSquarer(IntervalFrecuency, numberIntervals);
            double ErrorChiSquarer = ChiSquared.InvCDF(numberIntervals - 1, 1 - EstimationError);
            ResultData.Add("ErrorChiSquarer total CHI2: ", resultChiSquarer);
            ResultData.Add("Valor de CHI inv: ", ErrorChiSquarer);
            Boolean isValid = ErrorChiSquarer >= ErrorChiSquarer;
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

        private double calculateChiSquarer(Dictionary<Tuple<double, double>, int> IntervalFrecuency, int numberOfIntervals)
        {
            double expectedFrecuency = (double)RiData.Count / numberOfIntervals;
            List<double> chiSquarer = new List<double>();
            int index = 1;
            foreach (var kvp in IntervalFrecuency)
            {
                double difference = Math.Pow(kvp.Value - expectedFrecuency, 2) / expectedFrecuency;
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
                chiSquarer.Add(difference);
            }
            double resultChiSquarer = chiSquarer.Sum();
            return resultChiSquarer;
        }

    }
}
