namespace SipayTestCase.Shared.Helpers;

public class ApiService: IApiService
{
    public Response<TResponse> GetService<TResponse>(ApiModel apiModel)
    {
        return ApiHelper.GetApiResponse<TResponse>(apiModel);
    }

    public Response<TResponse> PostService<TResponse, TRequest>(TRequest request, ApiModel apiModel)
    {
        throw new NotImplementedException();
    }
}