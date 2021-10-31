using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StuffQualityControlObservation : IEntity
  {
    protected internal StuffQualityControlObservation()
    {
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string Description { get; set; }
    public DateTime RegisterDateTime { get; set; }
    public int RegisterarUserId { get; set; }
    public int StuffId { get; set; }
    public virtual User RegisterarUser { get; set; }
    public virtual Stuff Stuff { get; set; }
  }
}