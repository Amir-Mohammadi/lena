using System;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionTerminal
{
  public class ConnectProductionTerminalResult
  {
    public string Token { get; set; }

    public string RefreshToken { get; set; }

    public DateTime ExpiresIn { get; set; }
  }
}
