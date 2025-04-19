﻿using NSubstitute;
using RigCountDownloader.FileConverters;
using RigCountDownloader.StreamProcessors;
using Xunit;

namespace RigCountDownloader.Tests
{
	public class XlsxDataConverterTests
	{
		private readonly IDataProcessorFactory _dataProcessorFactory;
		private readonly IDataProcessor _dataProcessor;

		public XlsxDataConverterTests()
		{
			_dataProcessorFactory = Substitute.For<IDataProcessorFactory>();
			_dataProcessor = Substitute.For<ExcelDataProcessor>();
		}

		[Fact]
		public async Task ProcessStreamAsync_CorrectMemoryStream_CallsFileConverter()
		{
			// Arrange
			string mediaType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			string fileName = "Worldwide Rig Count.xlsx";
			using MemoryStream memoryStream = new();

			Response response = new(mediaType, fileName, memoryStream);

			_dataProcessorFactory.CreateFileConverter(response).Returns(_dataProcessor);
			XlsxDataConverter xlsxDataConverter = new(_dataProcessorFactory, response);

			// Act
			await xlsxDataConverter.ConvertDataAsync(memoryStream);

			// Assert
			_dataProcessorFactory.Received(1).CreateFileConverter(response);
			await _dataProcessor.Received(1).ProcessAndSaveAsync();
		}
	}
}
