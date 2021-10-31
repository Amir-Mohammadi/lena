using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.QAReviewEmployeeComplain
{
  public class AddQAReviewEmployeeComplainInput
  {
    public int EmployeeComplainItemId { get; set; }
    public DateTime DateTime { get; set; }
    public string ActionDescription { get; set; }
    public int ActionResponsibleUserId { get; set; }
    public DateTime ActionStartDate { get; set; }
    public DateTime ActionFinishDate { get; set; }
    public string ActionResult { get; set; }
    public QAReviewEmployeeComplainStatus Status { get; set; }
  }
}
