using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class OperationType : IEntity
  {
    protected internal OperationType()
    {
      this.Operations = new HashSet<Operation>();
    }
    public byte Id { get; set; }
    public string Title { get; set; }
    public byte[] Symbol { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<Operation> Operations { get; set; }
  }
}
