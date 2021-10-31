using System;
namespace core.FileHandler
{
  public interface IUploadedFile
  {
    Guid FileKey { get; set; }
    string FileName { get; set; }
    byte[] Stream { get; set; }
  }
}