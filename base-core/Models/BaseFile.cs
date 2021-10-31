using System;
namespace core.Models
{
  [Serializable]
  public class BaseFile : IFile
  {
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FileType { get; set; }
    public byte[] FileStream { get; set; }
    public byte[] RowVersion { get; set; }
  }
}