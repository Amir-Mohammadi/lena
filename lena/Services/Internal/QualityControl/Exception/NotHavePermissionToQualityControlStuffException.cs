using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl.Exception
{
  public class NotHavePermissionToQualityControlStuffException : InternalServiceException
  {
    public string StuffCode { get; set; }
    public int ValidUserGroupId { get; set; }
    public string ValidUserGroupName { get; set; }

    public NotHavePermissionToQualityControlStuffException(
        string stuffCode,
        int validUserGroupId,
        string validUserGroupName)
    {
      StuffCode = stuffCode;
      ValidUserGroupId = validUserGroupId;
      ValidUserGroupName = validUserGroupName;
    }
  }
}
