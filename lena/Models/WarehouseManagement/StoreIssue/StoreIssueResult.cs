using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StoreIssue
{
  public class StoreIssueResult
  {
    public int Id { get; set; }
    public int FromStoreId { get; set; }
    public int ToStoreId { get; set; }

    public string ToStoreName { get; set; }
    public string FromStoreName { get; set; }


    public DateTime SendDate { get; set; }

    public int SenderUserId { get; set; }

    public string SenderUserName { get; set; }


    public StoreIssueState State { get; set; }
    public string StateName => State.ToString();


    public DateTime? ConfirmDate { get; set; }

    public int? ConfirmerUserId { get; set; }

    public string ConfirmerUsername { get; set; }


  }
}
