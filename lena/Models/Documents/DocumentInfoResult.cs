using lena.Domains.Enums;
namespace lena.Models.Documents
{
  public class DocumentInfoResult
  {
    public System.Guid Id { get; set; }
    public string FileName { get; set; }
    public string FileType { get; set; }
    public string FullName => $"{FileName}.{FileType}";
    public byte[] RowVersion { get; set; }
  }
}
