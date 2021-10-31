using System;

using lena.Domains.Enums;
namespace lena.Models.EntityLogs
{
  public class EntityLogResult
  {
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public DateTime DateTime { get; set; }
    public string Ip { get; set; }
    public string Api { get; set; }
    public string ApiParams { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
