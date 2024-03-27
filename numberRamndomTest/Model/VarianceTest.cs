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
        //Lista de datos a probar
        List<double> RiData = new List<double>();
        // Lista de resultados
        Dictionary<string, double> ResultData = new Dictionary<string, double>();
        //Error estimado
        double EstimationError;
        public VarianceTest(List<double> RiData, double EstimationError)
        {
            this.RiData = RiData;
            this.EstimationError = EstimationError;
        }
        //extraccion de los resultados
        public Dictionary<string, double> GetResults()
        {
            return this.ResultData;
        }
        //Realiación de la prueba de varianzas
        public Boolean testVar()
        {
            //calculo de la varianza de los datos a probar
            double variance = Statistics.Variance(RiData);
            //Calculo del valor de 1- (α/2)
            double oneMinusMidpointAlphaValue = 1 - (EstimationError / 2);
            //Calculo del valor de (α/2)
            double MidpointAlphaValue = (EstimationError / 2);
            //Calculo del valor para Z
            double zValue = Normal.InvCDF(0, 1, oneMinusMidpointAlphaValue);
            //Calculo del valor de X^2 de α/2
            double chiSquarer1 = ChiSquared.InvCDF(RiData.Count - 1, 1 - MidpointAlphaValue);
            //Calculo del valor de Valor para X^2 de 1-(α/2)
            double chiSquarer2 = ChiSquared.InvCDF(RiData.Count - 1, 1 - oneMinusMidpointAlphaValue);
            //Limite inferrior
            double lowerLimit = chiSquarer1 / (12 * (RiData.Count - 1));
            //limite superior
            double upperLimit = chiSquarer2 / (12 * (RiData.Count - 1));
            //Resultado de la prueba
            Boolean isValid = variance >= upperLimit && variance <= lowerLimit;
            //adicionar datos a la lista de resultados
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
