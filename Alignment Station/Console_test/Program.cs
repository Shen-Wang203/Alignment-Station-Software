﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;

namespace Console_test
{
    class Program
    {

        static void Main(string[] args)
        {
            Father f = new Father();
            f.Runprint();
            Son s = new Son();
            s.Run();

            
            
        }



    } 
}
