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
        //Lista de datos a probar
        List<double> RiData;
        //Lista de resultados
        Dictionary<string, double> ResultData = new Dictionary<string, double>();
        //Error Estimado
        double EstimationError;
        public MeanTest(List<double> RiData, double EstimationError) {
            this.RiData = RiData;
            this.EstimationError = EstimationError;
        }
        //Obtener lista de resultados
        public Dictionary<string, double> GetResults()
        {
            return this.ResultData;
        }
        //Realizar la prueba de medias
        public Boolean TakeTest()
        {
            //Calculo de la media
            double average = Statistics.Mean(RiData);
            //Calculo de 1-α/2
            double oneMinusMidpointAlphaValue = 1 - (EstimationError / 2);
            //calculo del valor de Z
            double zValue = Normal.InvCDF(0, 1, oneMinusMidpointAlphaValue);
            //Limite inferiror permitido
            double lowerLimit = (0.5) - (zValue * (1 / (Math.Sqrt(12 * RiData.Count))));
            //Limite superior permitido
            double upperLimit = (0.5) + (zValue * (1 / (Math.Sqrt(12 * RiData.Count))));
            //Resultado de la prueba
            Boolean isValid = average < upperLimit && average > lowerLimit;
            //Agregar datos a la lsita de resultados
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
