using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class DeleteLadingItemDetailChange : IEntity
  {
    protected internal DeleteLadingItemDetailChange()
    {
    }
    public int Id { get; set; }
    public int LadingIemId { get; set; }
    public int CargoItemId { get; set; }
    public int LadingItemDetailId { get; set; }
    public int LadingChangeRequestId { get; set; }
    public byte[] LadingItemDetailRowVersion { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual LadingChangeRequest LadingChangeRequest { get; set; }
  }
}
