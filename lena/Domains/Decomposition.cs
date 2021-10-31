using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Decomposition : BaseEntity, IEntity
  {
    protected internal Decomposition()
    {
    }
    public int StuffId { get; set; }
    public Nullable<long> StuffSerialCode { get; set; }
    public int ProductionOperationId { get; internal set; }
    public byte[] RowVersion { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual StuffSerial StuffSerial { get; set; }
    public virtual ProductionOperation ProductionOperation { get; set; }
  }
}