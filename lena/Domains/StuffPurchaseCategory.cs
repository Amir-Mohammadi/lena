using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class StuffPurchaseCategory : IEntity
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public virtual int StuffDefinitionUserGroupId { get; set; }
    public virtual UserGroup StuffDefinitionUserGroup { get; set; }
    public int QualityControlUserGroupId { get; set; }
    public UserGroup QualityControlUserGroup { get; set; }
    public short QualityControlDepartmentId { get; set; }
    public Department QualityControlDepartment { get; set; }
    public virtual int StuffDefinitionConfirmerUserGroupId { get; set; }
    public virtual UserGroup StuffDefinitionConfirmerUserGroup { get; set; }
    public virtual ICollection<Stuff> Stuffs { get; set; }
    public virtual ICollection<StuffPurchaseCategoryDetail> Details { get; set; }
    public virtual ICollection<StuffDefinitionRequest> StuffDefinitionRequests { get; set; }
    public byte[] RowVersion { get; set; }
  }
}