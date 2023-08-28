using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using RichardSzalay.MockHttp;
using RigCountDownloader.FileConverters;
using Serilog;

namespace RigCountDownloader.Tests
{
	public class TestFixture
	{
		protected IServiceProvider ServiceProvider { get; private set; }

		public TestFixture()
		{
			var loggerSubstitute = Substitute.For<ILogger>();
			var confSubstitute = Substitute.For<IConfiguration>();

			// Configure dependencies
			var services = new ServiceCollection()
				.AddSingleton(provider => Substitute.For<ILogger>())
				.AddSingleton(provider => Substitute.For<IConfiguration>())
				.AddSingleton<MockHttpMessageHandler>()
				.AddTransient<IFileConverterFactory, FileConverterFactory>();

			ServiceProvider = services.BuildServiceProvider();
		}
	}
}
