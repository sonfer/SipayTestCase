using System.Runtime.Serialization;
using Microsoft.AspNetCore.Http;

namespace SipayTestCase.Shared.Helpers;

public class Response<T>
{
    public Response()
    {
        Messages = new Dictionary<string, string>();
    }
    
    [DataMember]
    public ServiceResponseStatuses Status { get; set; }
    
    [DataMember]
    public T Data { get; set; }
    
    [DataMember]
    public Dictionary<string, string> Messages { get; set; }
    
    [DataMember]
    public int StatusCode { get; set; }
    
    [DataMember]
    public int TotalCount { get; set; } = 0;
    
    
    public static Response<T> CreateResponse(T data, Dictionary<string, string> messages, int totalCount, int statusCode)
    {
        var response = new Response<T> { Status = ServiceResponseStatuses.Success, Data = data, StatusCode = statusCode, TotalCount = totalCount, Messages = messages };
        return response;
    }
    
    public static Response<T> CreateResponse(T data, int totalCount, int statusCode)
    {
        var response = new Response<T> { Status = ServiceResponseStatuses.Success, Data = data, StatusCode = statusCode, TotalCount = totalCount };
        return response;
    }

    public static Response<T> CreateResponse(T data, int statusCode)
    {
        var response = new Response<T> { Status = ServiceResponseStatuses.Success, Data = data, StatusCode = statusCode };
        return response;
    }

    public static Response<T> CreateResponse(int statusCode)
    {
        var response = new Response<T> { Status = ServiceResponseStatuses.Success, Data = default(T), StatusCode = statusCode };
        return response;
    }
    
    
    public static Response<T> ErrorResponse(string messageKey, string message, int statusCode)
    {
        var dictionary = new Dictionary<string, string> { { messageKey, message } };

        var response = new Response<T> { Status = ServiceResponseStatuses.Error, Messages = dictionary, StatusCode = statusCode };
        return response;
    }

    public static Response<T> ErrorResponse(Dictionary<string, string> messages, int statusCode)
    {
        var response = new Response<T> { Status = ServiceResponseStatuses.Error, Messages = messages, StatusCode = statusCode };
        return response;
    }
    
    public void AddMessage(string messageKey, string message)
    {
        if (this.Messages == null)
        {
            this.Messages = new Dictionary<string, string>();
        }

        var alreadyExists = this.Messages.Any(eachMessage => string.Compare(eachMessage.Key, messageKey, StringComparison.OrdinalIgnoreCase) == 0);

        if (!alreadyExists)
        {
            this.Messages.Add(messageKey, message);
        }
    }

    public bool IsSuccessful()
    {
        this.StatusCode = StatusCodes.Status200OK;
        return this.Status == ServiceResponseStatuses.Success;
    }
    
    public void Error(int statusCode)
    {
        this.StatusCode = statusCode;
        this.Status = ServiceResponseStatuses.Error;
    }
    
    public void Error(string messageKey, string message, int statusCode)
    {
        this.Error(statusCode);
        this.AddMessage(messageKey, message);
    }
    
    public void Error(Dictionary<string, string> messages, int statusCode)
    {
        this.Error(statusCode);
        foreach (var keyValuePair in messages)
        {
            this.AddMessage(keyValuePair.Key, keyValuePair.Value);
        }
    }
    
    public void Success(int statusCode)
    {
        this.StatusCode = statusCode;
        this.Status = ServiceResponseStatuses.Success;
    }
    
    public void Success(T data, int statusCode)
    {
        this.Data = data;
        this.Success(statusCode);
    }
    
    public void Success(T data, int statusCode, bool clearMessages)
    {
        this.Data = data;
        if (clearMessages)
        {
            this.Messages.Clear();
        }
        this.Success(statusCode);
    }
}

[Serializable]
[DataContract]
public enum ServiceResponseStatuses
{
    [EnumMember]
    Error,

    [EnumMember]
    Success,

    [EnumMember]
    Warning
}