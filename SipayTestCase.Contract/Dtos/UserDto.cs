namespace SipayTestCase.Contract.Dtos;

public class UserDto: IDto
{
    public string Email { get; set; }
    public string? FullName { get; set; }
    public bool IsValidUser { get; set; }
    public string? RecoveryPasswordCode { get; set; }
    public string AuthToken { get; set; }
}