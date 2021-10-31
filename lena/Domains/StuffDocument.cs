using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffDocument : IEntity
  {
    protected internal StuffDocument()
    {
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid DocumentId { get; set; }
    public int StuffId { get; set; }
    public StuffDocumentType StuffDocumentType { get; set; }
    public int UserId { get; set; }
    public DateTime DateTime { get; set; }
    public string FileName { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual User User { get; set; }
  }
}