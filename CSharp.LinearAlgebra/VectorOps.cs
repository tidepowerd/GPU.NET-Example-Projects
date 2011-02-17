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
    /// <summary>
    /// Provides methods for performing basic vector operations.
    /// </summary>
    internal static class VectorOps
    {
        #region Kernel "Wrappers"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public static float[] Add(float[] a, float[] b)
        {
            // Preconditions
            // TODO

            // Postconditions
            // TODO

            // Allocate an array to hold the results
            float[] Result = new float[a.Length];

            // Call the kernel method
            AddGpu(a, b, Result);

            // Return the result
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public static int[] Add(int[] a, int[] b)
        {
            // Preconditions
            // TODO

            // Postconditions
            // TODO

            // Allocate an array to hold the results
            int[] Result = new int[a.Length];

            // Call the kernel method
            AddGpu(a, b, Result);

            // Return the result
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float[] HadamardProduct(float[] a, float[] b)
        {
            // Preconditions
            // TODO

            // Postconditions
            // TODO

            // Allocate an array to hold the results
            float[] Result = new float[a.Length];

            // Call the kernel method
            HadamardProductGpu(a, b, Result);

            // Return the result
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public static float[] Sub(float[] a, float[] b)
        {
            // Preconditions
            // TODO

            // Postconditions
            // TODO

            // Allocate an array to hold the results
            float[] Result = new float[a.Length];

            // Call the kernel method
            SubGpu(a, b, Result);

            // Return the result
            return Result;
        }

        #endregion

        #region GPU Kernel Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        [Kernel(CustomFallbackMethod = "AddCpu")]
        private static void AddGpu(float[] a, float[] b, float[] c)
        {
            // Get the thread id and total number of threads
            int ThreadId = BlockDimension.X * BlockIndex.X + ThreadIndex.X;
            int TotalThreads = BlockDimension.X * GridDimension.X;

            // Loop over the vectors 'a' and 'b', adding them pairwise and storing the sums in 'c'
            for (int ElementIndex = ThreadId; ElementIndex < a.Length; ElementIndex += TotalThreads)
            {
                c[ElementIndex] = a[ElementIndex] + b[ElementIndex];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        [Kernel(CustomFallbackMethod = "AddCpu")]
        private static void AddGpu(int[] a, int[] b, int[] c)
        {
            // Get the thread id and total number of threads
            int ThreadId = BlockDimension.X * BlockIndex.X + ThreadIndex.X;
            int TotalThreads = BlockDimension.X * GridDimension.X;

            // Loop over the vectors 'a' and 'b', adding them pairwise and storing the sums in 'c'
            for (int ElementIndex = ThreadId; ElementIndex < a.Length; ElementIndex += TotalThreads)
            {
                c[ElementIndex] = a[ElementIndex] + b[ElementIndex];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        [Kernel(CustomFallbackMethod = "HadamardProductCpu")]
        private static void HadamardProductGpu(float[] a, float[] b, float[] c)
        {
            // Get the thread id and total number of threads
            int ThreadId = BlockDimension.X * BlockIndex.X + ThreadIndex.X;
            int TotalThreads = BlockDimension.X * GridDimension.X;

            // Loop over the vectors 'a' and 'b', multiplying them pairwise and storing the products in 'c'
            for (int ElementIndex = ThreadId; ElementIndex < a.Length; ElementIndex += TotalThreads)
            {
                c[ElementIndex] = a[ElementIndex] * b[ElementIndex];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        [Kernel(CustomFallbackMethod = "SubCpu")]
        private static void SubGpu(float[] a, float[] b, float[] c)
        {
            // Get the thread id and total number of threads
            int ThreadId = BlockDimension.X * BlockIndex.X + ThreadIndex.X;
            int TotalThreads = BlockDimension.X * GridDimension.X;

            // Loop over the vectors 'a' and 'b', subtracting them pairwise and storing the differences in 'c'
            for (int ElementIndex = ThreadId; ElementIndex < a.Length; ElementIndex += TotalThreads)
            {
                c[ElementIndex] = a[ElementIndex] - b[ElementIndex];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        [Kernel(CustomFallbackMethod = "SubCpu")]
        private static void SubGpu(int[] a, int[] b, int[] c)
        {
            // Get the thread id and total number of threads
            int ThreadId = BlockDimension.X * BlockIndex.X + ThreadIndex.X;
            int TotalThreads = BlockDimension.X * GridDimension.X;

            // Loop over the vectors 'a' and 'b', subtracting them pairwise and storing the differences in 'c'
            for (int ElementIndex = ThreadId; ElementIndex < a.Length; ElementIndex += TotalThreads)
            {
                c[ElementIndex] = a[ElementIndex] - b[ElementIndex];
            }
        }

        #endregion

        #region CPU Fallback Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        private static void AddCpu(float[] a, float[] b, float[] c)
        {
            // Loop over the vectors 'a' and 'b', adding them pairwise and storing the sums in 'c'
            for (int ElementIndex = 0; ElementIndex < a.Length; ElementIndex++)
            {
                c[ElementIndex] = a[ElementIndex] + b[ElementIndex];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        private static void AddCpu(int[] a, int[] b, int[] c)
        {
            // Loop over the vectors 'a' and 'b', adding them pairwise and storing the sums in 'c'
            for (int ElementIndex = 0; ElementIndex < a.Length; ElementIndex++)
            {
                c[ElementIndex] = a[ElementIndex] + b[ElementIndex];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        private static void HadamardProductCpu(float[] a, float[] b, float[] c)
        {
            // Loop over the vectors 'a' and 'b', multiplying them pairwise and storing the products in 'c'
            for (int ElementIndex = 0; ElementIndex < a.Length; ElementIndex++)
            {
                c[ElementIndex] = a[ElementIndex] * b[ElementIndex];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        private static void SubCpu(float[] a, float[] b, float[] c)
        {
            // Loop over the vectors 'a' and 'b', adding them pairwise and storing the sums in 'c'
            for (int ElementIndex = 0; ElementIndex < a.Length; ElementIndex++)
            {
                c[ElementIndex] = a[ElementIndex] - b[ElementIndex];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        private static void SubCpu(int[] a, int[] b, int[] c)
        {
            // Loop over the vectors 'a' and 'b', adding them pairwise and storing the sums in 'c'
            for (int ElementIndex = 0; ElementIndex < a.Length; ElementIndex++)
            {
                c[ElementIndex] = a[ElementIndex] - b[ElementIndex];
            }
        }

        #endregion
    }
}
