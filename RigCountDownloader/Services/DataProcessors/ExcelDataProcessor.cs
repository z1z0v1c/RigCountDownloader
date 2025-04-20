using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using RigCountDownloader.Domain.Interfaces;
using Serilog;

namespace RigCountDownloader.Services.DataProcessors
{
    public abstract class ExcelDataProcessor(ILogger logger, IConfiguration configuration, ExcelPackage excelPackage)
        : IDataProcessor
    {
        public ILogger Logger { get; set; } = logger;
        public IConfiguration Configuration { get; set; } = configuration;
        public ExcelPackage ExcelPackage { get; set; } = excelPackage;

        public abstract Task ProcessAndSaveAsync(CancellationToken cancellationToken);
    }
}