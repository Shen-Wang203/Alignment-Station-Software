using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_test
{
    class Father
    {
        public void Runprint()
        {
            Print();
        }

        protected virtual void Print()
        {
            Console.WriteLine("Here is in Father");
        }

    }
}
