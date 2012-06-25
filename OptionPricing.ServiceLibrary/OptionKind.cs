using System;
using System.ComponentModel;

namespace TidePowerd.Example.OptionPricing.ServiceLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public enum OptionKind
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("Call")]
        Call = 1,

        /// <summary>
        /// 
        /// </summary>
        [Description("Put")]
        Put = 2
    }
}
