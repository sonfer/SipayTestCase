namespace SipayTestCase.Shared.Interfaces;

public interface IResult
{
    bool Success { get; set; }
    string Message { get; set; }
}