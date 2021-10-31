using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class WorkStation : IEntity
  {
    protected internal WorkStation()
    {
      this.WorkStationParts = new HashSet<WorkStationPart>();
      this.WorkStationOperations = new HashSet<WorkStationOperation>();
      this.OperationSequences = new HashSet<OperationSequence>();
    }
    public short Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ProductionLineId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<WorkStationPart> WorkStationParts { get; set; }
    public virtual ICollection<WorkStationOperation> WorkStationOperations { get; set; }
    public virtual ICollection<OperationSequence> OperationSequences { get; set; }
    public virtual ProductionLine ProductionLine { get; set; }
  }
}