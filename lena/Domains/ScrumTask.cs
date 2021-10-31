using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ScrumTask : ScrumEntity, IEntity
  {
    protected internal ScrumTask()
    {
    }
    public int ScrumTaskTypeId { get; set; }
    public Nullable<int> UserId { get; set; }
    public long SpentTime { get; set; }
    public long RemainedTime { get; set; }
    public int ScrumBackLogId { get; set; }
    public ScrumTaskStep ScrumTaskStep { get; set; }
    public virtual User User { get; set; }
    public virtual ScrumBackLog ScrumBackLog { get; set; }
    public virtual ScrumTaskType ScrumTaskType { get; set; }
  }
}