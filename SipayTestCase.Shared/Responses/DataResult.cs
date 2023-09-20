using SipayTestCase.Shared.Interfaces;

namespace SipayTestCase.Shared.Responses;

public class DataResult<T> : Result, IDataResult<T>
{
    public DataResult(bool success, T data) : base(success)
    {
        Data = data;
    }

    public DataResult(bool success, T data, long totalCount) : base(success)
    {
        Data = data;
        TotalCount = totalCount;
    }

    public DataResult(bool success, string message, T data) : base(success, message)
    {
        Data = data;
    }

    public DataResult(bool success, string message, T data, long totalCount) : base(success, message)
    {
        Data = data;
        TotalCount = totalCount;
    }

    public DataResult(bool success, T data, long totalCount, int page, int pageSize, int totalPage) : base(success)
    {
        Data = data;
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
        TotalPage = totalPage;
    }

    public DataResult(bool success, string message, T data, long totalCount, int page, int pageSize, int totalPage) : base(success, message)
    {
        Data = data;
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
        TotalPage = totalPage;
    }

    public T Data { get; set; }
    public long TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPage { get; set; }
}