using System;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPResponsibleEmployee
{
  public class ProjectERPResponsibleEmployeeComboResult
  {
    public short Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}