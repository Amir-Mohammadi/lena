using System;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.TestCondition
{
  public class TestConditionResult
  {
    public int Id { get; set; }
    public string Condition { get; set; }
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
