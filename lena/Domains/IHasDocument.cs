using System;
using lena.Domains.Enums;
namespace lena.Domains
{
  public interface IHasDocument
  {
    Guid DocumentId { get; set; }
    Document Document { get; set; }
  }
}