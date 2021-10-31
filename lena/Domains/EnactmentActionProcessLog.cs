using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EnactmentActionProcessLog : IEntity
  {
    protected internal EnactmentActionProcessLog()
    {
    }
    public int Id { get; set; }
    public int EnactmentId { get; set; }
    public int EnactmentActionProcessId { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }
    public int UserId { get; set; }
    public virtual EnactmentActionProcess EnactmentActionProcess { get; set; }
    public virtual Enactment Enactment { get; set; }
    public virtual User User { get; set; }
  }
}
