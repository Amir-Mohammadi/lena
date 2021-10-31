using System;
namespace core.Models
{
  public interface IFile : IHasRowVersion
  {
    Guid Id { get; set; }
    string FileName { get; set; }
    string FileType { get; set; }
    byte[] FileStream { get; set; }
  }
}