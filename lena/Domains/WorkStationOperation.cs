using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class WorkStationOperation : IEntity
  {
    protected internal WorkStationOperation()
    {
      this.OperationSequences = new HashSet<OperationSequence>();
    }
    public short WorkStationId { get; set; }
    public short OperationId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual WorkStation WorkStation { get; set; }
    public virtual Operation Operation { get; set; }
    public virtual ICollection<OperationSequence> OperationSequences { get; set; }
  }
}