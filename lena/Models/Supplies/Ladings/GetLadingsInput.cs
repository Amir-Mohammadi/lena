using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Domains.Enums;
namespace lena.Models.Supplies.Ladings
{
  public class GetLadingsInput : SearchInput<LadingSortType>
  {
    public int? LadingId { get; set; }
    public int[] LadingIds { get; set; }
    public string LadingCode { get; set; }
    public string BankOrderNumber { get; set; }
    public string KotazhCode { get; set; }
    public string SataCode { get; set; }
    public int? EmployeeId { get; set; }
    public int[] EmployeeIds { get; set; }
    public int? UserId { get; set; }

    public bool? HasReceiptLicence { get; set; }
    public int? BankOrderStatusId { get; set; }
    public int? LadingBankOrderStatusId { get; set; }
    public int? CustomhouseStatusId { get; set; }

    public DateTime? FromDeliveryDate { get; set; }

    public DateTime? ToDeliveryDate { get; set; }

    public DateTime? FromTransportDate { get; set; }

    public DateTime? ToTransportDate { get; set; }
    public int? CustomhouseId { get; set; }
    public bool? IsLocked { get; set; }
    public string PlanCode { get; set; }
    public string CargoItemCode { get; set; }
    public LadingType[] Types { get; set; }
    public LadingType[] NotHasTypes { get; set; }

    public GetLadingsInput(PagingInput pagingInput, LadingSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
