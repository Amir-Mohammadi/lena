using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class AddLadingItemDetailChange : IEntity
  {
    protected internal AddLadingItemDetailChange()
    {
    }
    public int Id { get; set; }
    public double Qty { get; set; }
    public int LadingIemId { get; set; }
    public int CargoItemId { get; set; }
    public int CargoItemDetailId { get; set; }
    public int LadingChangeRequestId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual LadingChangeRequest LadingChangeRequest { get; set; }
  }
}