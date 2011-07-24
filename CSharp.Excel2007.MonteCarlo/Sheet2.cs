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
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

using TidePowerd.Example.CSharp.Excel2007.MonteCarlo.OptionPricing;

namespace TidePowerd.Example.CSharp.Excel2007.MonteCarlo
{
    public partial class Sheet2
    {
        //
        private readonly object m_computeLock = new object();

        private void Sheet2_Startup(object sender, System.EventArgs e)
        {
        }

        private void Sheet2_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Sheet2_NumberOfTimeSteps.Change += new Microsoft.Office.Interop.Excel.DocEvents_ChangeEventHandler(this.Sheet2_NumberOfTimeSteps_Change);
            this.Sheet2_NumberOfScenarios.Change += new Microsoft.Office.Interop.Excel.DocEvents_ChangeEventHandler(this.Sheet2_NumberOfScenarios_Change);
            this.Startup += new System.EventHandler(this.Sheet2_Startup);
            this.Shutdown += new System.EventHandler(this.Sheet2_Shutdown);

        }

        #endregion

        private void Sheet2_NumberOfTimeSteps_Change(Excel.Range Target)
        {
            try
            {
                ComputeOptionPrices();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Sheet2_NumberOfScenarios_Change(Excel.Range Target)
        {
            try
            {
                ComputeOptionPrices();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComputeOptionPrices()
        {
            lock (m_computeLock)
            {
                // Get parameter values from the spreadsheet
                var NumberOfTimeStemps = (int)(double)this.Sheet2_NumberOfTimeSteps.Value2;
                var NumberOfScenarios = (int)(double)this.Sheet2_NumberOfScenarios.Value2;

                // Get column data from the spreadsheet.
                var SpotPrices = this.Sheet2_Spot.Cells.GetColumn(x => (float)(double)x);
                var StrikePrices = this.Sheet2_Strike.Cells.GetColumn(x => (float)(double)x);
                var OptionTypes = this.Sheet2_OptionType.Cells.GetColumn(x =>
                    {
                        switch ((x as string) ?? String.Empty)
                        {
                            case "Put": return OptionPricing.OptionType.Put;
                            case "Call": return OptionPricing.OptionType.Call;
                            default: throw new NotSupportedException("Unsupported option type.");
                        }
                    });
                var RiskFreeRates = this.Sheet2_RiskFreeRate.Cells.GetColumn(x => (float)(double)x);
                var Volatilities = this.Sheet2_Volatility.Cells.GetColumn(x => (float)(double)x);
                var Tenors = this.Sheet2_Tenor.Cells.GetColumn(x => (float)(double)x);

                // DEBUG : Make sure all of the arrays have the same length.
                //Debug.Assert(SpotPrices.Length == StrikePrices.Length == OptionTypes.Length == RiskFreeRates.Length == Volatilities.Length == Tenors.Length, "The named ranges have uneven lengths.");

                // Create an AsianOptionPricingPlan from the parsed data
                var OptionPricingPlan = new AsianOptionPricingPlan(SpotPrices, StrikePrices, RiskFreeRates, Volatilities, Tenors, OptionTypes, NumberOfScenarios, NumberOfTimeStemps);

                // Calculate the option prices
                var OptionPrices = OptionPricingPlan.CalculatePrices();

                // Save the calculated option prices to the spreadsheet
                //
                int wwijiwj = "ooowowkw".Length;
            }
        }
    }
}
