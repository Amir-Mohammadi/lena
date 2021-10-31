using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionOperatorEmployeeBan : IEntity
  {
    protected internal ProductionOperatorEmployeeBan()
    {
      this.SerialFailedOperationFaultOperationEmployees = new HashSet<SerialFailedOperationFaultOperationEmployee>();
    }
    public int Id { get; set; }
    public int ProductionOperatorId { get; set; }
    public int EmployeeId { get; set; }
    public bool IsBan { get; set; }
    public DateTime BanDateTime { get; set; }
    public Nullable<DateTime> RevokeDateTime { get; set; }
    public Nullable<int> RevokeUserId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ProductionOperator ProductionOperator { get; set; }
    public virtual Employee Employee { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<SerialFailedOperationFaultOperationEmployee> SerialFailedOperationFaultOperationEmployees { get; set; }
  }
}
