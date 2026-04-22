using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1245
{
    internal class data
    {
        public reactorStateInfo reactor_state { get; set; }

    }

    internal class reactorStateInfo
    {
        public float water_level { get; set; }

        public float radiation { get; set; }

        public float temperature { get; set; }
    }
}
