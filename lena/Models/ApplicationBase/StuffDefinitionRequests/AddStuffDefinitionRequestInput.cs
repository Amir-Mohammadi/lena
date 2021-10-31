using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.StuffDefinitionRequests
{
  public class AddStuffDefinitionRequestInput
  {
    public string Name { get; set; }
    public string Title { get; set; }
    public string Noun { get; set; }
    public int StuffPurchaseCategoryId { get; set; }
    public byte UnitTypeId { get; set; }
    public StuffType StuffType { get; set; }
    public StuffDefinitionStatus DefinitionStatus { get; set; }
    public string Description { get; set; }
  }
}
