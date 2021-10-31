using System.Collections.Generic;
using System.Linq;
using lena.Services.CryptoMessaging.Model;
using lena.Services.Core;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Terminal.Exception;
using lena.Domains.Enums;
using lena.Services.CryptoMessaging.Crypto;
using Newtonsoft.Json;
using System;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
using core.Data;
namespace lena.Services.Internals.Terminal
{
  public class TerminalManagement : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public TerminalManagement(IRepository repository)
    {
      this.repository = repository;
    }
    public SignedMessage Execute(SignedMessage message)
    {
      var input = message.Params;
      var command = Command.From(input);
      var ticket = "";
      //var ticket = message.Ticket;
      //var sessionId = message.Ssid;
      //var ticketManagement = App.Internals.TicketManagement;
      //var ticketOwner = "PUBLIC";
      //if ((App.Providers.Session.Contains(SessionKey.TerminalSecretKey.ToString())))
      //{
      //    ticketOwner = App.Providers.Session.SessionId;
      //}
      //if (!ticketManagement.IsValid(ticket, ticketOwner))
      //{
      //    throw new InvalidTicketException();
      //}
      //if (!command.IsA(Scheme.Connect.Method) && !command.IsA(Scheme.ConnectAll.Method))
      //{
      //if (!App.Providers.Session.Contains(SessionKey.TerminalSecretKey.ToString()))
      //{
      //    throw new TerminalNotAuthorizedException();
      //}
      //if (!message.IsValid(App.Providers.Session.GetAs<string>(SessionKey.TerminalSecretKey.ToString())))
      //{
      //    throw new InvalidTerminalMessageSignnatureException();
      //}
      //if (sessionId != App.Providers.Session.SessionId)
      //{
      //    throw new TerminalSessionMismatchException();
      //}
      //}
      var paramz = SelectMethod(command);
      return CreateTerminalResponse(paramz);
    }
    private Dictionary<string, string> SelectMethod(Command command)
    {
      if (MatchMethod(command, Scheme.Connect.Method))
      {
        return Connect(command);
      }
      if (MatchMethod(command, Scheme.ConnectAll.Method))
      {
        return ConnectAll(command);
      }
      if (MatchMethod(command, Scheme.EditProductionTerminal.Method))
      {
        return EditProductionTerminal(command);
      }
      if (MatchMethod(command, Scheme.AddProductionOperation.Method))
      {
        return AddProductionOperation(command);
      }
      if (MatchMethod(command, Scheme.SetTerminalEmployee.Method))
      {
        return SetTerminalEmployee(command);
      }
      if (MatchMethod(command, Scheme.SetTerminalOperation.Method))
      {
        return SetTerminalOperation(command);
      }
      if (MatchMethod(command, Scheme.AddPartialProductionOperation.Method))
      {
        return AddPartialProductionOperation(command);
      }
      if (MatchMethod(command, Scheme.RefreshTokenProcess.Method))
      {
        return ProductionTerminalRefreshToken(command);
      }
      throw new TerminalArgumentNotfoundException();
    }
    private SignedMessage CreateTerminalResponse(Dictionary<string, string> paramz)
    {
      var signedMessage = new SignedMessage() { Params = paramz };
      return signedMessage;
    }
    private bool MatchMethod(Command command, Scheme.Definition<string> method)
    {
      return command.Name.ToLower().Equals(method.Key.ToLower());
    }
    private Dictionary<string, string> EditProductionTerminal(Command command)
    {
      var productionLineId = command.Arg(Scheme.EditProductionTerminal.ProductionLineId);
      var terminalOrder = command.Arg(Scheme.EditProductionTerminal.TerminalOrder);
      var networkId = command.Arg(Scheme.EditProductionTerminal.NetworkId);
      var id = networkId * 1000 + terminalOrder;
      string description = networkId + " - " + terminalOrder;
      var productionTerminal = App.Internals.Production.GetProductionTerminal(id: id);
      App.Internals.Production.EditProductionTerminal(productionTerminal: productionTerminal,
                    rowVersion: productionTerminal.RowVersion,
                    productionLineId: productionLineId,
                    description: description);
      return new Dictionary<string, string>();
    }
    private Dictionary<string, string> Connect(Command command)
    {
      var deviceId = command.Arg(Scheme.Connect.DeviceId);
      //var publicKey = command.Arg(Scheme.Connect.ClientPublicKey);
      var productionLineId = command.Arg(Scheme.Connect.ProductionLineId);
      var terminalOrderId = command.Arg(Scheme.Connect.TerminalOrderId);
      var networkId = command.Arg(Scheme.Connect.NetworkId);
      //var handshake = new Handshake();
      //if (deviceId == "esp8266")
      //{
      //    handshake = new Handshake("3581143837", "2");
      //}
      //handshake.ComputeSharedKey(publicKey);
      //var sharedKey = handshake.SharedKeyBase64;
      //App.Providers.Session.Set(SessionKey.TerminalSecretKey.ToString(), sharedKey);
      App.Internals.Production.ConnectProductionTerminal();
      var productionTerminal = App
                .Internals
                .Production
                .AddProductionTerminalProcess(terminalOrder: terminalOrderId, productionLineId: productionLineId, networkId: networkId);
      return new Dictionary<string, string>
          {
                    {"terminal_id", productionTerminal.Id.ToString()},
                    {"server_public_key", "" /*handshake.PublicKey*/}
          };
    }
    private Dictionary<string, string> ConnectAll(Command command)
    {
      var deviceId = command.Arg(Scheme.ConnectAll.DeviceId);
      //var publicKey = command.Arg(Scheme.ConnectAll.ClientPublicKey);
      var productionLineId = command.Arg(Scheme.ConnectAll.ProductionLineId);
      var maxOrderId = command.Arg(Scheme.ConnectAll.MaxOrderId);
      var networkId = command.Arg(Scheme.ConnectAll.NetworkId);
      var terminalOffset = command.Arg(Scheme.ConnectAll.TerminalOffset);
      //var handshake = new Handshake();
      //if (deviceId == "esp8266")
      //{
      //    ////TODO:: change handshake common secret key
      //    handshake = new Handshake("1994", "2");
      //}
      //handshake.ComputeSharedKey(publicKey);
      //var sharedKey = handshake.SharedKeyBase64;
      //App.Providers.Session.Set(SessionKey.TerminalSecretKey.ToString(), sharedKey);
      var loginedUser = App.Internals.Production.ConnectProductionTerminal();
      var productionTerminal = App.Internals.Production
                .AddProductionTerminalsProcess(networkId: networkId, maxcount: maxOrderId, productionLineId: productionLineId, terminalOffset: terminalOffset);
      var terminals = productionTerminal.Select(terminal => new
      {
        ServerId = terminal.Id,
        ProductionLineId = terminal.ProductionLineId,
        EmployeeCode = terminal.Employee == null ? "" : terminal.Employee.Code
      }).ToList();
      return new Dictionary<string, string>
          {
                    {"terminals",  JsonConvert.SerializeObject(terminals)},
                    {"token",JsonConvert.SerializeObject(loginedUser.Token)},
                    {"refresh_token",JsonConvert.SerializeObject(loginedUser.RefreshToken)},
                    {"expires_in",JsonConvert.SerializeObject(loginedUser.ExpiresIn)},
                    {"server_public_key", ""/*handshake.PublicKey*/}
          };
    }
    private Dictionary<string, string> AddProductionOperation(Command command)
    {
      var barcode = command.Arg(Scheme.AddProductionOperation.Barcode);
      var productionOrderId = command.Arg(Scheme.AddProductionOperation.ProductionOrderId);
      var terminalOrderId = command.Arg(Scheme.AddProductionOperation.TerminalOrderId);
      var isFailed = command.Arg(Scheme.AddProductionOperation.IsFailed);
      App.Internals.Production.ProductionTerminalProduction(
                    productionOrderId: productionOrderId,
                    productionTerminalId: terminalOrderId,
                    isFailed: isFailed,
                    serial: barcode);
      return new Dictionary<string, string>();
    }
    private Dictionary<string, string> AddPartialProductionOperation(Command command)
    {
      var count = command.Arg(Scheme.AddPartialProductionOperation.Count);
      var productionOrderId = command.Arg(Scheme.AddPartialProductionOperation.ProductionOrderId);
      var terminalOrderId = command.Arg(Scheme.AddPartialProductionOperation.TerminalOrderId);
      App.Internals.Production.PartialTerminalProduction(
                    productionOrderId: productionOrderId,
                    productionTerminalId: terminalOrderId,
                    count: count
                );
      return new Dictionary<string, string>();
    }
    private Dictionary<string, string> SetTerminalEmployee(Command command)
    {
      var employeeCode = command.Arg(Scheme.SetTerminalEmployee.EmployeeCode);
      var terminalOrder = command.Arg(Scheme.SetTerminalEmployee.TerminalOrderId);
      var productionOrderId = command.Arg(Scheme.SetTerminalEmployee.ProductionOrderId);
      var networkId = command.Arg(Scheme.SetTerminalEmployee.NetworkId);
      var productionTerminalId = terminalOrder;
      var result = App.Internals.Production.SetTerminalEmployee(
                productionOrderId: productionOrderId,
                productionTermialId: productionTerminalId,
                employeeCode: employeeCode);
      return new Dictionary<string, string>
          {
                    {"result",  JsonConvert.SerializeObject(result.Result)},
                    {"employee_code",JsonConvert.SerializeObject(result.EmployeeCode)},
                    {"operation_code",JsonConvert.SerializeObject(result.OperationCode)}
          };
    }
    private Dictionary<string, string> SetTerminalOperation(Command command)
    {
      var operationCode = command.Arg(Scheme.SetTerminalOperation.OperationCode);
      var terminalOrder = command.Arg(Scheme.SetTerminalOperation.TerminalOrderId);
      var productionOrderId = command.Arg(Scheme.SetTerminalEmployee.ProductionOrderId);
      var networkId = command.Arg(Scheme.SetTerminalEmployee.NetworkId);
      var productionTerminalId = terminalOrder;
      var result = App.Internals.Production.SetTerminalOperation(
                productionOrderId: productionOrderId,
                productionTermialId: productionTerminalId,
                operationCode: operationCode);
      return new Dictionary<string, string>
          {
                    {"result",  JsonConvert.SerializeObject(result.Result)},
                    {"employee_code",JsonConvert.SerializeObject(result.EmployeeCode)},
                    {"operation_code",JsonConvert.SerializeObject(result.OperationCode)}
          };
    }
    private Dictionary<string, string> ProductionTerminalRefreshToken(Command command)
    {
      var token = command.Arg(Scheme.RefreshTokenProcess.Token);
      var refreshToken = command.Arg(Scheme.RefreshTokenProcess.RefreshToken);
      var refreshTokenResult = App.Internals.UserManagement.RefreshToken(
                      token: token,
                      refreshToken: refreshToken
                  );
      return new Dictionary<string, string>
          {
                    {"token",JsonConvert.SerializeObject(refreshTokenResult.Token)},
                    {"refresh_token",JsonConvert.SerializeObject(refreshTokenResult.RefreshToken)},
                    {"expires_in",JsonConvert.SerializeObject(refreshTokenResult.ExpiresIn)},
          };
    }
  }
}