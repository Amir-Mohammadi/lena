using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class WorkStationPart : IEntity
  {
    protected internal WorkStationPart()
    {
      this.OperationSequences = new HashSet<OperationSequence>();
    }
    public short Id { get; set; }
    public string Name { get; set; }
    public short WorkStationId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual WorkStation WorkStation { get; set; }
    public virtual ICollection<OperationSequence> OperationSequences { get; set; }
  }
}