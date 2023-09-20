using System.ComponentModel.DataAnnotations;

namespace SipayTestCase.Domain.Entities;

public class BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}