C# Example Projects for <a href="http://www.tidepowerd.com" title="Learn more about GPU.NET at tidepowerd.com">GPU.NET</a>
============================

Project Descriptions
--------------------

*	**[BlackScholes.Cli](http://github.com/tidepowerd/GPU.NET-Example-Projects/tree/master/BlackScholes.Cli/)**

	Generates random option data, then computes the call/put option prices on the GPU using the Black-Scholes model.
	
*	**[BlackScholes.WinForms](http://github.com/tidepowerd/GPU.NET-Example-Projects/tree/master/BlackScholes.WinForms/)**

	The same kernels demonstrated in BlackScholes, but wrapped in a WinForms GUI which uses BackgroundWorker instances to keep the GUI responsive while running the calculations in the background.
	
*	**[LinearAlgebra.Library](http://github.com/tidepowerd/GPU.NET-Example-Projects/tree/master/LinearAlgebra.Library/)**

	Demonstrates GPU-based implementations of basic vector/matrix operations.
	
*	**[OptionPricing.Cli](http://github.com/tidepowerd/GPU.NET-Example-Projects/tree/master/OptionPricing.Cli/)**

	A simple console project which consumes the accelerated OptionPricing.Library project.
	
*	**[OptionPricing.Library](http://github.com/tidepowerd/GPU.NET-Example-Projects/tree/master/OptionPricing.Library/)**

  Demonstrates GPU-based option-valuation techniques. Includes an implemenation of a Monte Carlo-based pricing engine for Asian options.
  
*	**[OptionPricing.ServiceApp](http://github.com/tidepowerd/GPU.NET-Example-Projects/tree/master/OptionPricing.ServiceApp/)**

  A WCF Service Application which exposes the service from the OptionPricing.ServiceLibrary project.
  
*	**[OptionPricing.ServiceLibrary](http://github.com/tidepowerd/GPU.NET-Example-Projects/tree/master/OptionPricing.ServiceLibrary/)**

  A WCF Service Library which wraps the OptionPricing.Library project to expose it through a web service.

*	**[Reduction.Cli](http://github.com/tidepowerd/GPU.NET-Example-Projects/tree/master/Reduction.Cli/)**

	Demonstrates the use of the [SharedMemory] attribute by summing an array of integer values on the GPU.
	
*	**[UnitTesting.Test](http://github.com/tidepowerd/GPU.NET-Example-Projects/tree/master/UnitTesting.Test/)**

	Demonstrates how GPU.NET-accelerated code can be unit-tested like any other .NET code by implementing some simple unit tests within a Visual Studio / MSTest project.

Questions? Ideas for new examples?
----------------------------------
Please <a href="mailto:info@tidepowerd.com" title="Contact TidePowerd for information about GPU.NET">contact us</a> if you have any questions about the example projects or ideas for new example projects!
