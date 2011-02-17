/*  The MIT License

Copyright (c) 2011 TidePowerd Limited (http://www.tidepowerd.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

*/

// GPU.NET Example Project : Linear Algebra (C#)
// More examples available at http://github.com/tidepowerd

using System;

using TidePowerd.DeviceMethods;

namespace TidePowerd.Example.CSharp.LinearAlgebra
{
    class Program
    {
        static void Main(string[] args)
        {
            // Allocate vectors
            const int VectorLength = 8388608; // 8 * 1024 * 1024 elements
            const int MaxValue = 1000;
            float[] Left = RandFloatVector(VectorLength, MaxValue);
            float[] Right = RandFloatVector(VectorLength, MaxValue);

            // Set grid and block sizes
            // Each GPU thread will process (VectorLength / (BlockDimension.X * GridDimension.X)) elements
            Launcher.SetBlockSize(128);
            Launcher.SetGridSize(50);

            //// Test the kernel methods ////

            float[] VecSum = VectorOps.Add(Left, Right);

            float[] VecDiff = VectorOps.Sub(Left, Right);

            float[] VecHadamardProduct = VectorOps.HadamardProduct(Left, Right);

            //
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Generates a vector filled with random values.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static float[] RandFloatVector(int length, int maxValue)
        {
            // Preconditions
            if (length < 1) { throw new ArgumentOutOfRangeException("length"); }
            else if (maxValue < 1) { throw new ArgumentOutOfRangeException("maxValue"); }

            // Postconditions
            // TODO

            Random rand = new Random();

            float[] Vec = new float[length];
            for (int i = 0; i < Vec.Length; i++)
            {
                Vec[i] = (float)(rand.NextDouble() * maxValue);
            }

            return Vec;
        }
    }
}
