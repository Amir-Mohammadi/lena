using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement
{
  public class GetCooperatorInput
  {
    public bool? ConfirmationDetailedCode { get; set; }
  }
}