using System.Dynamic;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffFractionByProject
{
  public class StuffFractionByProjectResult
  {
    public ExpandoObject[] Data { get; set; }
    public string[] DynamicColumnNames { get; set; }

  }
}
