using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class EnactmentActionProcess : IEntity
  {
    protected internal EnactmentActionProcess()
    {
      this.EnactmentActionProcessLogs = new HashSet<EnactmentActionProcessLog>();
    }
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<EnactmentActionProcessLog> EnactmentActionProcessLogs { get; set; }
  }
}
