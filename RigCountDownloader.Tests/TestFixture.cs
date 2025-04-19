using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using OfficeOpenXml;
using RichardSzalay.MockHttp;
using RigCountDownloader.Domain.Interfaces.Factories;
using RigCountDownloader.Services.DataProcessors;
using RigCountDownloader.Services.Factories;
using Serilog;

namespace RigCountDownloader.Tests
{
	public class TestFixture
	{
		protected IServiceProvider ServiceProvider { get; private set; }

		public TestFixture()
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			var loggerSubstitute = Substitute.For<ILogger>();
			var confSubstitute = Substitute.For<IConfiguration>();

			// Configure dependencies
			var services = new ServiceCollection()
				.AddSingleton(provider => Substitute.For<ILogger>())
				.AddSingleton(provider => Substitute.For<IConfiguration>())
				.AddSingleton<MockHttpMessageHandler>()
				.AddTransient<IDataProcessorFactory, DataProcessorFactory>()
				.AddTransient<RigCountDataProcessor>();

			ServiceProvider = services.BuildServiceProvider();
		}
	}
}
