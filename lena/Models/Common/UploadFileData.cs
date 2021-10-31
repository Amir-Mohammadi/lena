using lena.Domains.Enums;
namespace lena.Models.Common
{
  public class UploadFileData
  {
    public string FileName { get; set; }
    public byte[] FileData { get; set; }
  }
}