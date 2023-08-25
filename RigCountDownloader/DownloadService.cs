﻿using System.Diagnostics;
using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;

namespace RigCountDownloader
{
	public class DownloadService : IDownloadService
	{
		private readonly IConfiguration _configuration;
		private readonly HttpClient _httpClient;

		public DownloadService(IConfiguration configuration, HttpClient httpClient)
		{
			this._configuration = configuration;
			this._httpClient = httpClient;
			this._httpClient.DefaultRequestHeaders.Add("User-Agent", "RigCountDownloader/1.0");
			this._httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
		}

		public async Task<Stream> DownloadFileAsStreamAsync()
		{
			Uri uri = new(_configuration["BaseAddress"] + _configuration["FileLink"]);

			using HttpRequestMessage request = new(HttpMethod.Get, uri);
			HttpResponseMessage response = new();

			try
			{
				response = await _httpClient.SendAsync(request);
			}
			catch (Exception ex)
			{
				Console.WriteLine("An exception occurred: " + ex.Message);
				Console.WriteLine("Stack Trace: " + ex.StackTrace);
			}

			if (response.IsSuccessStatusCode)
			{
				HttpContent content = response.Content;
				Stream stream = await content.ReadAsStreamAsync();

				return stream;
			}

			return MemoryStream.Null;
		}
	}
}
