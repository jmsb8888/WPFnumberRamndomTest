using MathNet.Numerics.Distributions;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelRandomTest
{
    internal class MeanTest
    {
        List<double> RiData;
        double EstimationError;
        public MeanTest(List<double> RiData, double EstimationError) {
            Console.WriteLine("Cantidad de dato00s " + RiData.Count);
            this.RiData = RiData;
            this.EstimationError = EstimationError;

        }

        public Boolean TakeTest()
        {
            Console.WriteLine("Cantidad de datos "+RiData.Count);    
            double average = Statistics.Mean(RiData);
            double oneMinusMidpointAlphaValue = 1 - (EstimationError / 2);
            double zValue = Normal.InvCDF(0, 1, oneMinusMidpointAlphaValue);
            double lowerLimit = (0.5) - (zValue * (1 / (Math.Sqrt(12 * RiData.Count))));
            double upperLimit = (0.5) + (zValue * (1 / (Math.Sqrt(12 * RiData.Count))));
            Boolean isValid = average < upperLimit && average > lowerLimit;


            Console.WriteLine(RiData.Count);
            Console.WriteLine("average: " + average + "\n");
            Console.WriteLine("value: " + oneMinusMidpointAlphaValue + "\n");
            Console.WriteLine("lowerLimit: " + lowerLimit + "\n");
            Console.WriteLine("upperLimit: " + upperLimit + "\n");
            Console.WriteLine("exit: " + isValid + "\n");
            Console.WriteLine("z: " + zValue + "\n");
            return isValid;
        }
    }
}
