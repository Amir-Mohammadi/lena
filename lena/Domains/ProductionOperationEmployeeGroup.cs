using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionOperationEmployeeGroup : IEntity
  {
    protected internal ProductionOperationEmployeeGroup()
    {
      this.ProductionOperationEmployees = new HashSet<ProductionOperationEmployee>();
      this.ProductionOperations = new HashSet<ProductionOperation>();
    }
    public int Id { get; set; }
    public string HashedEmployee { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<ProductionOperationEmployee> ProductionOperationEmployees { get; set; }
    public virtual ICollection<ProductionOperation> ProductionOperations { get; set; }
  }
}
