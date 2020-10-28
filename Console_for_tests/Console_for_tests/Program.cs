using System;

namespace Console_for_tests
{
    class Program
    {
        static void Main(string[] args)
        {
            BeetleMathModel.set_pivotPoint = new float[] { 0, -2.1f, 5.62f, 0 };
            double[] L = BeetleMathModel.FindAxialPosition(0, 0.6f, 144, 0.2f, -2, 0);
            foreach (float i in BeetleMathModel.set_pivotPoint)
                Console.WriteLine(i);
            foreach (double i in L)
                Console.WriteLine(i);

        }
    }
}
