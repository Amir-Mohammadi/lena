using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPEventDocument
{
  public class AddProjectERPEventDocumentInput
  {
    public string Description { get; set; }
    public string FileKey { get; set; }
    public UploadFileData UploadFileData { get; set; }
  }
}
