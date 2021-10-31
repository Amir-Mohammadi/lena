using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class DocumentResult : IEntity
  {
    protected internal DocumentResult()
    {
    }
    public Guid Id { get; set; }
    public byte[] FileStream { get; set; }
    public string Name { get; set; }
    public bool IsDirectory { get; set; }
    public string FilePath { get; set; }
    public string FileType { get; set; }
    public DateTime CreationTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
