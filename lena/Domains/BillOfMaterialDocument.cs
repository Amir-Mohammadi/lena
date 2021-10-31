using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class BillOfMaterialDocument : IEntity
  {
    protected internal BillOfMaterialDocument()
    {
    }
    public int Id { get; set; }
    public short BillOfMaterialVersion { get; set; }
    public int BillOfMaterialStuffId { get; set; }
    public int BillOfMaterialDocumentTypeId { get; set; }
    public string Title { get; set; }
    public string FileName { get; set; }
    public string Description { get; set; }
    public Guid DocumentId { get; set; }
    public byte[] RowVersion { get; set; }
    public int UserId { get; set; }
    public Boolean IsDelete { get; set; } = false;
    public DateTime? DateOfDelete { get; set; }
    public int? DeleteUserId { get; set; }
    public virtual BillOfMaterial BillOfMaterial { get; set; }
    public virtual BillOfMaterialDocumentType BillOfMaterialDocumentType { get; set; }
    public virtual User User { get; set; }
  }
}