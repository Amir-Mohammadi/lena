using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class WeightDay : IEntity
  {
    // وزن روز ها
    protected internal WeightDay()
    {
    }
    public int Id { get; set; }
    public int Day { get; set; }
    public double Amount { get; set; }
    public int IndicatorWeightId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual IndicatorWeight IndicatorWeight { get; set; }
  }
}