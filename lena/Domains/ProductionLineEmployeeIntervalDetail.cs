using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionLineEmployeeIntervalDetail : IEntity
  {
    public int Id { get; set; }
    public short OperationId { get; set; }
    public int ProductionLineEmployeeIntervalId { get; set; }
    public long Time { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Operation Operation { get; set; }
    public virtual ProductionLineEmployeeInterval ProductionLineEmployeeInterval { get; set; }
  }
}
