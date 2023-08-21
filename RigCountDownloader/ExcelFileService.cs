using OfficeOpenXml;

namespace RigCountDownloader
{
	public class ExcelFileService : IFileService
	{
		public async Task WriteToFileAsync(HttpContent content)
		{
			using Stream stream = await content.ReadAsStreamAsync();
			using ExcelPackage package = new(stream);

			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			string currentDirectory = Directory.GetCurrentDirectory();

			// Specify the relative path
			string relativePath = "..\\..\\..\\..\\Worldwide Rig Count Jul 2023.xlsx";

			// Combine the current directory and relative path to get the absolute path
			string absolutePath = Path.Combine(currentDirectory, relativePath);

			await File.WriteAllBytesAsync(absolutePath, package.GetAsByteArray());
		}
	}
}
