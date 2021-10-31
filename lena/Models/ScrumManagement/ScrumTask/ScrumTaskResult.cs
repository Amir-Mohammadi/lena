using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.ScrumManagement.ScrumTask
{
  public class ScrumTaskResult
  {
    public int Id { get; set; }
    public int ScrumBackLogId { get; set; }
    public string Code { get; set; }
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int? UserId { get; set; }
    public string UserName { get; set; }
    public string EmployeeCode { get; set; }
    public string EmployeeName { get; set; }
    public byte[] EmployeePicture { get; set; }
    public long EstimatedTime { get; set; }
    public long RemainedTime { get; set; }
    public long SpentTime { get; set; }
    public int ScrumTaskTypeId { get; set; }
    public string ScrumTaskTypeName { get; set; }
    public ScrumTaskStep Status { get; set; }
    public string Color { get; set; }
    public bool IsCommit { get; set; }
    public bool IsDelete { get; set; }
    public bool IsArchive { get; set; }
    public bool IsMyTask { get; set; }
    public int? PreviousScrumTaskId { get; set; }
    public int? BaseEntityId { get; set; }
    public DateTime CreationDateTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
