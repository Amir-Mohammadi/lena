using System;
using System.Collections.Generic;

using lena.Domains.Enums;
namespace lena.Models.UserManagement.PermissionRequest
{
  public class PermissionRequestResult
  {
    public int Id { get; set; }
    public Nullable<DateTime> RegisterDateTime { get; set; }
    public string RegistrarUserFullName { get; set; }
    public string IntendedUserFullName { get; set; }
    public IEnumerable<PermissionRequestActionResult> PermissionRequestActions { get; set; }

  }
}