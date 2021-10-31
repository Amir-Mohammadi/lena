using System.Collections.Generic;
using lena.Models.Planning.MachineTypeParameter;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkStationPart
{
  public class WorkStationPartComboResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int WorkStationId { get; set; }
    public IEnumerable<MachineTypeParameterResult> MachineTypeParameters { get; set; }
  }
}
