using lena.Domains.Enums;
using lena.Models.Supplies.LadingBlocker;
using lena.Models.Supplies.LadingItemDetail;
using System;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingChangeRequest
{
  public class LadingChangeRequestResult
  {
    public int Id { get; set; }
    public int DemandantUserId { get; set; } // کاربر درخواست کننده
    public string DemandantEmployeeFullName { get; set; }
    public int? ConfirmerUserId { get; set; } //کاربر تایید کننده  
    public string ConfirmerEmployeeFullName { get; set; }
    public int LadingId { get; set; }
    public string LadingCode { get; set; }
    public DateTime RequestDateTime { get; set; } // تاریخ درخواست ویرایش بارنامه
    public DateTime? ConfirmDateTime { get; set; } // تاریخ تایید/ رد ویرایش بارنامه
    public string Description { get; set; }
    public LadingType LadingType { get; set; }
    public LadingChangeRequestStatus Status { get; set; }
    public byte[] RowVersion { get; set; }

    public IQueryable<AddLadingChangeRequestResult> AddLadingItemDetailChanges { get; set; }
    public IQueryable<EditLadingChangeRequestResult> EditLadingItemDetailChanges { get; set; }
    public IQueryable<DeleteLadingChangeRequestResult> DeleteLadingItemDetailChanges { get; set; }//DeleteLadingItemDetailInput    تغییر دادم


  }
}
