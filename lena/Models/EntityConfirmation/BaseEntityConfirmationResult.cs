using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.EntityConfirmation
{
  public class BaseEntityConfirmationResult
  {
    public ConfirmationStatus Status { get; set; }
    public string ConfirmDescription { get; set; }
    public int BaseEntityConfirmTypeId { get; set; }
    public long ConfirmingEntityId { get; set; }
    public DateTime ConfirmDateTime { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; }
  }
}
