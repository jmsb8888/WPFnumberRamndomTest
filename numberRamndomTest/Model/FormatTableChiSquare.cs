using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace numberRamndomTest.Model
{
    class FormatTableChiSquare: IFormatData
    {
        public int Index { get; set; }
        public double beginning { get; set; }
        public double End { get; set; }
        public double ObtainedFrequency { get; set; }
        public double ExpectedFrequency { get; set; }
        public double CHiSquarer { get; set; }
    }
}
