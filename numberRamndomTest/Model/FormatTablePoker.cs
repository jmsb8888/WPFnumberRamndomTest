using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numberRamndomTest.Model
{
    //Formato de entrega de los resultados para la prueba de Poker
    class FormatTablePoker
    {
        public string Hand { get; set; } 
        public double ObservedQuantity { get; set; }
        public double Probability { get; set; }
        public double ExpectedProbability { get; set; }
        public double Result { get; set; }
    }
}
