using System;
using core.FileHandler;
using Microsoft.AspNetCore.Http;
namespace core.Models
{
  public class UploadedFile : IUploadedFile
  {
    public UploadedFile() { }
    public UploadedFile(IFormFile formFile)
    {
      var fileStream = formFile.OpenReadStream();
      Stream = new byte[formFile.Length];
      fileStream.Read(Stream, 0, (int)formFile.Length);
      this.FileKey = Guid.NewGuid();
      FileName = formFile.FileName;
    }
    public Guid FileKey { get; set; }
    public string FileName { get; set; }
    public byte[] Stream { get; set; }
  }
}