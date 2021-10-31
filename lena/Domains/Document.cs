using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class Document : IEntity
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double FileSize { get; set; }
    public string FileType { get; set; }
    public byte[] FileStream { get; set; }
    public DateTime CreationTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
