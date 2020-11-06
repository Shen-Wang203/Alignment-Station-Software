using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_test
{
    class PowerMeter
    {
        public static double Read()
        {
            double loss = 0;
            GlobalVar.loss = loss;
            return loss;
        }
    }
}
