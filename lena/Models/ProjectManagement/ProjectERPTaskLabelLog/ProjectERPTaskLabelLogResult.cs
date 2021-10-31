using System;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPTaskLabelLog
{
  public class ProjectERPTaskLabelLogResult
  {
    public int ProjectERPTaskId { get; set; }
    public short ProjectERPLabelId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}