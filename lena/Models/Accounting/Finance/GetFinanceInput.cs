using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Accounting.Finance
{
  public class GetFinanceInput : SearchInput<FinanceSortType>
  {
    public int? Id { get; set; }
    public string Code { get; set; }
    public int? CooperatorId { get; set; }
    public int? CurrencyId { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDataTime { get; set; }
    public FinanceConfirmationStatus[] Statuses { get; set; }
    public FinanceConfirmationStatus[] NotHasStatuses { get; set; }
    public FinanceTransferStatus? FinanceTransferStatus { get; set; }
    public FinanceTransferStatus? FinanceTransferInDetailStatus { get; set; }
    public GetFinanceInput(PagingInput pagingInput, FinanceSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
