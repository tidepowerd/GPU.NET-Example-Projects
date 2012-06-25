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

namespace TidePowerd.Example.LinearAlgebra.Library
{
    /// <summary>
    /// Provides extension methods for computing vector norms.
    /// </summary>
    public static class VectorNormExtensions
    {
        // With C# 3.0 and newer, extension methods can serve as wrappers;
        // this means you can "fake" GPU-accelerated instance methods by
        // creating an extension/wrapper method which calls one or more kernels
        // to perform the desired calculations.

        #region Kernels

        //
        private static void OneNormKernel(double[] inputVector, double[] partialResults)
        {
            //
        }

        #endregion

        #region Custom Fallbacks

        //

        #endregion

        #region Wrappers

        //
        public static double OneNorm(this double[] vec)
        {
            // Preconditions
            // TODO

            // Postconditions
            // TODO : Result (vector norm) value must be non-negative.

            // Set grid and block size for kernel
            // TODO

            // Holds partial results
            var PartialResults = new double[128];   // TODO : Set to grid size (which will be based on the vector size)

            // Call the kernel method to partially compute the result.
            //

            // Finish computing the sums on the CPU
            //

            // Return the result
            //

            throw new NotImplementedException();
        }

        #endregion
    }
}
