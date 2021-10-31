using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EmployeeOperatorType : IEntity
  {
    protected internal EmployeeOperatorType()
    {
    }
    public int EmployeeId { get; set; }
    public short OperatorTypeId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Employee Employee { get; set; }
    public virtual OperatorType OperatorType { get; set; }
  }
}
