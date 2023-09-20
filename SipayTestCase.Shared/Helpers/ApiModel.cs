using System.Net.Http.Headers;

namespace SipayTestCase.Shared.Helpers;

public class ApiModel
{
    public string Url { get; set; }
    public string Token { get; set; }
    public AuthenticationHeaderValue AuthenticationHeaderValue { get; set; }
    public HttpContentType HttpContentType { get; set; }
    public int RetryCount { get; set; } = 0;
    public int WaitAndRetry { get; set; }
    
    public Dictionary<string, string> CustomerHeaders { get; set; }
}

public enum HttpContentType
{
    JsonContent,
    FormDataContent,
    FormUrlEncodedContent
}