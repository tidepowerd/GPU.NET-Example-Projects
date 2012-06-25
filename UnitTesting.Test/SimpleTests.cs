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

// GPU.NET Example Project : Unit Testing (C#)
// More examples available at http://github.com/tidepowerd

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TidePowerd.Example.UnitTesting.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class SimpleTests
    {
        //
        [TestMethod]
        public void AbsInt32()
        {
            // Create some test input data
            var InputData = new int[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 };

            // Call the wrapper (which, in turn, calls the kernel we want to test).
            var Results = SimpleTestsKernels.Abs(InputData);

            // Compute the expected values.
            var ExpectedResults = Array.ConvertAll(InputData, x => Math.Abs(x));

            // Determine if the results calculated on the GPU match the expected results.
            /* NOTE :   Alternatively, you can use LINQ to compare the results (or PLINQ,
                        if your test datasets are large). However, be careful to avoid
                        race conditions -- in some cases, LINQ/PLINQ will re-order the
                        computation in such a way that this test will fail even when the
                        calculated results are correct (because the calculated and expected
                        results may not be matched pairwise, as expected). */
            int ErrorCount = 0;
            for (int i = 0; i < InputData.Length; i++)
            {
                if (Results[i] != ExpectedResults[i])
                {
                    ErrorCount++;
                }
            }

            // If any errors were found, the test fails.
            if (ErrorCount > 0)
            {
                throw new AssertFailedException(String.Format("{0} of {1} results did not match their expected value.", ErrorCount, InputData.Length));
            }
        }
    }
}
