using NSubstitute;
using RigCountDownloader.FileConverters;
using RigCountDownloader.StreamProcessors;
using Xunit;

namespace RigCountDownloader.Tests
{
	public class ExcelStreamProcessorTests
	{
		private readonly IFileConverterFactory fileConverterFactory;
		private readonly IFileConverter fileConverter;

		public ExcelStreamProcessorTests()
		{
			fileConverterFactory = Substitute.For<IFileConverterFactory>();
			fileConverter = Substitute.For<ExcelFileConverter>();
		}

		[Fact]
		public async Task ProcessStreamAsync_CorrectMemoryStream_CallsFileConverter()
		{
			// Arrange
			fileConverterFactory.CreateFileConverter().Returns(fileConverter);
			var excelStreamProcessor = new ExcelStreamProcessor(fileConverterFactory);
			using var memoryStream = new MemoryStream();

			// Act
			await excelStreamProcessor.ProcessStreamAsync(memoryStream);

			// Assert
			fileConverterFactory.Received(1).CreateFileConverter();
			await fileConverter.Received(1).ConvertAndSaveAsync();
		}
	}
}
