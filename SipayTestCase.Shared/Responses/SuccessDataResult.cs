namespace SipayTestCase.Shared.Responses;

public class SuccessDataResult<T> : DataResult<T>
{
    public SuccessDataResult(T data) : base(true, data)
    {

    }

    public SuccessDataResult(T data, long totalCount) : base(true, data, totalCount)
    {

    }

    public SuccessDataResult(string message, T data) : base(true, message, data)
    {

    }

    public SuccessDataResult(string message, T data, long totalCount) : base(true, message, data, totalCount)
    {

    }

    public SuccessDataResult(T data, long totalCount, int page, int pageSize, int totalPage) : base(true, data, totalCount, page, pageSize, totalPage)
    {

    }

    public SuccessDataResult(string message, T data, long totalCount, int page, int pageSize, int totalPage) : base(true, message, data, totalCount, page, pageSize, totalPage)
    {

    }
}