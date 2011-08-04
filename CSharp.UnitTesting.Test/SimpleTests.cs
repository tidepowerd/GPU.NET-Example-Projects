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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TidePowerd.Example.CSharp.UnitTesting.Test
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

            // Use LINQ to compare the computed results with the expected values and determine if there are any mismatches.
            var ErrorCount =
                ParallelEnumerable
                .Zip(Results.AsParallel(), InputData.AsParallel().Select(x => Math.Abs(x)), (result, expected) => result != expected)
                .Count(x => x);

            // If any errors were found, the test fails.
            if (ErrorCount > 0)
            {
                throw new AssertFailedException(String.Format("{0} of {1} results did not match their expected value.", ErrorCount, InputData.Length));
            }
        }
    }
}
