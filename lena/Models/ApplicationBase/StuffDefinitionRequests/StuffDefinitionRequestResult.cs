using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.StuffDefinitionRequests
{
  public class StuffDefinitionRequestResult
  {
    public int Id { get; set; }
    public string StuffCode { get; set; }
    public string Name { get; set; }
    public string Noun { get; set; }
    public string Title { get; set; }
    public int StuffPurchaseCategoryId { get; set; }
    public string StuffPurchaseCategoryName { get; set; }
    public int UnitTypeId { get; set; }
    public string UnitTypeName { get; set; }
    public StuffType StuffType { get; set; }
    public StuffDefinitionStatus DefinitionStatus { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string EmployeeFullName { get; set; }
    public DateTime DateTime { get; set; }
    public string ConfirmationDescription { get; set; }
    public DateTime? ConfirmDateTime { get; set; }
    public int? ConfirmerUserId { get; set; }
    public string ConfirmerFullName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
