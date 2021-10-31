using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Calendar : IEntity
  {
    protected internal Calendar()
    {
    }
    public DateTime Date { get; set; }
    public bool IsWorkingDay { get; set; }
    public bool IsHoliday { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
