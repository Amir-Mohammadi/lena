using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.BankOrder
{
  public class GetBankOrdersInput : SearchInput<BankOrderSortType>
  {
    public string OrderNumber { get; set; }
    public string FolderCode { get; set; }
    public int[] EmployeeIds { get; set; }
    public int? StuffPriority { get; set; }
    public DateTime? FromRegisterDate { get; set; }
    public DateTime? ToRegisterDate { get; set; }
    public DateTime? FromExpireDate { get; set; }
    public DateTime? ToExpireDate { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public int? ProviderId { get; set; }
    public int? CurrencyId { get; set; }
    public BankOrderStatus? Status { get; set; }
    public BankOrderStatus[] Statuses { get; set; }
    public int[] BankOrderStatusTypes { get; set; }
    public int? BankId { get; set; }
    public int? CustomhouseId { get; set; }
    public int? CountryId { get; set; }
    public BankOrderStatus[] NotHasStatuses { get; set; }

    public GetBankOrdersInput(PagingInput pagingInput, BankOrderSortType sortType, SortOrder sortOrder)
        : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}