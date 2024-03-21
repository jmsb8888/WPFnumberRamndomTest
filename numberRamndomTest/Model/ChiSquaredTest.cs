﻿using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRandomTest
{
    internal class ChiSquaredTest
    {
        List<double> RiData = new List<double>();
        double EstimationError;
        int numberIntervals;
        public ChiSquaredTest(List<double> RiData, double EstimationError, int numberIntervals)
        {
            this.RiData = RiData;
            this.EstimationError = EstimationError;
            this.numberIntervals = numberIntervals;
        }
        public Boolean testChiSquarer()
        {
            double minData = RiData.Min();
            double maxData = RiData.Max();
            if (numberIntervals <= 0)
            {
                numberIntervals = (int)Math.Ceiling((3.5 * Statistics.StandardDeviation(RiData)) / Math.Cbrt(RiData.Count));
            }
            Console.WriteLine("NUMERO INETRVALOS  " + numberIntervals);
            Dictionary<Tuple<double, double>, int> IntervalFrecuency = CalculateFrecuency(CreateIntervals(minData, maxData, numberIntervals), numberIntervals);
            double resultChiSquarer = calculateChiSquarer(IntervalFrecuency, numberIntervals);
            double ErrorChiSquarer = ChiSquared.InvCDF(numberIntervals - 1, 1 - EstimationError);
            Console.WriteLine($"suma chi2 {resultChiSquarer}  CHISQUARER {ErrorChiSquarer}");
            Boolean isValid = ErrorChiSquarer >= resultChiSquarer;
            Console.WriteLine($"sirven: {isValid}");
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
            foreach (var kvp in IntervalFrecuency)
            {
                Console.WriteLine($"Intervalo [{kvp.Key.Item1}, {kvp.Key.Item2}): {kvp.Value}");
            }
            return IntervalFrecuency;
        }

        private double calculateChiSquarer(Dictionary<Tuple<double, double>, int> IntervalFrecuency, int numberOfIntervals)
        {
            double expectedFrecuency = (double)RiData.Count / numberOfIntervals;
            List<double> chiSquarer = new List<double>();
            foreach (var kvp in IntervalFrecuency)
            {
                double difference = Math.Pow(kvp.Value - expectedFrecuency, 2) / expectedFrecuency;
                Console.WriteLine($"Diferencia para {kvp.Key}: {difference}  esperada {expectedFrecuency}");
                chiSquarer.Add(difference);
            }
            double resultChiSquarer = chiSquarer.Sum();
            Console.WriteLine($"suma chi2 {resultChiSquarer}");
            return resultChiSquarer;
        }

    }
}
