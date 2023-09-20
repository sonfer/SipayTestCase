using System.Text;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SipayTestCase.Shared.Helpers;

public class ApiHelper
{
    /// <summary>
    /// /// Belirtilen adrese http GET yapar ve sonucu belirtilen tipte(T) geri döner
    /// </summary>
    /// <param name="api"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Response<T> GetApiResponse<T>(ApiModel api)
    {
        var response = new Response<T>();

        try
        {
            using (var client = new HttpClient())
            {
                HttpClientHeader(client, api);

                int retryCount = 0;

                while (true)
                {
                    var task = client.GetAsync(api.Url)
                        .ContinueWith((taskwithresponse) =>
                        {
                            var apiResponse = taskwithresponse.Result;
                            var exceptionMessage = taskwithresponse.Exception != null
                                ? Helpers.GetInnerException(taskwithresponse.Exception).Message
                                : "";
                            var jsonString = apiResponse.Content.ReadAsStringAsync();
                            jsonString.Wait();
                            if (apiResponse.IsSuccessStatusCode)
                            {
                                response.Success(JsonConvert.DeserializeObject<T>(jsonString.Result),
                                    StatusCodes.Status200OK);
                            }
                            else
                            {
                                response.AddMessage("ApiError_ExceptionMessage", exceptionMessage);
                                response.AddMessage("ApiError_StatusCode", apiResponse.StatusCode.ToString());
                                response.AddMessage("ApiError_Content", jsonString.Result);
                                response.AddMessage("ApiError", apiResponse.ToString());
                                response.Status = ServiceResponseStatuses.Error;
                                response.StatusCode = StatusCodes.Status400BadRequest;
                            }
                        });
                    task.Wait();

                    if (response.IsSuccessful())
                        break;

                    if (retryCount >= api.RetryCount)
                        break;

                    retryCount++;

                    Thread.Sleep(api.WaitAndRetry * 1000);
                }
            }
        }
        catch (Exception ex)
        {
            response.AddMessage("ApiError", Helpers.GetInnerException(ex).Message);
            response.AddMessage("ApiErrorTrace", ex.StackTrace);
            response.Status = ServiceResponseStatuses.Error;
            response.StatusCode = StatusCodes.Status400BadRequest;
        }

        return response;
    }

    /// <summary>
    /// Belirtilen adrese http POST yapar ve sonucu belirtilen tipte(TResponse) geri döner
    /// </summary>
    /// <typeparam name="TResponse">Response Type</typeparam>
    /// <typeparam name="TRequest">Request Type</typeparam>
    /// <param name="request">TRequest typed request object</param>
    /// <param name="api">ApiModel</param>
    /// <returns></returns>
    public static Response<TResponse> PostApiResponse<TResponse, TRequest>(TRequest request, ApiModel api)
    {
        var response = new Response<TResponse>();

        try
        {
            using var client = new HttpClient();
            HttpClientHeader(client, api);

            HttpContent content = api.HttpContentType switch
            {
                HttpContentType.FormDataContent => ObjectToMultipartFormData(request),
                HttpContentType.JsonContent => new StringContent(JsonConvert.SerializeObject(request),
                    Encoding.UTF8, "application/json"),
                HttpContentType.FormUrlEncodedContent => ObjectToFormUrlEncodedContent(request),
                _ => new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8,
                    "application/json")
            };

            int retryCount = 0;

            var contentString = JsonConvert.SerializeObject(request);
            while (true)
            {
                var task = client.PostAsync(api.Url, content)
                    .ContinueWith(postResponse =>
                    {
                        var apiResponse = postResponse.Result;
                        var exceptionMessage = postResponse.Exception != null
                            ? Helpers.GetInnerException(postResponse.Exception).Message
                            : "";
                        var jsonString = apiResponse.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        if (apiResponse.IsSuccessStatusCode)
                        {
                            response.Success(JsonConvert.DeserializeObject<TResponse>(jsonString.Result),
                                StatusCodes.Status200OK);
                        }
                        else
                        {
                            response.AddMessage("ApiError_ExceptionMessage", exceptionMessage);
                            response.AddMessage("ApiError_StatusCode", apiResponse.StatusCode.ToString());
                            response.AddMessage("ApiError_Content", jsonString.Result);
                            response.AddMessage("ApiError", apiResponse.ToString());
                            response.Status = ServiceResponseStatuses.Error;
                            response.StatusCode = StatusCodes.Status400BadRequest;
                        }
                    });

                task.Wait();

                if (response.IsSuccessful())
                    break;

                if (retryCount >= api.RetryCount)
                    break;

                retryCount++;

                Thread.Sleep(api.WaitAndRetry * 1000);
            }
        }
        catch (System.Exception ex)
        {
            response.AddMessage("ApiErrorMessage", Helpers.GetInnerException(ex).Message);
            response.AddMessage("ApiErrorTrace", ex.StackTrace);
            response.Status = ServiceResponseStatuses.Error;
            response.StatusCode = StatusCodes.Status400BadRequest;
        }

        return response;
    }

    /// <summary>
    /// Verilen nesneyi FormUrlEncodedContent'e dönüştürür.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="api"></param>
    private static FormUrlEncodedContent ObjectToFormUrlEncodedContent(object data)
    {
        Dictionary<string, string> keyValues = new Dictionary<string, string>();

        var properties = data.GetType().GetProperties();

        foreach (var property in properties)
        {
            keyValues.Add(property.Name, property.GetValue(data).ToString());
        }

        return new FormUrlEncodedContent(keyValues);
    }


    /// <summary>
    /// Verilen nesneyi MultipartFormDataContent'e dönüştürür.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="api"></param>
    private static MultipartFormDataContent ObjectToMultipartFormData(object data)
    {
        var formData = new MultipartFormDataContent();

        var properties = data.GetType().GetProperties();

        foreach (var property in properties)
        {
            formData.Add(new StringContent(property.GetValue(data).ToString()), property.Name);
        }

        return formData;
    }


    /// <summary>
    /// request içindeki header bilgilerini http header' a ekler.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="api"></param>
    private static void HttpClientHeader(HttpClient client, ApiModel api)
    {
        if (!string.IsNullOrEmpty(api.Token))
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.SetBearerToken(api.Token);
        }
        else if (api.AuthenticationHeaderValue is not null)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = api.AuthenticationHeaderValue;
        }

        if (api.CustomerHeaders is not null && api.CustomerHeaders.Any())
        {
            foreach (var customerHeader in api.CustomerHeaders)
            {
                client.DefaultRequestHeaders.Add(customerHeader.Key, customerHeader.Value);
            }
        }
    }
}