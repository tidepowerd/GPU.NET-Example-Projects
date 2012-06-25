/*  The MIT License

Copyright (c) 2011-2012 TidePowerd LLC (http://www.tidepowerd.com)

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
using System.Threading.Tasks;
using TidePowerd.DeviceMethods;

namespace TidePowerd.Example.CSharp.LinearAlgebra.Library
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
        public static double[] Add(double[] a, double[] b)
        {
            // Preconditions
            // TODO

            // Postconditions
            // TODO

            // Allocate an array to hold the results
            var Result = new double[a.Length];

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
        public static float[] Add(float[] a, float[] b)
        {
            // Preconditions
            // TODO

            // Postconditions
            // TODO

            // Allocate an array to hold the results
            var Result = new float[a.Length];

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
            var Result = new int[a.Length];

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
        public static long[] Add(long[] a, long[] b)
        {
            // Preconditions
            // TODO

            // Postconditions
            // TODO

            // Allocate an array to hold the results
            var Result = new long[a.Length];

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
        public static double[] HadamardProduct(double[] a, double[] b)
        {
            // Preconditions
            // TODO

            // Postconditions
            // TODO

            // Allocate an array to hold the results
            var Result = new double[a.Length];

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
        /// <returns></returns>
        public static float[] HadamardProduct(float[] a, float[] b)
        {
            // Preconditions
            // TODO

            // Postconditions
            // TODO

            // Allocate an array to hold the results
            var Result = new float[a.Length];

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
        /// <returns></returns>
        public static double[] MultiplyAdd(double[] a, double[] b, double[] c)
        {
            // Preconditions
            // TODO

            // Postconditions
            // TODO

            // Allocate an array to hold the results
            var Result = new double[a.Length];

            // Call the kernel method
            MultiplyAddGpu(a, b, c, Result);

            // Return the result
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static float[] MultiplyAdd(float[] a, float[] b, float[] c)
        {
            // Preconditions
            // TODO

            // Postconditions
            // TODO

            // Allocate an array to hold the results
            var Result = new float[a.Length];

            // Call the kernel method
            MultiplyAddGpu(a, b, c, Result);

            // Return the result
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static long[] MultiplyAdd(int[] a, int[] b, long[] c)
        {
            // Preconditions
            // TODO

            // Postconditions
            // TODO

            // Allocate an array to hold the results
            var Result = new long[a.Length];

            // Call the kernel method
            MultiplyAddGpu(a, b, c, Result);

            // Return the result
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public static double[] Subtract(double[] a, double[] b)
        {
            // Preconditions
            // TODO

            // Postconditions
            // TODO

            // Allocate an array to hold the results
            var Result = new double[a.Length];

            // Call the kernel method
            SubtractGpu(a, b, Result);

            // Return the result
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public static float[] Subtract(float[] a, float[] b)
        {
            // Preconditions
            // TODO

            // Postconditions
            // TODO

            // Allocate an array to hold the results
            var Result = new float[a.Length];

            // Call the kernel method
            SubtractGpu(a, b, Result);

            // Return the result
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public static int[] Subtract(int[] a, int[] b)
        {
            // Preconditions
            // TODO

            // Postconditions
            // TODO

            // Allocate an array to hold the results
            var Result = new int[a.Length];

            // Call the kernel method
            SubtractGpu(a, b, Result);

            // Return the result
            return Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public static long[] Subtract(long[] a, long[] b)
        {
            // Preconditions
            // TODO

            // Postconditions
            // TODO

            // Allocate an array to hold the results
            var Result = new long[a.Length];

            // Call the kernel method
            SubtractGpu(a, b, Result);

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
        private static void AddGpu(double[] a, double[] b, double[] c)
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
        [Kernel(CustomFallbackMethod = "AddCpu")]
        private static void AddGpu(long[] a, long[] b, long[] c)
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
        private static void HadamardProductGpu(double[] a, double[] b, double[] c)
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
        /// <param name="d"></param>
        [Kernel(CustomFallbackMethod = "MultiplyAddCpu")]
        private static void MultiplyAddGpu(double[] a, double[] b, double[] c, double[] d)
        {
            // Get the thread id and total number of threads
            int ThreadId = BlockDimension.X * BlockIndex.X + ThreadIndex.X;
            int TotalThreads = BlockDimension.X * GridDimension.X;

            // Loop over the vectors 'a', 'b', and 'c', multiplying the elements from 'a' and 'b', adding the product to the element from 'c', then storing the result in 'd'.
            for (int ElementIndex = ThreadId; ElementIndex < a.Length; ElementIndex += TotalThreads)
            {
                // Within kernel methods, use the DeviceMath.MultiplyAdd(...) call instead of separate multiply/add steps whenever possible,
                // as this allows the GPU.NET compiler to emit the most-efficient (i.e., fastest-executing) code for your specific device.
                d[ElementIndex] = DeviceMath.MultiplyAdd(a[ElementIndex], b[ElementIndex], c[ElementIndex]);
                //d[ElementIndex] = (a[ElementIndex] * b[ElementIndex]) + c[ElementIndex];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        [Kernel(CustomFallbackMethod = "MultiplyAddCpu")]
        private static void MultiplyAddGpu(float[] a, float[] b, float[] c, float[] d)
        {
            // Get the thread id and total number of threads
            int ThreadId = BlockDimension.X * BlockIndex.X + ThreadIndex.X;
            int TotalThreads = BlockDimension.X * GridDimension.X;

            // Loop over the vectors 'a', 'b', and 'c', multiplying the elements from 'a' and 'b', adding the product to the element from 'c', then storing the result in 'd'.
            for (int ElementIndex = ThreadId; ElementIndex < a.Length; ElementIndex += TotalThreads)
            {
                // Within kernel methods, use the DeviceMath.MultiplyAdd(...) call instead of separate multiply/add steps whenever possible,
                // as this allows the GPU.NET compiler to emit the most-efficient (i.e., fastest-executing) code for your specific device.
                d[ElementIndex] = DeviceMath.MultiplyAdd(a[ElementIndex], b[ElementIndex], c[ElementIndex]);
                //d[ElementIndex] = (a[ElementIndex] * b[ElementIndex]) + c[ElementIndex];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        [Kernel(CustomFallbackMethod = "MultiplyAddCpu")]
        private static void MultiplyAddGpu(int[] a, int[] b, long[] c, long[] d)
        {
            // Get the thread id and total number of threads
            int ThreadId = BlockDimension.X * BlockIndex.X + ThreadIndex.X;
            int TotalThreads = BlockDimension.X * GridDimension.X;

            // Loop over the vectors 'a', 'b', and 'c', multiplying the elements from 'a' and 'b', adding the product to the element from 'c', then storing the result in 'd'.
            for (int ElementIndex = ThreadId; ElementIndex < a.Length; ElementIndex += TotalThreads)
            {
                // Within kernel methods, use the DeviceMath.MultiplyAdd(...) call instead of separate multiply/add steps whenever possible,
                // as this allows the GPU.NET compiler to emit the most-efficient (i.e., fastest-executing) code for your specific device.
                // NOTE : This specific overload ensures the multiplication step doesn't overflow by casting the 'a' and 'b' values to long (int64) before multiplying them.
                d[ElementIndex] = DeviceMath.MultiplyAdd(a[ElementIndex], b[ElementIndex], c[ElementIndex]);
                //d[ElementIndex] = ((long)a[ElementIndex] * (long)b[ElementIndex]) + c[ElementIndex];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        [Kernel(CustomFallbackMethod = "SubtractCpu")]
        private static void SubtractGpu(double[] a, double[] b, double[] c)
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
        [Kernel(CustomFallbackMethod = "SubtractCpu")]
        private static void SubtractGpu(float[] a, float[] b, float[] c)
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
        [Kernel(CustomFallbackMethod = "SubtractCpu")]
        private static void SubtractGpu(int[] a, int[] b, int[] c)
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
        [Kernel(CustomFallbackMethod = "SubtractCpu")]
        private static void SubtractGpu(long[] a, long[] b, long[] c)
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
        private static void AddCpu(double[] a, double[] b, double[] c)
        {
            // Loop over the vectors 'a' and 'b', adding them pairwise and storing the sums in 'c'
            Parallel.For(0, a.Length, ElementIndex =>
            {
                c[ElementIndex] = a[ElementIndex] + b[ElementIndex];
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        private static void AddCpu(float[] a, float[] b, float[] c)
        {
            // Loop over the vectors 'a' and 'b', adding them pairwise and storing the sums in 'c'
            Parallel.For(0, a.Length, ElementIndex =>
            {
                c[ElementIndex] = a[ElementIndex] + b[ElementIndex];
            });
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
            Parallel.For(0, a.Length, ElementIndex =>
            {
                c[ElementIndex] = a[ElementIndex] + b[ElementIndex];
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        private static void AddCpu(long[] a, long[] b, long[] c)
        {
            // Loop over the vectors 'a' and 'b', adding them pairwise and storing the sums in 'c'
            Parallel.For(0, a.Length, ElementIndex =>
            {
                c[ElementIndex] = a[ElementIndex] + b[ElementIndex];
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        private static void HadamardProductCpu(double[] a, double[] b, double[] c)
        {
            // Loop over the vectors 'a' and 'b', multiplying them pairwise and storing the products in 'c'
            Parallel.For(0, a.Length, ElementIndex =>
            {
                c[ElementIndex] = a[ElementIndex] * b[ElementIndex];
            });
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
            Parallel.For(0, a.Length, ElementIndex =>
            {
                c[ElementIndex] = a[ElementIndex] * b[ElementIndex];
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        private static void MultiplyAddCpu(double[] a, double[] b, double[] c, double[] d)
        {
            // Loop over the vectors 'a', 'b', and 'c'; multiply each pair of elements from 'a' and 'b', add the element from 'c' to the product, then store the result in 'd'.
            Parallel.For(0, a.Length, ElementIndex =>
            {
                d[ElementIndex] = (a[ElementIndex] * b[ElementIndex]) + c[ElementIndex];
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        private static void MultiplyAddCpu(float[] a, float[] b, float[] c, float[] d)
        {
            // Loop over the vectors 'a', 'b', and 'c'; multiply each pair of elements from 'a' and 'b', add the element from 'c' to the product, then store the result in 'd'.
            Parallel.For(0, a.Length, ElementIndex =>
            {
                d[ElementIndex] = (a[ElementIndex] * b[ElementIndex]) + c[ElementIndex];
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        private static void MultiplyAddCpu(int[] a, int[] b, long[] c, long[] d)
        {
            // Loop over the vectors 'a', 'b', and 'c'; multiply each pair of elements from 'a' and 'b', add the element from 'c' to the product, then store the result in 'd'.
            Parallel.For(0, a.Length, ElementIndex =>
            {
                // Multiplying two integers could overflow, so we cast them to long *before* the multiplication so we'll get the full product.
                d[ElementIndex] = ((long)a[ElementIndex] * (long)b[ElementIndex]) + c[ElementIndex];
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        private static void SubtractCpu(double[] a, double[] b, double[] c)
        {
            // Loop over the vectors 'a' and 'b', subtracting them pairwise and storing the sums in 'c'
            Parallel.For(0, a.Length, ElementIndex =>
            {
                c[ElementIndex] = a[ElementIndex] - b[ElementIndex];
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        private static void SubtractCpu(float[] a, float[] b, float[] c)
        {
            // Loop over the vectors 'a' and 'b', subtracting them pairwise and storing the sums in 'c'
            Parallel.For(0, a.Length, ElementIndex =>
            {
                c[ElementIndex] = a[ElementIndex] - b[ElementIndex];
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        private static void SubtractCpu(int[] a, int[] b, int[] c)
        {
            // Loop over the vectors 'a' and 'b', subtracting them pairwise and storing the sums in 'c'
            Parallel.For(0, a.Length, ElementIndex =>
            {
                c[ElementIndex] = a[ElementIndex] - b[ElementIndex];
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        private static void SubtractCpu(long[] a, long[] b, long[] c)
        {
            // Loop over the vectors 'a' and 'b', subtracting them pairwise and storing the sums in 'c'
            Parallel.For(0, a.Length, ElementIndex =>
            {
                c[ElementIndex] = a[ElementIndex] - b[ElementIndex];
            });
        }

        #endregion
    }
}
