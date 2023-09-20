namespace SipayTestCase.Domain.Entities;

public class User: BaseEntity
{
    public string Email { get; set; }
    public string? FullName { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public byte[]? PasswordHash { get; set; }
    public bool IsValidUser { get; set; }
    public string? RecoveryPasswordCode { get; set; }
    public string? ActivationCode { get; set; }
}