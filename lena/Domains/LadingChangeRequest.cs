using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class LadingChangeRequest : IEntity
  {
    protected internal LadingChangeRequest()
    {
      this.AddLadingItemDetailChanges = new HashSet<AddLadingItemDetailChange>();
      this.EditLadingItemDetailChanges = new HashSet<EditLadingItemDetailChange>();
      this.DeleteLadingItemDetailChanges = new HashSet<DeleteLadingItemDetailChange>();
    }
    public int Id { get; set; }
    public int LadingId { get; set; }
    public int UserId { get; set; } // کاربر درخواست کننده
    public int? ConfirmerUserId { get; set; } //کاربر تایید کننده  
    public DateTime RequestDateTime { get; set; } // تاریخ درخواست ویرایش بارنامه
    public DateTime? ConfirmDateTime { get; set; } // تاریخ تایید/ رد ویرایش بارنامه
    public string Description { get; set; }
    public LadingType LadingType { get; set; }
    public LadingChangeRequestStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual Lading Lading { get; set; }
    public virtual User User { get; set; }
    public virtual User ConfirmerUser { get; set; }
    public virtual ICollection<AddLadingItemDetailChange> AddLadingItemDetailChanges { get; set; }
    public virtual ICollection<EditLadingItemDetailChange> EditLadingItemDetailChanges { get; set; }
    public virtual ICollection<DeleteLadingItemDetailChange> DeleteLadingItemDetailChanges { get; set; }
  }
}
