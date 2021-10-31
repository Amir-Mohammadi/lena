using lena.Models.Common;
using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.StuffDocument
{
  public class EditStuffDocumentInput
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string FileKey { get; set; }
    public string Description { get; set; }
    public StuffDocumentType StuffDocumentType { get; set; }
    public int UserId { get; set; }
    public string FileName { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
