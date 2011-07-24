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

// GPU.NET Example Project : Monte Carlo (C# Excel2007)
// More examples available at http://github.com/tidepowerd

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

using TidePowerd.Example.CSharp.Excel2007.MonteCarlo.OptionPricing;

namespace TidePowerd.Example.CSharp.Excel2007.MonteCarlo
{
    public partial class Sheet1
    {
        #region Fields

        //

        #endregion

        #region Properties

        //

        #endregion

        #region Methods

        //
        private void button1_Click(object sender, EventArgs e)
        {
            //// Get the number of steps per side.
            //int stepsPerSide = (int)Math.Truncate((double)namedRangeParameterStepsPerSide.Value);

            //// If the number of steps per side is less than one (1), show an error MessageBox then return without calling the kernel.
            //if (stepsPerSide < 1)
            //{
            //    MessageBox.Show("The number of steps per side cannot be less than one (1).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            var rrr = this.Cells.GetColumn("A3", "A10");

            var eee = this.Cells.GetColumn("A3", "A10", x => (x as string) ?? String.Empty);

            var www = "weiwji".Length;

            // Call the kernel (via a wrapper method) from a ThreadPool thread -- this keeps the UI responsive while the kernel is running.
            ThreadPool.QueueUserWorkItem(state =>
            {
                // TODO : Get option parameters from the spreadsheet
                //

                // Create an instance of AsianOptionPricingPlan from the spreadsheet data
                var PricingPlan = new AsianOptionPricingPlan(
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    0,
                    0);

                // Calculate the option prices
                float[] OptionPrices = PricingPlan.CalculatePrices();

                // Save the option prices into the spreadsheet
                // TODO
            });
        }

        //
        private void Sheet1_Startup(object sender, System.EventArgs e) { }

        //
        private void Sheet1_Shutdown(object sender, System.EventArgs e) { }

        #endregion


        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.Startup += new System.EventHandler(this.Sheet1_Startup);
            this.Shutdown += new System.EventHandler(this.Sheet1_Shutdown);

        }

        #endregion

    }
}
