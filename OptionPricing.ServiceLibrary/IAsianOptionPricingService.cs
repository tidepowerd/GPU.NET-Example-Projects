using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace TidePowerd.Example.OptionPricing.ServiceLibrary
{
    /// <summary>
    /// Defines the interface for a service which computes the value of an Asian option.
    /// </summary>
    [ServiceContract]
    public interface IAsianOptionPricingService
    {
        /// <summary>
        /// Computes the value of an Asian option.
        /// </summary>
        /// <param name="parameters">Option parameters used as inputs to the valuation model.</param>
        /// <returns>The computed value of the option.</returns>
        [OperationContract]
        double ComputeValue(OptionPricingParameters parameters);
    }
}
