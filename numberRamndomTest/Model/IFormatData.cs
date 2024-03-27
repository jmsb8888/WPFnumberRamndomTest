using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numberRamndomTest.Model
{
    //interfaz para implementacion de grafico
    interface IFormatData
    {
        int Index { get; }
        double ObtainedFrequency { get; }
    }
}
