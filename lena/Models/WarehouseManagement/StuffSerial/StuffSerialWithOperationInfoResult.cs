using System.ComponentModel.DataAnnotations;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffSerial
{
  //[NotMapped]
  public class StuffSerialWithOperationInfoResult
  {
    [Key]
    public int Id { get; set; }
    public int ProductionId { get; set; }
    public int LastOperationId { get; set; }
    public string LastOperationName { get; set; }
    public int LastProductionOperationId { get; set; }
    public int StuffSerialStuffId { get; set; }
    public long StuffSerialCode { get; set; }
    public string OperatorNames { get; set; }
  }
}
