using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class QualityControlConfirmationTestItem : IEntity
  {
    public int Id { get; set; }
    public string SampleName { get; set; }
    public int QualityControlConfirmationTestId { get; set; }
    public double? ObtainAmount { get; set; } // مقدار بدست آمده نوع وصفی
    public double? MinObtainAmount { get; set; } // مقدار بدست آمده حداقل
    public double? MaxObtainAmount { get; set; } // مقدار بدست امده حداکثر
    public int TesterUserId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User TesterUser { get; set; }
    public virtual QualityControlConfirmationTest QualityControlConfirmationTest { get; set; }
  }
}
