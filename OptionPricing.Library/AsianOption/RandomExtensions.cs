/*  The MIT License

Copyright (c) 2011-2012 TidePowerd Limited (http://www.tidepowerd.com)

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

// GPU.NET Example Project : Option Pricing Library (C#)
// More examples available at http://github.com/tidepowerd

using System;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;

namespace TidePowerd.Example.OptionPricing.Library.AsianOption
{
    /// <summary>
    /// Extension methods for random-number generation.
    /// </summary>
    internal static class RandomExtensions
    {
        #region Fields

        /// <summary>
        /// The largest possible value of System.UInt64, represented as a double-precision floating-point value.
        /// </summary>
        private const double c_maxUInt64AsDouble = (double)UInt64.MaxValue;

        #endregion

        #region Methods

        /// <summary>
        /// Generates a standard-normally-distributed random sample from two uniformly-distributed sample values.
        /// </summary>
        /// <param name="u1"></param>
        /// <param name="u2"></param>
        /// <returns></returns>
        private static double BoxMuller(double u1, double u2)
        {
            // Preconditions
            Contract.Requires(u1 >= 0.0d && u1 <= 1.0d);
            Contract.Requires(u2 >= 0.0d && u2 <= 1.0d);

            // Compute the random normal sample value from the two random uniform sample values using the Box-Muller transform.
            return Math.Sqrt(-2.0d * Math.Log(u1)) * Math.Cos(2.0d * Math.PI * u2);
        }

        /// <summary>
        /// Returns a standard-normally-distributed random number.
        /// </summary>
        /// <param name="random"></param>
        /// <returns></returns>
        public static double NextDoubleNormal(this Random random)
        {
            // Preconditions
            Contract.Requires(random != null);

            // Get two random uniformly-distributed samples from the RNG;
            // use the Box-Muller transform to combine them into a normally-distributed value.
            return BoxMuller(random.NextDouble(), random.NextDouble());
        }

        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <param name="rngCrypto"></param>
        /// <returns></returns>
        public static double NextDouble(this RNGCryptoServiceProvider rngCrypto)
        {
            // Get random bytes from the RNG.
            var RandBytes = new byte[sizeof(ulong)];
            rngCrypto.GetBytes(RandBytes);

            // Convert the random bytes into an int64, cast that to a double,
            // then divide by the maximum UInt64 value to restrict the output
            // to the range [0, 1].
            return ((double)BitConverter.ToUInt64(RandBytes, 0)) / c_maxUInt64AsDouble;
        }

        /// <summary>
        /// Returns a standard-normally-distributed random number.
        /// </summary>
        /// <param name="rngCrypto"></param>
        /// <returns></returns>
        public static double NextDoubleNormal(this RNGCryptoServiceProvider rngCrypto)
        {
            // Get random bytes from the RNG.
            var RandBytes = new byte[2 * sizeof(ulong)];
            rngCrypto.GetBytes(RandBytes);

            // Convert the random bytes into uniformly-sampled double-precision floats in the range [0, 1].
            var U1 = ((double)BitConverter.ToUInt64(RandBytes, 0)) / c_maxUInt64AsDouble;
            var U2 = ((double)BitConverter.ToUInt64(RandBytes, sizeof(ulong))) / c_maxUInt64AsDouble;

            // Use the Box-Muller transform to create a normally-distributed value.
            return BoxMuller(U1, U2);
        }

        #endregion
    }
}
