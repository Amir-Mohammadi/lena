using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class TestCondition : IEntity
  {
    protected internal TestCondition()
    {
      this.QualityControlConfirmationTests = new HashSet<QualityControlConfirmationTest>();
    }
    public int Id { get; set; }
    public string Condition { get; set; }
    public int UserId { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User User { get; set; }
    public virtual ICollection<QualityControlConfirmationTest> QualityControlConfirmationTests { get; set; }
    public virtual ICollection<QualityControlTestCondition> QualityControlTestConditions { get; set; }
  }
}