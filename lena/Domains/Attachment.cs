using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Attachment : IEntity
  {
    protected internal Attachment()
    {
    }
    public int Id { get; set; }
    public int MessageId { get; set; }
    public string FileName { get; set; }
    public double Size { get; set; }
    public string Format { get; set; }
    public byte[] FileContent { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Message Message { get; set; }
  }
}