using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRandomTest
{
    internal class VarianceTest
    {
        List<double> RiData = new List<double>();
        double EstimationError;
        public VarianceTest(List<double> RiData, double EstimationError)
        {
            this.RiData = RiData;
            this.EstimationError = EstimationError;

        }

        public Boolean testVar()
        {
            double variance = Statistics.Variance(RiData);
            double oneMinusMidpointAlphaValue = 1 - (EstimationError / 2);
            double MidpointAlphaValue = (EstimationError / 2);
            double zValue = Normal.InvCDF(0, 1, oneMinusMidpointAlphaValue);
            double chiSquarer1 = ChiSquared.InvCDF(RiData.Count - 1, 1 - MidpointAlphaValue);
            double chiSquarer2 = ChiSquared.InvCDF(RiData.Count - 1, 1 - oneMinusMidpointAlphaValue);
            double lowerLimit = chiSquarer1 / (12 * (RiData.Count - 1));
            double upperLimit = chiSquarer2 / (12 * (RiData.Count - 1));
            Boolean isValid = variance >= upperLimit && variance <= lowerLimit;


            Console.WriteLine(RiData.Count);
            Console.WriteLine("variance: " + variance + "\n");
            Console.WriteLine("value: " + oneMinusMidpointAlphaValue + "\n");
            Console.WriteLine("valueMeans: " + MidpointAlphaValue + "\n");
            Console.WriteLine("chiSquarer1: " + chiSquarer1 + "\n");
            Console.WriteLine("chiSquarer2: " + chiSquarer2 + "\n");
            Console.WriteLine("lowerLimit: " + lowerLimit + "\n");
            Console.WriteLine("upperLimit: " + upperLimit + "\n");
            Console.WriteLine("exit: " + isValid + "\n");
            Console.WriteLine("z: " + zValue + "\n");
            return isValid;
        }
    }
}
