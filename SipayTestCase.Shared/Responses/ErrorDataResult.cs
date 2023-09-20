namespace SipayTestCase.Shared.Responses;

public class ErrorDataResult<T> : DataResult<T>
{
    public ErrorDataResult(T data) : base(false, data)
    {

    }

    public ErrorDataResult(T data, int count) : base(false, data, count)
    {

    }

    public ErrorDataResult(string message, T data) : base(false, message, data)
    {

    }

    public ErrorDataResult(string message, T data, int count) : base(false, message, data, count)
    {

    }
}