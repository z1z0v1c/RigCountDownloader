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
			string mediaType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			string fileName = "Worldwide Rig Count.xlsx";
			using MemoryStream memoryStream = new();

			Response response = new(mediaType, fileName, memoryStream);

			fileConverterFactory.CreateFileConverter(response).Returns(fileConverter);
			ExcelStreamProcessor excelStreamProcessor = new(fileConverterFactory, response);

			// Act
			await excelStreamProcessor.ProcessStreamAsync(memoryStream);

			// Assert
			fileConverterFactory.Received(1).CreateFileConverter(response);
			await fileConverter.Received(1).ConvertAndSaveAsync();
		}
	}
}
