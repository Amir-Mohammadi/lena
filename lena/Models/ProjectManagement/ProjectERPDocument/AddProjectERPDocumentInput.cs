using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPDocument
{
  public class AddProjectERPDocumentInput
  {
    public string Description { get; set; }
    public short ProjectERPDocumentTypeId { get; set; }
    public string FileKey { get; set; }
    public UploadFileData UploadFileData { get; set; }
  }
}
