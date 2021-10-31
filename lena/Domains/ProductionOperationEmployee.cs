using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionOperationEmployee : IEntity
  {
    protected internal ProductionOperationEmployee()
    {
      this.SerialFailedOperationFaultOperationEmployees = new HashSet<SerialFailedOperationFaultOperationEmployee>();
    }
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public int ProductionOperationEmployeeGroupId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Employee Employee { get; set; }
    public virtual ProductionOperationEmployeeGroup GetProductionOperationEmployeeGroup { get; set; }
    public virtual ICollection<SerialFailedOperationFaultOperationEmployee> SerialFailedOperationFaultOperationEmployees { get; set; }
  }
}
