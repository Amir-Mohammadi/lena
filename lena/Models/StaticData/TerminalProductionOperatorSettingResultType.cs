using lena.Domains.Enums;
namespace lena.Domains.Enums
{
  public static class TerminalProductionOperatorSettingResultType
  {
    public static string Login => "LOGIN";
    public static string Logout => "LOGOUT";
    public static string OperationNotRegistered => "OPERATION_NOT_REGISTERED";
    public static string OperationRegistered => "OPERATION_REGISTERED";
    public static string OperationCleared => "OPERATION_CLEARED";
    public static string NoSuchOperation => "NO_SUCH_OPERATION";
    public static string MultipleSetting => "MULTIPLE_SETTING";
    public static string InvalidOperationCode => "INVALID_OPERATION_CODE";
    public static string InvalidOperatorId => "INVALID_OPERATOR_ID";


  }
}
