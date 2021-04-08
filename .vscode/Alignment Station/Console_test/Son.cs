using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_test
{
    class Son : Father
    {
        public void Run()
        {
            Runprint();
        }

        protected override void Print()
        {
            Console.WriteLine("Here is Son");
        }

    }
}
