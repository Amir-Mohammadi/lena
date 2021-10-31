using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class EmployeeWorkDetailView : IEntity
  {
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public DateTime UtcDate { get; set; }
    public string PersianDate { get; set; }
    public string EmployeeCode { get; set; }
    public DateTime? FirstEnterTime { get; set; }
    public DateTime? LastExitTime { get; set; }
    //Minute
    public int WorkTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
