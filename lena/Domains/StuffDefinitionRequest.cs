using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public class StuffDefinitionRequest : IEntity
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Noun { get; set; }
    public string Title { get; set; }
    public int StuffPurchaseCategoryId { get; set; }
    public virtual StuffPurchaseCategory StuffPurchaseCategory { get; set; }
    public byte UnitTypeId { get; set; }
    public virtual UnitType UnitType { get; set; }
    public StuffType StuffType { get; set; }
    public StuffDefinitionStatus DefinitionStatus { get; set; }
    public string Description { get; set; }
    public virtual Stuff Stuff { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; }
    public DateTime DateTime { get; set; }
    public int? ConfirmerUserId { get; set; }
    public virtual User ConfirmerUser { get; set; }
    public DateTime? ConfirmDateTime { get; set; }
    public string ConfirmDescription { get; set; }
    public byte[] RowVersion { get; set; }
  }
}