using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class TagCounting : IEntity
  {
    protected internal TagCounting()
    {
    }
    public int Id { get; set; }
    public int StockCheckingTagId { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? DeletedAt { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual StockCheckingTag StockCheckingTag { get; set; }
    public virtual Unit Unit { get; set; }
    public virtual User User { get; set; }
  }
}