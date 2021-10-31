using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.PermissionRequest
{
  public class GetPermissionRequestInput : SearchInput<PermissionRequestSortType>
  {
    public int Id { get; set; }
    public DateTime? RegisterDateTime { get; set; }
    public DateTime? FromRegisterDateTime { get; set; }
    public DateTime? ToRegisterDateTime { get; set; }
    public int? RegistrarUserId { get; set; }
    public int? ConfirmationUserId { get; set; }
    public int? IntendedUserId { get; set; }

    public GetPermissionRequestInput(PagingInput pagingInput, PermissionRequestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}