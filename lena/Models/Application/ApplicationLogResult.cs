using System;

using lena.Domains.Enums;
namespace lena.Models.Application
{
  public class ApplicationLogResult
  {
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string UserName { get; set; }
    public int? EmployeeId { get; set; }
    public string EmployeeCode { get; set; }
    public string EmployeeFullName { get; set; }
    public string ClientIp { get; set; }
    public string UserAgent { get; set; }
    public string Action { get; set; }
    public DateTime LogTime { get; set; }
    public DateTime? RequestEndTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
