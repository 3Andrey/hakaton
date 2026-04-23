using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1245
{
    internal class reactorResponse
    {
        public dataInfo data { get; set; }

    }

    internal class dataInfo
    {
        public reactor_stateInfo reactor_state { get; set; }
    }
    internal class reactor_stateInfo
    {
        public double radiation { get; set; }

        public double temperature { get; set; }

        public double water_level { get; set; }
    }
}
