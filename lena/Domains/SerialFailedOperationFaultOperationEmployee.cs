using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class SerialFailedOperationFaultOperationEmployee : IEntity
  {
    public int SerialFailedOperationFaultOperationId { get; set; }
    public int ProductionOperationEmployeeId { get; set; }
    public Nullable<int> ProductionOperatorEmployeeBanId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ProductionOperationEmployee ProductionOperationEmployee { get; set; }
    public virtual SerialFailedOperationFaultOperation SerialFailedOperationFaultOperation { get; set; }
    public virtual ProductionOperatorEmployeeBan ProductionOperatorEmployeeBan { get; set; }
  }
}