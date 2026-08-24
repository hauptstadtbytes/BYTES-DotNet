using BYTES.NET.IO;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Security;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Example of how to use the HTTPClient class
/// Connect to https://hub-mockup.bytescloud.de/v1/users and send GET, POST, DELETE, PATCH requests
/// </summary>
public class HTTPClientCLI
{
    #region main method

    public static async Task Main()
    {
        HttpClient client = CreateHttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Baerer", "ABC123");

        Console.WriteLine("HttpClient created");

        Console.WriteLine("Send GET request...");
        await GetRequest(client);
        Console.WriteLine("--------------");
        Console.WriteLine("Send POST request...");
        await PostRequest(client);
        Console.WriteLine("--------------");
        Console.WriteLine("Send DELETE request...");
        await DeleteRequest(client);
        Console.WriteLine("--------------");
        Console.WriteLine("Send PATCH request...");
        await PatchRequest(client);
    }

    #endregion

    #region constructor 

    static HttpClient CreateHttpClient()
    {
        return new HttpClient();
    }

    #endregion

    #region request methods

    static async Task GetRequest(HttpClient client)
    {
        HttpResponseMessage response = await client.GetAsync("https://hub-mockup.bytescloud.de/v1/users");
        Console.WriteLine(response);
    }

    static async Task PostRequest(HttpClient client)
    {
        string json = JsonSerializer.Serialize(new { token = "ABC123" });
        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        
        HttpResponseMessage response = await client.PostAsync("https://hub-mockup.bytescloud.de/v1/auth", content);
        Console.WriteLine(response);
    }

    static async Task DeleteRequest(HttpClient client)
    {
        HttpResponseMessage response = await client.DeleteAsync("https://hub-mockup.bytescloud.de/v1/sessions");
        Console.WriteLine(response);
    }

    static async Task PatchRequest(HttpClient client)
    {
        CancellationToken cancellationToken = default;

        Uri requestURL = new Uri("https://hub-mockup.bytescloud.de/v1/sessions");

        var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestURL)
        {
            Content = null
        };
        HttpResponseMessage response = await client.SendAsync(request, cancellationToken);
        Console.WriteLine(response);
    }

    #endregion
}