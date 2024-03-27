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
        Dictionary<string, double> ResultData = new Dictionary<string, double>();
        double EstimationError;
        public MeanTest(List<double> RiData, double EstimationError) {
            this.RiData = RiData;
            this.EstimationError = EstimationError;
        }
        public Dictionary<string, double> GetResults()
        {
            return this.ResultData;
        }
        public Boolean TakeTest()
        {
            double average = Statistics.Mean(RiData);
            double oneMinusMidpointAlphaValue = 1 - (EstimationError / 2);
            double zValue = Normal.InvCDF(0, 1, oneMinusMidpointAlphaValue);
            double lowerLimit = (0.5) - (zValue * (1 / (Math.Sqrt(12 * RiData.Count))));
            double upperLimit = (0.5) + (zValue * (1 / (Math.Sqrt(12 * RiData.Count))));
            Boolean isValid = average < upperLimit && average > lowerLimit;
            ResultData.Add("Cantidad de datos: ",RiData.Count);
            ResultData.Add("Media de los datos: ",average);
            ResultData.Add("Valor para 1- (α/2): ", oneMinusMidpointAlphaValue);
            ResultData.Add("Valor de z: ",zValue);
            ResultData.Add("Límite inferior",lowerLimit);
            ResultData.Add("Límite superior",upperLimit);
            return isValid;
        }
    }
}
