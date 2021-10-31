using lena.Services.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Core.Provider.Storage
{
  public partial class Storage
  {
    protected void OnStart()
    {
      Loader = new DatabaseLoader();
    }

    protected void OnBoot()
    {
      LoggerEnabled = true;
      PersistentLoggerEnabled = true;
      PersistentLoggerRouteBlackList = new string[] {
                "/api/terminal/execute"
            };

      SystemProductionTerminalId = 59;
      SystemEmplyeeId = 20004;
    }


    /// Your Settings

    public bool PersistentLoggerEnabled { get; set; }

    public string[] PersistentLoggerRouteBlackList { get; set; }

    public bool LoggerEnabled { get; set; }
    public bool CheckTransactionBatchNegativeStuffValues { get; set; }
    public bool CheckTransactionBatchNegativeStuffSerialValues { get; set; }
    public bool CheckTransactionBatchFragmentedSerials { get; set; }

    public bool CheckInternalProvider { get; set; }

    public bool CheckForeignProvider { get; set; }

    public string RedisHost { get; set; }
    public int RedisPort { get; set; }
    public int TokenTimeout { get; set; }
    public string Issuer { get; set; }
    public string SecretKey { get; set; }
    public string Encryptkey { get; set; }
    public int SystemProductionTerminalId { get; set; }

    public int SystemEmplyeeId { get; set; }
    public bool ApplicationLogEnabled { get; set; }
    public string NotificationHost { get; set; }
    public int NotificationPort { get; set; }
    public string LoggerHost { get; set; }
    public int LoggerPort { get; set; }
    public string LoggerToken { get; set; }
  }
}
