using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class QAReviewEmployeeComplain : IEntity
  {
    protected internal QAReviewEmployeeComplain()
    {
    }
    public int Id { get; set; }
    public int EmployeeComplainItemId { get; set; }
    public string ActionDescription { get; set; }
    public int CreatorUserId { get; set; }
    public DateTime DateTime { get; set; }
    public int ActionResponsibleUserId { get; set; }
    public DateTime ActionStartDate { get; set; }
    public DateTime ActionFinishDate { get; set; }
    public string ActionResult { get; set; }
    public QAReviewEmployeeComplainStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual EmployeeComplainItem EmployeeComplainItem { get; set; }
    public virtual User ResponsibleUser { get; set; }
    public virtual User CreatorUser { get; set; }
  }
}