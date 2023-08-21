using System.Diagnostics;
using HtmlAgilityPack;

var url = "https://bakerhughesrigcount.gcs-web.com/intl-rig-count?c=79687&p=irol-rigcountsintl";

var htmlContent = string.Empty;
HttpResponseMessage response = new HttpResponseMessage();

using var httpClient = new HttpClient();
httpClient.Timeout = TimeSpan.FromSeconds(10);
using var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

requestMessage.Headers.Add("User-Agent", "RigCountDownloader/1.0");
requestMessage.Headers.Add("Connection", "keep-alive");


try
{
	response = await httpClient.SendAsync(requestMessage);
}
catch (Exception ex)
{
	Trace.TraceError("An exception occurred: " + ex.Message);
	Trace.TraceError("Stack Trace: " + ex.StackTrace);
}

if (response.IsSuccessStatusCode)
{
	// Get the HTML content as a string
	htmlContent = await response.Content.ReadAsStringAsync();

	Console.WriteLine("HTML content:");
	Console.WriteLine(htmlContent);
}
else
{
	Console.WriteLine($"Failed to get content. Status code: {response.StatusCode}");
}

var htmlDocument = new HtmlDocument();
htmlDocument.LoadHtml(htmlContent);
