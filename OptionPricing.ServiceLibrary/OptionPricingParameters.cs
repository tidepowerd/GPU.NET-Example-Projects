using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace TidePowerd.Example.OptionPricing.ServiceLibrary
{
    /// <summary>
    /// Parameters for an Asian option valuation simulation.
    /// </summary>
    [DataContract]
    public class OptionPricingParameters
    {
        /// <summary>
        /// The spot price of the option.
        /// </summary>
        [DataMember]
        public double SpotPrice { get; set; }

        /// <summary>
        /// The strike price of the option.
        /// </summary>
        [DataMember]
        public double StrikePrice { get; set; }

        /// <summary>
        /// The risk-free rate to use when calculating the time-value of money.
        /// </summary>
        /// <remarks>
        /// The rate should be provided in 'decimal' form; i.e., for a risk-free
        /// rate of 3.5%, specify the value 0.035d.
        /// </remarks>
        [DataMember]
        public double RiskFreeRate { get; set; }

        /// <summary>
        /// The annualized volatility ("sigma") of the underlying stock.
        /// </summary>
        [DataMember]
        public double Volatility { get; set; }

        /// <summary>
        /// The time-to-maturity ("tenor") of the option.
        /// </summary>
        [DataMember]
        public double Tenor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public OptionKind Kind { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public double TimestepLength { get; set; }
    }
}
