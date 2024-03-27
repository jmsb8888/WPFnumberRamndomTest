using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace numberRamndomTest.Model
{
    //Clase modelo para obtener el valor de la tabla ks
    //Cuenta con la cantidad de datos y el valor para cada uno de los errores permitidos
    class ModelKS
    {
        public double Quantity { get; set; }
        public double Error020 { get; set; }
        public double Error010 { get; set; }
        public double Error005 { get; set; }
        public double Error002 { get; set; }
        public double Error001 { get; set; }
        public double Error0005 { get; set; }
        public double Error0002 { get; set; }
        public double Error0001 { get; set; }

       //Este metodo extre el valor de la tabla ks para cuando son menos de 50 datos, teneindo en cuenta el error estimado
        public double GetValue(double value)
        {
            double result = 0;
            switch (value)
            {
                case 0.20: result = Error020 ; break;
                case 0.10: result = Error010; break;
                case 0.05: result = Error005; break;
                case 0.02: result = Error002; break;
                case 0.01: result = Error001; break;
                case 0.005: result = Error005; break;
                case 0.002: result = Error002; break;
                case 0.001: result = Error001; break;
            }
            return result;
        }
        //Cuando excede los 50 datos este metodo realiza el calculo del valor KS a emplear teniendo en cuenta el error estimado
        public double CalculateError(double error, int quantity)
        {
            double result = 0;
            switch (error)
            {
                case 0.20: result = (1.07/(Math.Sqrt(quantity))); break;
                case 0.10: result = (1.22 / (Math.Sqrt(quantity))); break;
                case 0.05: result = (1.36 / (Math.Sqrt(quantity))); break;
                case 0.02: result = (1.52 / (Math.Sqrt(quantity))); break;
                case 0.01: result = (1.63 / (Math.Sqrt(quantity))); break;
                case 0.005: result = (1.73 / (Math.Sqrt(quantity))); break;
                case 0.002: result = (1.85 / (Math.Sqrt(quantity))); break;
                case 0.001: result = (1.95 / (Math.Sqrt(quantity))); break;
            }
            return result;
        }
    }
}
