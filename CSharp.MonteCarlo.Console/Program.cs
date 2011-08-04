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

// GPU.NET Example Project : Monte Carlo (C# Console)
// More examples available at http://github.com/tidepowerd

using System;
using TidePowerd.Example.CSharp.MonteCarlo.Library.OptionPricing;

namespace TidePowerd.Example.CSharp.MonteCarlo.Console
{
    class Program
    {
        //
        private const int c_initialListCapacity = 100;

        //
        private const int c_numberOfSimulations = 10000;

        //
        private const int c_numberOfTimesteps = 100;

        static void Main(string[] args)
        {
            // Create an AsianOptionPricingEngine to use to compute the option values.
            var PricingEngine = new AsianOptionPricingEngine(c_numberOfSimulations);

            // Create a single option, then compute it's price
            var OptionPrice = PricingEngine.CalculatePrice(new AsianOptionSingle(40.0f, 35.0f, 0.03f, 0.20f, 1.0f / 3.0f, OptionType.Call, 1.0f / 261));

            System.Console.WriteLine("Option Price: {0}", OptionPrice);
            
            const double Tolerance = 0.1;
            if (Math.Abs(OptionPrice - 5.162534) <= Tolerance)
            {
                System.Console.WriteLine("Success. The option price is within the accepted tolerance range of the known value.");
            }
            else
            {
                System.Console.WriteLine("Error! The difference between the computed and known values is greater than the allowed tolerance.");
            }
        }
    }
}
