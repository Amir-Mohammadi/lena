using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class StuffFractionTemporaryStuff : IEntity
  {
    public int Id { get; set; }
    public int StuffId { get; set; }
    public Stuff Stuff { get; set; }
    public string ProjectCode { get; set; }
    public double Qty { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
  }
}