using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class StuffPurchaseCategoryDetail : IEntity
  {
    public int Id { get; set; }
    public int ApplicatorUserGroupId { get; set; }
    public UserGroup ApplicatorUserGroup { get; set; }
    public int ApplicatorConfirmerUserGroupId { get; set; }
    public UserGroup ApplicatorConfirmerUserGroup { get; set; }
    public int? RequestConfirmerUserGroupId { get; set; }
    public UserGroup RequestConfirmerUserGroup { get; set; }
    public int StuffPurchaseCategoryId { get; set; }
    public StuffPurchaseCategory StuffPurchaseCategory { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
