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
        public float radiation { get; set; }

        public float temperature { get; set; }

        public float water_level { get; set; }
    }
}
