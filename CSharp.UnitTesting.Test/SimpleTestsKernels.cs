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

// GPU.NET Example Project : Unit Testing (C# Test Project)
// More examples available at http://github.com/tidepowerd

using System;
using TidePowerd.DeviceMethods;

namespace TidePowerd.Example.CSharp.UnitTesting.Test
{
    /// <summary>
    /// 
    /// </summary>
    internal static class SimpleTestsKernels
    {
        #region Kernels

        //
        [Kernel]
        private static void Abs(int[] input, int[] output)
        {
            // Thread index
            int ThreadId = BlockDimension.X * BlockIndex.X + ThreadIndex.X;

            // Total number of threads in execution grid
            int TotalThreads = BlockDimension.X * GridDimension.X;

            // Loop over the test cases (input data), process them, and store the results for comparison against the reference values.
            for (int CaseIndex = ThreadId; CaseIndex < input.Length; CaseIndex += TotalThreads)
            {
                output[CaseIndex] = DeviceMath.Abs(input[CaseIndex]);
            }
        }

        #endregion

        #region Wrappers

        //
        public static int[] Abs(int[] input)
        {
            // Set grid/block size for GPU execution
            // NOTE : These settings can be tweaked, but in most cases, we don't much care about the performance of a unit test,
            // so they can be left alone unless the test is crashing due to threads being starved of resources.
            // If the block/grid size is larger than the number of elements in the test vector, that's OK too -- the loop within
            // the kernel ensures that any "extra" threads just idle instead of reading/writing out-of-bounds.
            Launcher.SetBlockSize(128);
            Launcher.SetGridSize(256);

            // Create an array to hold the output of the kernel.
            var Results = new int[input.Length];

            // Call the kernel method.
            Abs(input, Results);

            // Return the results.
            return Results;
        }

        #endregion
    }
}
