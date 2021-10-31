using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Terminal
{
  public class Scheme
  {
    public static string[] DefinedDevices =
    {
            "pc",
            "esp8266"
        };

    public static string[][] DefinedMethods = {
            new[]
            {
              Connect.Method.Key,
              Connect.DeviceId.Key,
              Connect.TerminalOrderId.Key,
              Connect.ProductionLineId.Key,
              Connect.ClientPublicKey.Key
            },

            new[]
            {
                ConnectAll.Method.Key,
                ConnectAll.DeviceId.Key,
                ConnectAll.MaxOrderId.Key,
                ConnectAll.ProductionLineId.Key,
                ConnectAll.ClientPublicKey.Key
            },

            new[]
            {
              AddProductionOperation.Method.Key,
              AddProductionOperation.Barcode.Key,
              AddProductionOperation.ProductionOrderId.Key,
              AddProductionOperation.TerminalOrderId.Key,
              AddProductionOperation.IsFailed.Key
            } ,
            new[]
            {
                EditProductionTerminal.Method.Key,
                EditProductionTerminal.ProductionLineId.Key,
                EditProductionTerminal.TerminalOrder.Key,
                EditProductionTerminal.NetworkId.Key
            },
            new[]
            {
                SetTerminalEmployee.Method.Key,
                SetTerminalEmployee.TerminalOrderId.Key,
                SetTerminalEmployee.ProductionOrderId.Key,
                SetTerminalEmployee.EmployeeCode.Key
            },
            new[]
            {
                SetTerminalOperation.Method.Key,
                SetTerminalOperation.TerminalOrderId.Key,
                SetTerminalOperation.ProductionOrderId.Key,
                SetTerminalOperation.OperationCode.Key
            },
            new[]
            {
                RefreshTokenProcess.Method.Key,
                RefreshTokenProcess.Token.Key,
                RefreshTokenProcess.RefreshToken.Key
            },

            new[]
            {
              AddPartialProductionOperation.Method.Key,
              AddPartialProductionOperation.Count.Key,
              AddPartialProductionOperation.ProductionOrderId.Key,
              AddPartialProductionOperation.TerminalOrderId.Key
            }
        };

    public static class EditProductionTerminal
    {
      public static readonly Definition<string> Method = new Definition<string>("@edit_production_terminal");

      public static readonly Definition<int> TerminalOrder = new Definition<int>("terminal_order");

      public static readonly Definition<int> ProductionLineId = new Definition<int>("production_line_id");

      public static readonly Definition<int> NetworkId = new Definition<int>("network_id");
    }

    public static class Connect
    {
      public static readonly Definition<string> Method = new Definition<string>("@connect");

      public static readonly Definition<string> DeviceId = new Definition<string>("device_id");

      public static readonly Definition<int> ProductionLineId = new Definition<int>("production_line_id");

      public static readonly Definition<int> TerminalOrderId = new Definition<int>("terminal_order_id");

      public static readonly Definition<string> ClientPublicKey = new Definition<string>("client_public_key");

      public static readonly Definition<int> NetworkId = new Definition<int>("network_id");
    }


    public static class ConnectAll
    {
      public static readonly Definition<string> Method = new Definition<string>("@connect_all");

      public static readonly Definition<string> DeviceId = new Definition<string>("device_id");

      public static readonly Definition<int> ProductionLineId = new Definition<int>("production_line_id");

      public static readonly Definition<int> MaxOrderId = new Definition<int>("max_order_id");

      public static readonly Definition<int> TerminalOffset = new Definition<int>("terminal_offset");

      public static readonly Definition<string> ClientPublicKey = new Definition<string>("client_public_key");

      public static readonly Definition<int> NetworkId = new Definition<int>("network_id");

    }


    public static class AddProductionOperation
    {
      public static readonly Definition<string> Method = new Definition<string>("@add_production_operation");

      public static readonly Definition<int> TerminalOrderId = new Definition<int>("terminal_id");

      public static readonly Definition<int> ProductionOrderId = new Definition<int>("production_order_id");

      public static readonly Definition<bool> IsFailed = new Definition<bool>("is_failed");

      public static readonly Definition<string> Barcode = new Definition<string>("barcode");

    }

    public static class RefreshTokenProcess
    {
      public static readonly Definition<string> Method = new Definition<string>("@refresh_Token");

      public static readonly Definition<string> Token = new Definition<string>("token");

      public static readonly Definition<string> RefreshToken = new Definition<string>("refresh_token");

    }

    public static class SetTerminalEmployee
    {
      public static readonly Definition<string> Method = new Definition<string>("@set_terminal_employee");

      public static readonly Definition<int> NetworkId = new Definition<int>("network_id");

      public static readonly Definition<int> TerminalOrderId = new Definition<int>("terminal_id");

      public static readonly Definition<int> ProductionOrderId = new Definition<int>("production_order_id");

      public static readonly Definition<string> EmployeeCode = new Definition<string>("employee_code");

    }

    public static class AddPartialProductionOperation
    {
      public static readonly Definition<string> Method = new Definition<string>("@add_partial_production_operation");

      public static readonly Definition<int> TerminalOrderId = new Definition<int>("terminal_id");

      public static readonly Definition<int> ProductionOrderId = new Definition<int>("production_order_id");

      public static readonly Definition<int> Count = new Definition<int>("count");

    }

    public static class SetTerminalOperation
    {
      public static readonly Definition<string> Method = new Definition<string>("@set_terminal_operation");

      public static readonly Definition<int> NetworkId = new Definition<int>("network_id");

      public static readonly Definition<int> TerminalOrderId = new Definition<int>("terminal_id");

      public static readonly Definition<int> ProductionOrderId = new Definition<int>("production_order_id");

      public static readonly Definition<string> OperationCode = new Definition<string>("operation_code");

    }




    public class Definition<T>
    {
      public string Key;
      public Type Type;

      public Definition(string key)
      {
        Key = key;
        Type = typeof(T);
      }
    }


  }
}