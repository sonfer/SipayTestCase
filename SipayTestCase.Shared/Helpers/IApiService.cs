namespace SipayTestCase.Shared.Helpers;

public interface IApiService
{
    Response<TResponse> GetService<TResponse>(ApiModel apiModel);
    Response<TResponse> PostService<TResponse, TRequest>(TRequest request, ApiModel apiModel);
}