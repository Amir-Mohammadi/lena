using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class RiskResolve : IEntity
  {
    protected internal RiskResolve()
    {
    }
    public int Id { get; set; }
    public int RiskId { get; set; }
    public string CorrectiveAction { get; set; }
    public int CreatorUserId { get; set; }
    public DateTime DateTime { get; set; }
    public RiskResolveStatus Status { get; set; }
    public Nullable<int> ReviewerUserId { get; set; }
    public Nullable<DateTime> RevieweDateTime { get; set; }
    public string ReviewDescription { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User CreatorUser { get; set; }
    public virtual User ReviewerUser { get; set; }
    public virtual Risk Risk { get; set; }
    public virtual RiskStatus RiskStatus { get; set; }
  }
}