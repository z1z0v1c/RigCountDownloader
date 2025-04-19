using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using RigCountDownloader.Domain.Interfaces.DataProcessors;
using Serilog;

namespace RigCountDownloader.Services.DataProcessors
{
    public abstract class ExcelDataProcessor : IDataProcessor
    {
        protected ExcelDataProcessor(ILogger logger, IConfiguration configuration, ExcelPackage excelPackage)
        {
            Logger = logger;
            Configuration = configuration;
            ExcelPackage = excelPackage;
        }

        public ILogger Logger { get; set; }  
        public IConfiguration Configuration { get; set; }
        public ExcelPackage ExcelPackage { get; set; }

        public abstract Task ProcessAndSaveAsync();
    }
}