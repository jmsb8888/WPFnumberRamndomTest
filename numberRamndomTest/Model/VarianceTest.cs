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
        Dictionary<string, double> ResultData = new Dictionary<string, double>();
        double EstimationError;
        public VarianceTest(List<double> RiData, double EstimationError)
        {
            this.RiData = RiData;
            this.EstimationError = EstimationError;
        }
        public Dictionary<string, double> GetResults()
        {
            return this.ResultData;
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

            ResultData.Add("Cantidad de datos: ", RiData.Count);
            ResultData.Add("Varianza de los datos: ", variance);
            ResultData.Add("Valor para 1- (α/2): ", oneMinusMidpointAlphaValue);
            ResultData.Add("Valor para α/2: ", MidpointAlphaValue);
            ResultData.Add("Valor para X^2 de α/2: ", chiSquarer1);
            ResultData.Add("Valor para X^2 de 1-(α/2): ", chiSquarer2);
            ResultData.Add("Valor de z: ", zValue);
            ResultData.Add("Límite inferior", lowerLimit);
            ResultData.Add("Límite superior", upperLimit);
            return isValid;
        }
    }
}
