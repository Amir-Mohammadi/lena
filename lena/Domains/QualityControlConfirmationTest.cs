using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class QualityControlConfirmationTest : IEntity
  {
    protected internal QualityControlConfirmationTest()
    {
    }
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime DateTime { get; set; }
    public int StuffId { get; set; }
    public long QualityControlTestId { get; set; }
    public int TestConditionId { get; set; }// وقتی که داری تست میکنی شرایط تست هم همون لحظه مشخص کن
    public int QualityControlConfirmationId { get; set; }
    public string Description { get; set; }
    public QualityControlConfirmationTestStatus Status { get; set; }
    public double AQLAmount { get; set; }
    public byte[] RowVersion { get; set; }
    public User User { get; set; }
    public TestCondition TestCondition { get; set; }
    public virtual StuffQualityControlTest StuffQualityControlTest { get; set; }
    public virtual QualityControlConfirmation QualityControlConfirmation { get; set; }
    public virtual ICollection<QualityControlConfirmationTestItem> QualityControlConfirmationTestItems { get; set; }
  }
}
