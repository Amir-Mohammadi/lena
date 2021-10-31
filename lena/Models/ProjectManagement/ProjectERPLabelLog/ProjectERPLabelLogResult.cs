using System;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPLabelLog
{
  public class ProjectERPLabelLogResult
  {
    public int ProjectERPId { get; set; }
    public short ProjectERPLabelId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}