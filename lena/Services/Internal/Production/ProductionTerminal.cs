using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Production.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Production.ProductionTerminal;
using lena.Models.UserManagement.User;
using lena.Models.Production.Production;
using lena.Services.Core.TokenManager;
using System.Configuration;
using lena.Models.Production.ProductionLineEmployeeInterval;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Add
    public ProductionTerminal AddProductionTerminal(
        int id,
        int productionLineId,
        int networkId,
        string description)
    {
      var productionTerminal = repository.Create<ProductionTerminal>();
      productionTerminal.Id = id;
      productionTerminal.Type = ProductionTerminalType.Complete;
      productionTerminal.ProductionLineId = productionLineId;
      productionTerminal.NetworkId = networkId;
      productionTerminal.Description = description;
      repository.Add(productionTerminal);
      return productionTerminal;
    }
    #endregion
    #region AddProcess
    public List<ProductionTerminal> AddProductionTerminalsProcess(int networkId, int productionLineId, int maxcount, int terminalOffset)
    {
      List<ProductionTerminal> productionTerminals = new List<ProductionTerminal>();
      maxcount = maxcount + terminalOffset;
      for (int i = terminalOffset; i < maxcount; i++)
      {
        var terminal = AddProductionTerminalProcess(networkId: networkId, terminalOrder: i, productionLineId: productionLineId);
        productionTerminals.Add(terminal);
      }
      return productionTerminals;
    }
    public ProductionTerminal AddProductionTerminalProcess(
        int? terminalOrder,
        int productionLineId,
        int networkId)
    {
      ProductionTerminal productionTerminal = null;
      #region Generate terminalOrder if is null
      if (!terminalOrder.HasValue)
      {
        var query = GetProductionTerminals(
                      selector: i => i.Id - (networkId * 1000),
                      networkId: networkId);
        query = query.Where(i => i > 255);
        terminalOrder = query.Any() ? query.Max() : 255;
        terminalOrder++;
      }
      #endregion
      #region Generate id
      var id = networkId * 1000 + terminalOrder.Value;
      #endregion
      #region GetProductionTerminal
      productionTerminal = GetProductionTerminals(
              selector: e => e,
              id: id)
          .FirstOrDefault();
      #endregion
      #region AddProductionTerminal
      if (productionTerminal == null)
      {
        productionTerminal = AddProductionTerminal(
                      id: id,
                      productionLineId: productionLineId,
                      networkId: networkId,
                      description: networkId + " - " + terminalOrder);
      }
      #endregion
      return productionTerminal;
    }
    #endregion
    #region Edit
    public ProductionTerminal EditProductionTerminal(
        int id,
        byte[] rowVersion,
        TValue<ProductionTerminalType> productionTerminalType = null,
        TValue<bool> isActive = null,
        TValue<int> productionLineId = null,
        TValue<int> networkId = null,
        TValue<int?> employeeId = null,
        TValue<string> description = null)
    {
      var productionTerminal = GetProductionTerminal(id: id);
      return EditProductionTerminal(
                    productionTerminal: productionTerminal,
                    rowVersion: rowVersion,
                    isActive: isActive,
                    productionLineId: productionLineId,
                    networkId: networkId,
                    employeeId: employeeId,
                    description: description);
    }
    public ProductionTerminal EditProductionTerminal(
        ProductionTerminal productionTerminal,
        byte[] rowVersion,
        TValue<ProductionTerminalType> productionTerminalType = null,
        TValue<bool> isActive = null,
        TValue<int> productionLineId = null,
        TValue<int> networkId = null,
         TValue<int?> employeeId = null,
        TValue<string> description = null)
    {
      if (productionTerminalType != null)
        productionTerminal.Type = productionTerminalType;
      if (isActive != null)
        productionTerminal.IsActive = isActive;
      if (productionLineId != null)
        productionTerminal.ProductionLineId = productionLineId;
      if (networkId != null)
        productionTerminal.NetworkId = networkId;
      if (employeeId != null)
      {
        if (employeeId.Value == null)
        {
          productionTerminal.Employee = null;
        }
        else
        {
          var employee = App.Internals.UserManagement.GetEmployee(id: employeeId.Value.Value);
          productionTerminal.Employee = employee;
        }
      }
      if (description != null)
        productionTerminal.Description = description;
      repository.Update<ProductionTerminal>(productionTerminal, rowVersion);
      return productionTerminal;
    }
    #endregion
    #region Get
    public ProductionTerminal GetProductionTerminal(int id) => GetProductionTerminal(id: id, selector: e => e);
    public TResult GetProductionTerminal<TResult>(
        Expression<Func<ProductionTerminal, TResult>> selector,
        int id)
    {
      var productionTerminal = GetProductionTerminals(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (productionTerminal == null)
        throw new ProductionTerminalNotFoundException(id);
      return productionTerminal;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionTerminals<TResult>(
        Expression<Func<ProductionTerminal, TResult>> selector,
        TValue<int> id = null,
        TValue<int> networkId = null,
        TValue<bool> isActive = null,
        TValue<int> productionLineId = null,
        TValue<int> EmployeeId = null,
        TValue<string> description = null)
    {
      var query = repository.GetQuery<ProductionTerminal>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (EmployeeId != null)
        query = query.Where(i => i.Employee.Id == id);
      if (networkId != null)
        query = query.Where(i => i.NetworkId == networkId);
      if (productionLineId != null)
        query = query.Where(i => i.ProductionLineId == productionLineId);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (isActive != null)
        query = query.Where(i => i.IsActive == isActive);
      return query.Select(selector);
    }
    #endregion
    #region ToResult
    public Expression<Func<ProductionTerminal, ProductionTerminalResult>> ToProductionTerminalResult =
        entity => new ProductionTerminalResult()
        {
          Id = entity.Id,
          IsActive = entity.IsActive,
          Description = entity.Description,
          ProductionLineId = entity.ProductionLineId,
          ProductionLineName = entity.ProductionLine.Name,
          NetworkId = entity.NetworkId,
          RowVersion = entity.RowVersion
        };
    #endregion
    #region ToComboResult
    public Expression<Func<ProductionTerminal, ProductionTerminalComboResult>> ToProductionTerminalComboResult =
        entity => new ProductionTerminalComboResult()
        {
          Id = entity.Id,
          Type = entity.Type,
          Description = entity.Description
        };
    #endregion
    #region Search
    public IQueryable<ProductionTerminalResult> SearchProductionTerminalResult(
        IQueryable<ProductionTerminalResult> query,
    string searchText,
    AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from pTerminal in query
                where pTerminal.Description.Contains(searchText) ||
                      pTerminal.ProductionLineName.Contains(searchText)
                select pTerminal;
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ProductionTerminalResult> SortProductionTerminalResult(
        IQueryable<ProductionTerminalResult> query,
        SortInput<ProductionTerminalSortType> sortInput)
    {
      switch (sortInput.SortType)
      {
        case ProductionTerminalSortType.Id:
          return query.OrderBy(r => r.Id, sortInput.SortOrder);
        case ProductionTerminalSortType.ProductionLineName:
          return query.OrderBy(r => r.ProductionLineName, sortInput.SortOrder);
        case ProductionTerminalSortType.IsActive:
          return query.OrderBy(r => r.IsActive, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException(nameof(sortInput.SortOrder), sortInput.SortType, null);
      }
    }
    #endregion
    #region Activate Production Terminal
    public ProductionTerminal ActivateProductionTerminal(int id, byte[] rowVersion)
    {
      return EditProductionTerminal(
              id: id,
              rowVersion: rowVersion,
              isActive: true);
    }
    #endregion
    #region Deactivate Production Terminal
    public ProductionTerminal DeactivateProductionTerminal(int id, byte[] rowVersion)
    {
      return EditProductionTerminal(
              id: id,
              rowVersion: rowVersion,
              isActive: false);
    }
    #endregion
    #region Change Production Terminal WorkStation
    public ProductionTerminal ChangeProductionTerminalWorkStation(
      int id,
      int productionLineId)
    {
      var productionTerminal = GetProductionTerminal(id: id);
      return EditProductionTerminal(
                    productionLineId: productionLineId,
                    productionTerminal: productionTerminal,
                    rowVersion: productionTerminal.RowVersion);
    }
    #endregion
    #region SetTerminalEmployee
    public TerminalProductionOperatorSettingResult SetTerminalEmployee(
      int productionOrderId,
      int productionTermialId,
      string employeeCode)
    {
      var result = new TerminalProductionOperatorSettingResult();
      #region GetEmployeeId
      var employees = App.Internals.UserManagement.GetEmployees(
          e => e,
          code: employeeCode);
      var employee = employees.FirstOrDefault();
      if (employee == null)
      {
        result.Result = TerminalProductionOperatorSettingResultType.InvalidOperatorId;
        result.EmployeeCode = employeeCode;
        return result;
      }
      #endregion
      int? productionOperatorMachineEmployeeId = null;
      List<int> logoutEmployeeIds = new List<int>();
      // لیست پرسنلی که برای این دستور کار ثبت شده را می گیریم 
      var productionOperatorMachineEmployees = repository.GetQuery<ProductionOperatorMachineEmployee>()
      .Where(pome => pome.ProductionOperator.ProductionOrderId == productionOrderId)
      .ToList();
      // لیست پرسنلی که برای این ترمینال و این دستور کار ثبت شده را می گیریم 
      var tpomes = productionOperatorMachineEmployees.Where(pome => pome.ProductionTerminalId == productionTermialId).ToList();
      //اگر لیست خالی بود یعنی برای آن ترمینال در این دستور کار قبلا هیچ عمیلاتی در نظر گرفته نشده است 
      if (tpomes.Count() == 0)
      {
        result.Result = TerminalProductionOperatorSettingResultType.OperationNotRegistered;
        return result;
      }
      // اگر پرسنل وارد شده در لیست موجود بود باید ویرایش شود و خروج بخورد و در لیست ورود و خروج نیز زمان خروج بخورد
      var currentProductionOperatorMachineEmployees = tpomes.Where(i => i.EmployeeId == employee.Id);
      if (currentProductionOperatorMachineEmployees.Count() > 0)
      {
        // خروج از عملیات در دستور کار 
        foreach (var cpome in currentProductionOperatorMachineEmployees)
        {
          EditProductionOperatorMachineEmployee(
                    productionOperatorMachineEmployee: cpome,
                    rowVersion: cpome.RowVersion,
                    employeeId: new TValue<int?>());
          productionOperatorMachineEmployeeId = cpome.Id;
        }
        result.Result = TerminalProductionOperatorSettingResultType.Logout;
        //اضافه کردن این پرسنل برای ثبت ساعت خروج 
        logoutEmployeeIds.Add(employee.Id);
      }
      else
      {
        // در غیر این صورت 
        // اگر بیشتر از یک پرسنل در لیست باشد باید پیغام مناسب صادر گردد 
        if (tpomes.Count() > 1)
        {
          result.Result = TerminalProductionOperatorSettingResultType.MultipleSetting;
          result.EmployeeCode = employee.Code;
          return result;
        }
        else if (tpomes.Count() == 1)
        {
          result.Result = TerminalProductionOperatorSettingResultType.Login;
          //اگر فقط یک پرسنل در لیست باشد باید پرسنل آن ترمینال ویرایش شود
          var cpome = tpomes.FirstOrDefault();
          // اضافه کردن پرسنل قبلی برای ثبت ساعت خروج                        
          if (cpome.EmployeeId != null)
            logoutEmployeeIds.Add(cpome.EmployeeId.Value);
          //حذف پرسنل از عملیات های دیگر در این دستور کار 
          var oldpomes = productionOperatorMachineEmployees.Where(i => i.EmployeeId == employee.Id);
          if (oldpomes.Count() > 0)
          {
            foreach (var oldpome in oldpomes)
            {
              //حذف پرسنل از عملیات قدیمی 
              EditProductionOperatorMachineEmployee(
                  productionOperatorMachineEmployee: oldpome,
                  rowVersion: oldpome.RowVersion,
                  employeeId: new TValue<int?>());
            }
          }
          //ویرایش پرسنل برای عملیات جدید
          EditProductionOperatorMachineEmployee(
              productionOperatorMachineEmployee: cpome,
              rowVersion: cpome.RowVersion,
              employeeId: employee.Id);
          productionOperatorMachineEmployeeId = cpome.Id;
        }
      }
      //گرفتن اطلاعات مورد نیاز برای درج رکورد ورود و خروجی 
      var pomeResult = repository.GetQuery<ProductionOperatorMachineEmployee>().
      Select(i => new
      {
        Id = i.Id,
        OperationCode = i.ProductionOperator.Operation.Code,
        EmployeeCode = i.Employee.Code,
        EmployeeId = i.EmployeeId,
        i.ProductionOperator.OperationId,
        i.ProductionOperator.DefaultTime,
        StuffId = i.ProductionOperator.ProductionOrder.WorkPlanStep.WorkPlan.BillOfMaterialStuffId,
        i.ProductionOperator.ProductionOrder.WorkPlanStep.ProductionLineId
      })
      .FirstOrDefault(i => i.Id == productionOperatorMachineEmployeeId);
      // ثبت ساعت خروج برای پرسنل هایی که عملیات آنها حذف شده است یا خروج خورده اند                 
      var productionLineEmployeeIntervals = repository.GetQuery<ProductionLineEmployeeInterval>()
      .Where(i => logoutEmployeeIds.Contains(i.EmployeeId) && i.ExitDateTime == null)
      .ToList();
      foreach (var plei in productionLineEmployeeIntervals)
      {
        EditExitDateTime(productionLineEmployeeInterval: plei,
                  rowVersion: plei.RowVersion);
      }
      //ثبت ساعت ورود برای این پرسنل به این عملیات
      if (result.Result == TerminalProductionOperatorSettingResultType.Login)
      {
        SaveExitAndEntranceProcess(
                  productionLineEmployeeIntervals: null,
                  productionLineId: pomeResult.ProductionLineId,
                  employeeId: pomeResult.EmployeeId.Value,
                  operationTime: new OperationList[] {
                        new OperationList() {
                            OperationId = pomeResult.OperationId,
                            Time = pomeResult.DefaultTime }
                },
                  stuffId: pomeResult.StuffId,
                  rfid: null);
      }
      result.OperationCode = pomeResult.OperationCode;
      result.EmployeeCode = employee.Code;
      return result;
    }
    #endregion
    #region SetTerminalOperation
    public TerminalProductionOperatorSettingResult SetTerminalOperation(
      int productionOrderId,
      int productionTermialId,
      string operationCode)
    {
      var result = new TerminalProductionOperatorSettingResult();
      #region GetOperation
      var operations = App.Internals.Planning.GetOperations(
          e => e,
          code: operationCode);
      var operation = operations.FirstOrDefault();
      if (operation == null)
      {
        result.Result = TerminalProductionOperatorSettingResultType.InvalidOperationCode;
        result.OperationCode = operationCode;
        return result;
      }
      #endregion
      var productionOperator = repository.GetQuery<ProductionOperator>()
    .Where(po => po.ProductionOrderId == productionOrderId && po.OperationId == operation.Id)
    .FirstOrDefault();
      if (productionOperator == null)
      {
        result.Result = TerminalProductionOperatorSettingResultType.NoSuchOperation;
        return result;
      }
      #region GetAll productionOperator for this productionTerminal and productionOrder
      var productionOperatorMachineEmployees = repository.GetQuery<ProductionOperatorMachineEmployee>()
    .Where(pome => pome.ProductionTerminalId == productionTermialId && pome.ProductionOperator.ProductionOrderId == productionOrderId)
    .ToList();
      #endregion
      if (productionOperatorMachineEmployees.Count() > 1)
      {
        result.Result = TerminalProductionOperatorSettingResultType.MultipleSetting;
        result.OperationCode = operation.Code;
        return result;
      }
      int? productionOperatorMachineEmployeeId = null;
      if (productionOperatorMachineEmployees.Count() == 1)
      {
        var productionOperatorMachineEmployee = productionOperatorMachineEmployees.FirstOrDefault();
        EditProductionOperatorMachineEmployee(
                  productionOperatorMachineEmployee: productionOperatorMachineEmployee,
                  productionOperatorId: productionOperator.Id,
                  rowVersion: productionOperatorMachineEmployee.RowVersion,
                  description: "Set With Barcode");
        productionOperatorMachineEmployeeId = productionOperatorMachineEmployee.Id;
      }
      else
      {
        var productionOperatorMachineEmployee = AddProductionOperatorMachineEmployee(
                  productionOperatorId: productionOperator.Id,
                  employeeId: null,
                  machineId: null,
                  productionTerminalId: productionTermialId,
                  description: "Set With Barcode");
        productionOperatorMachineEmployeeId = productionOperatorMachineEmployee.Id;
      }
      var productionOperatorMachineEmployeeResult = repository.GetQuery<ProductionOperatorMachineEmployee>().
            Select(i => new { Id = i.Id, OperationCode = i.ProductionOperator.Operation.Code, EmployeeCode = i.Employee.Code })
            .FirstOrDefault(i => i.Id == productionOperatorMachineEmployeeId);
      result.Result = TerminalProductionOperatorSettingResultType.OperationRegistered;
      result.OperationCode = productionOperatorMachineEmployeeResult.OperationCode;
      result.EmployeeCode = productionOperatorMachineEmployeeResult.EmployeeCode;
      return result;
    }
    #endregion
    #region GetProductionTermianlEmployee
    public string GetProductionTerminalEmployee(
      int productionTerminalId)
    {
      var ProductionTerminal = GetProductionTerminal(id: productionTerminalId);
      var Employee = ProductionTerminal.Employee;
      if (Employee == null)
      {
        return "";
      }
      return Employee.Code;
    }
    #endregion
    #region Connect
    public ConnectProductionTerminalResult ConnectProductionTerminal(int? publicKey = null)
    {
      #region Get User
      var user = App.Internals.UserManagement
    .GetUsers(userName: Models.StaticData.StaticVariables.TerminalUserName)
    .FirstOrDefault();
      #endregion
      if (user == null)
        throw new TerminalUserNotDefinedException();
      #region Create and set LoginResult
      var timeout = App.Providers.Storage.TokenTimeout;
      var expiresIn = DateTime.Now.AddMinutes(timeout);
      var refreshToken = Extentions.GenrateRefreshToken();
      var stateKey = Guid.NewGuid();
      string securityStamp = String.Empty;
      var token = JwtManager.GenerateToken(user, stateKey, expiresIn, ref securityStamp);
      var loginResult = new LoginResult()
      {
        UserId = user.Id,
        UserName = user.UserName,
        UserFirstName = user.Employee?.FirstName ?? "",
        UserEmployeeCode = user.Employee?.Code ?? "",
        UserLastName = user.Employee?.LastName ?? "",
        Image = user.Employee?.Image ?? null,
        UserEmployeeId = user.Employee?.Id ?? null,
        DepartmentId = user.Employee?.DepartmentId,
        Token = token,
        RefreshToken = refreshToken,
        ExpiresIn = expiresIn
      };
      App.Internals.UserManagement.AddUserToken(
                   userId: user.Id,
                   token: token,
                   refreshToken: refreshToken,
                   expiresIn: expiresIn);
      App.Providers.Session.Set(SessionKey.UserCredentials.ToString().KeyPrefix(stateKey.ToString()), loginResult, expiresIn.ComputeTimeSpan());
      App.Providers.Session.Set(SessionKey.SecurityStamp.ToString().KeyPrefix(user.Id.ToString()), securityStamp);
      #endregion
      var result = new ConnectProductionTerminalResult
      {
        Token = token,
        RefreshToken = refreshToken,
        ExpiresIn = expiresIn
      };
      return result;
    }
    #endregion
    //#region Connect
    //public InitialProductionTerminalResult InitialProductionTerminal(int publicKey)
    //{
    //    
    //        #region Get User
    //        var user = App.Internals.UserManagement.GetUser(id: Models.StaticData.StaticVariables.TerminalUserId)
    //            
    //;
    //        #endregion
    //        #region Create and set LoginResult 
    //        var loginResult = new LoginResult()
    //        {
    //            UserId = user.Id,
    //            UserName = user.UserName,
    //            UserFirstName = user.Employee?.FirstName ?? "",
    //            UserEmployeeCode = user.Employee?.Code ?? "",
    //            UserLastName = user.Employee?.LastName ?? "",
    //            Image = user.Employee?.Image ?? null,
    //            UserEmployeeId = user.Employee?.Id ?? null,
    //            DepartmentId = user.Employee?.DepartmentId
    //        };
    //        App.Providers.Session.Set(SessionKey.UserCredentials.ToString(), loginResult);
    //        #endregion
    //        return new ConnectProductionTerminalResult();
    //    });
    //}
    //#endregion
  }
}