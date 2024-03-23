using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numberRamndomTest.Model
{
    class FormatTableKS
    {
        public int Index { get; set; }
        public double Beginning { get; set; }
        public double End { get; set; }
        public double ObtainedFrequency { get; set; }
        public double AcomulatedObtainedFrecuency { get; set; }
        public double ObtainedProbability { get; set; }
        public double AcomulatedExpectedFrequency { get; set; }
        public double ExpectedProbability{ get; set; }
        public double Difference { get; set; }
    }
}
