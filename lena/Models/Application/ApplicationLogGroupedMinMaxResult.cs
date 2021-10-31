using System;

using lena.Domains.Enums;
namespace lena.Models.Application
{
  public class ApplicationLogGroupedMinMaxResult
  {
    private DateTime? lastRequestTime;
    private DateTime? firstRequestTime;

    public DateTime Date { get; set; }
    public int? UserId { get; set; }
    public string UserName { get; set; }
    public string EmployeeCode { get; set; }
    public int? EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public DateTime? FirstRequestTime
    {
      get
      {
        return firstRequestTime;
      }
      set
      {
        firstRequestTime = value;
        if (firstRequestTime != null && lastRequestTime != null)
          TotalTime = DateTime.Now.Date.AddMinutes((lastRequestTime - firstRequestTime).Value.TotalMinutes);
      }
    }
    public DateTime? LastRequestTime
    {
      get
      {
        return lastRequestTime;
      }
      set
      {
        lastRequestTime = value;
        if (firstRequestTime != null && lastRequestTime != null)
          TotalTime = DateTime.Now.Date.AddMinutes((lastRequestTime - firstRequestTime).Value.TotalMinutes);
      }
    }
    public DateTime? TotalTime { get; private set; }
  }
}
