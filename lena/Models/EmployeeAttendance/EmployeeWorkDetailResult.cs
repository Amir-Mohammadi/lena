using System;

using lena.Domains.Enums;
namespace lena.Models.EmployeeAttendance
{
  public class EmployeeWorkDetailResult
  {
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public DateTime UtcDate { get; set; }
    public string PersianDate { get; set; }
    public int? EmployeeId { get; set; }
    public string EmployeeCode { get; set; }
    public string EmployeeFullName { get; set; }
    public DateTime? FirstEnterTime { get; set; }
    public DateTime? LastExitTime { get; set; }
    //Minute
    public int WorkTime { get; set; }
  }
}
