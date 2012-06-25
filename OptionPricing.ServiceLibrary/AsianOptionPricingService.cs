using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

using TidePowerd.Example.OptionPricing.Library.AsianOption;

namespace TidePowerd.Example.OptionPricing.ServiceLibrary
{
    /// <summary>
    /// 
    /// </summary>
    // For now, only allow one client to access the service at a time.
    // Later, we'll implement some kind of message queue so clients don't wait/timeout, but instead can use some
    // other method to determine when their pricing simulation has completed.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class AsianOptionPricingService : IAsianOptionPricingService
    {
        /// <summary>
        /// The number of simulations to run for each option being priced.
        /// The values computed by each simulation are averaged to determine the
        /// most-likely value of the option at maturity.
        /// </summary>
        private const int c_numberOfSimulations = 10000;

        /// <summary>
        /// The GPU-based pricing engine used to compute option values
        /// via Monte-Carlo simulation.
        /// </summary>
        private static readonly IAsianOptionPricingEngine s_gpuPricingEngine =
            new AsianOptionPricingEngineGPU(c_numberOfSimulations);


        #region IAsianOptionPricingService Members

        double IAsianOptionPricingService.ComputeValue(OptionPricingParameters parameters)
        {
            // Preconditions
            if (parameters == null) { throw new ArgumentNullException("parameters"); }

            // Compute the current price of the option.
            return s_gpuPricingEngine.CalculatePrice(new AsianOptionSingle(
                // TEMP : The pricing engine uses an older bit of code which was only designed for
                // single-precision -- so for now, truncate the input parameters before computing the option value.
                (float)parameters.SpotPrice,
                (float)parameters.StrikePrice,
                (float)parameters.RiskFreeRate,
                (float)parameters.Volatility,
                (float)parameters.Tenor,
                (OptionType)(int)parameters.Kind,
                (float)parameters.TimestepLength
                ));
        }

        #endregion
    }
}
