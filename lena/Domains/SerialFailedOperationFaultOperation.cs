using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class SerialFailedOperationFaultOperation : IEntity
  {
    public SerialFailedOperationFaultOperation()
    {
      this.SerialFailedOperationFaultOperationEmployees = new HashSet<SerialFailedOperationFaultOperationEmployee>();
    }
    public int Id { get; set; }
    public int SerialFailedOperationId { get; set; }
    public short OperationId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual SerialFailedOperation SerialFailedOperation { get; set; }
    public virtual Operation Operation { get; set; }
    public virtual ICollection<SerialFailedOperationFaultOperationEmployee> SerialFailedOperationFaultOperationEmployees { get; set; }
  }
}