using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.AutomaticReceiptRegistration
{
  public class GetAutomaticReceiptRegistrationInput : SearchInput<AutomaticReceiptRegistrationSortType>
  {
    public GetAutomaticReceiptRegistrationInput(PagingInput pagingInput, AutomaticReceiptRegistrationSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public string LadingCode { get; set; }

    public ProviderType? ProviderType { get; set; }

    public int? ProviderId { get; set; }

    public DateTime? FromDateTime { get; set; }

    public DateTime? ToDateTime { get; set; }
  }
}
