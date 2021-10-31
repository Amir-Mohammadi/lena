using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectHeader
{
  public class RestoreProjectHeaderInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
