<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.Build.Framework.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.Build.Tasks.v4.0.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\Microsoft.Build.Utilities.v4.0.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ComponentModel.DataAnnotations.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Configuration.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Design.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.Protocols.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.EnterpriseServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Caching.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.ServiceProcess.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.ApplicationServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.RegularExpressions.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Web.Services.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Windows.Forms.dll</Reference>
  <NuGetReference>AntiXSS</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <NuGetReference>System.Text.Encodings.Web</NuGetReference>
  <Namespace>Microsoft.Security.Application</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>System.Text.Encodings.Web</Namespace>
  <Namespace>System.Web</Namespace>
</Query>

void Main()
{
	var url = "http://website.test?param=\"Quoted Value with spaces and &\"";
	Console.WriteLine($"原始網址：{Environment.NewLine}{url}{Environment.NewLine}");
	
	// using System.Web;
	var urlEncoded_HttpUtility = HttpUtility.UrlEncode(url);
	Console.WriteLine($"HttpUtility.UrlEncode 網址：{Environment.NewLine}{urlEncoded_HttpUtility}");
	var urlDecoded_HttpUtility = HttpUtility.UrlDecode(urlEncoded_HttpUtility);
	Console.WriteLine($"HttpUtility.UrlDecode 網址：{Environment.NewLine}{urlDecoded_HttpUtility}");
	var urlUnescapeDataString_Uri = Uri.UnescapeDataString(urlEncoded_HttpUtility);
	Console.WriteLine($"Uri.UnescapeDataString 網址：{Environment.NewLine}{urlUnescapeDataString_Uri}{Environment.NewLine}");

	// using System;
	var urlEscaped_UriEscapeUriString = Uri.EscapeUriString(url);
	Console.WriteLine($"Uri.EscapeUriString 網址：{Environment.NewLine}{urlEscaped_UriEscapeUriString}");
	urlDecoded_HttpUtility = HttpUtility.UrlDecode(urlEscaped_UriEscapeUriString);
	Console.WriteLine($"HttpUtility.UrlDecode 網址：{Environment.NewLine}{urlDecoded_HttpUtility}");
	urlUnescapeDataString_Uri = Uri.UnescapeDataString(urlEscaped_UriEscapeUriString);
	Console.WriteLine($"Uri.UnescapeDataString 網址：{Environment.NewLine}{urlUnescapeDataString_Uri}{Environment.NewLine}");

	// using System;
	var urlEscaped_UriEscapeDataString = Uri.EscapeDataString(url);
	Console.WriteLine($"Uri.EscapeDataString 網址：{Environment.NewLine}{urlEscaped_UriEscapeDataString}");
	urlDecoded_HttpUtility = HttpUtility.UrlDecode(urlEscaped_UriEscapeDataString);
	Console.WriteLine($"HttpUtility.UrlDecode 網址：{Environment.NewLine}{urlDecoded_HttpUtility}");
	urlUnescapeDataString_Uri = Uri.UnescapeDataString(urlEscaped_UriEscapeDataString);
	Console.WriteLine($"Uri.UnescapeDataString 網址：{Environment.NewLine}{urlUnescapeDataString_Uri}{Environment.NewLine}");

	// PM> Install-Package System.Text.Encodings.Web
	// using System.Text.Encodings.Web;
	var urlEncoded_UrlEncoder = UrlEncoder.Default.Encode(url);
	Console.WriteLine($"UrlEncoder.Default.Encode 網址：{Environment.NewLine}{urlEncoded_UrlEncoder}");
	urlDecoded_HttpUtility = HttpUtility.UrlDecode(urlEncoded_UrlEncoder);
	Console.WriteLine($"HttpUtility.UrlDecode 網址：{Environment.NewLine}{urlDecoded_HttpUtility}");
	urlUnescapeDataString_Uri = Uri.UnescapeDataString(urlEncoded_UrlEncoder);
	Console.WriteLine($"Uri.UnescapeDataString 網址：{Environment.NewLine}{urlUnescapeDataString_Uri}{Environment.NewLine}");

	// PM> Install-Package AntiXSS
	// using Microsoft.Security.Application;
	var urlSafe_Sanitizer = Sanitizer.GetSafeHtmlFragment(url);
	Console.WriteLine($"Sanitizer.GetSafeHtmlFragment 網址：{Environment.NewLine}{urlSafe_Sanitizer}");
	urlDecoded_HttpUtility = HttpUtility.UrlDecode(urlSafe_Sanitizer);
	Console.WriteLine($"HttpUtility.UrlDecode 網址：{Environment.NewLine}{urlDecoded_HttpUtility}");
	urlUnescapeDataString_Uri = Uri.UnescapeDataString(urlSafe_Sanitizer);
	Console.WriteLine($"Uri.UnescapeDataString 網址：{Environment.NewLine}{urlUnescapeDataString_Uri}{Environment.NewLine}");
}
