using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ReturnSerialToPreviousStateRequest : IEntity
  {
    protected internal ReturnSerialToPreviousStateRequest()
    {
    }
    public int Id { get; set; }
    public string Serial { get; set; }
    public int StuffId { get; set; }
    public long StuffSerialCode { get; set; }
    public int UserId { get; set; } // کاربر درخواست کننده
    public int? ConfirmerUserId { get; set; } //کاربر تایید کننده
    public int? WrongDoerUserId { get; set; } //کاربر خطا کار  
    public DateTime RequestDateTime { get; set; } // تاریخ درخواست 
    public DateTime? ConfirmDateTime { get; set; } // تاریخ تایید/ رد ویرایش 
    public string Description { get; set; }
    public ReturnSerialToPreviousStateRequestStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual StuffSerial StuffSerial { get; set; }
    public virtual User User { get; set; }
    public virtual User ConfirmerUser { get; set; }
    public virtual User WrongDoerUser { get; set; }
  }
}