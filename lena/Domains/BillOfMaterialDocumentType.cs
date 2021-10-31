using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BillOfMaterialDocumentType : IEntity
  {
    protected internal BillOfMaterialDocumentType()
    {
      this.BillOfMaterialDocuments = new HashSet<BillOfMaterialDocument>();
    }
    public int Id { get; set; }
    public string Description { get; set; }
    public string Title { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<BillOfMaterialDocument> BillOfMaterialDocuments { get; set; }
  }
}
