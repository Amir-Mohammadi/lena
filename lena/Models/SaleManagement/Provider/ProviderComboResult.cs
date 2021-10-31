using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.Provider
{
  public class ProviderComboResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string DetailedCode { get; set; }
    public bool ConfirmationDetailedCode { get; set; }

    public ProviderType Type { get; set; }

  }
}
