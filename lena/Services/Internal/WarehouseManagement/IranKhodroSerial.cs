using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.StaticData;
using lena.Models;
using lena.Models.Common;
using lena.Models.WarehouseManagement.StuffSerial;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Get
    public IranKhodroSerial GetIranKhodroSerial(string iranKhodroSerial) => GetIranKhodroSerial(selector: e => e, iranKhodroSerial: iranKhodroSerial);
    public TResult GetIranKhodroSerial<TResult>(
        Expression<Func<IranKhodroSerial, TResult>> selector,
        string iranKhodroSerial)
    {

      var iranKhodroSerialResult = GetIranKhodroSerials(
                selector: selector,
                linkedSerial: iranKhodroSerial)


            .FirstOrDefault();
      //if (iranKhodroSerialResult == null)
      //    throw new IranKhodroSerialNotFoundException(iranKhodroSerial);
      return iranKhodroSerialResult;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetIranKhodroSerials<TResult>(
        Expression<Func<IranKhodroSerial, TResult>> selector,
        TValue<int> stuffId = null,
        TValue<string> linkedSerial = null,
        TValue<int> customerStuffId = null,
        TValue<int> productionYearId = null,
        TValue<int> customerStuffVersionId = null,
        TValue<string> manufacturerCode = null,
        TValue<string> technicalNumber = null,
        TValue<int> productionDay = null,
        TValue<DateTime> productionDateTime = null,
        TValue<string[]> serials = null,
        TValue<string> toSerial = null,
        TValue<string> fromSerial = null)
    {

      var query = repository.GetQuery<IranKhodroSerial>();

      if (stuffId != null)
        query = query.Where(i => i.CustomerStuff.StuffId == stuffId);
      if (customerStuffId != null)
        query = query.Where(i => i.CustomerStuffId == customerStuffId);
      if (productionYearId != null)
        query = query.Where(i => i.ProductionYearId == productionYearId);
      if (customerStuffVersionId != null)
        query = query.Where(i => i.CustomerStuffVersionId == customerStuffVersionId);
      if (manufacturerCode != null)
        query = query.Where(i => i.CustomerStuff.ManufacturerCode == manufacturerCode);
      if (technicalNumber != null)
        query = query.Where(i => i.CustomerStuff.TechnicalNumber == technicalNumber);
      if (productionDateTime != null)
        query = query.Where(i => i.ProductionDateTime == productionDateTime);
      if (productionDay != null)
        query = query.Where(i => i.ProductionDay == productionDay);
      if (linkedSerial != null)
        query = query.Where(i => i.LinkSerial.LinkedSerial == linkedSerial);


      if (!string.IsNullOrWhiteSpace(fromSerial))
      {
        query = query.Where(i => i.LinkSerial.LinkedSerial.CompareTo(fromSerial) >= 0);
      }

      if (!string.IsNullOrWhiteSpace(toSerial))
      {
        query = query.Where(i => i.LinkSerial.LinkedSerial.CompareTo(toSerial) <= 0);
      }

      if (serials != null)
        query = query.Where(i => serials.Value.Contains(i.LinkSerial.LinkedSerial));


      return query.Select(selector);
    }
    #endregion

    #region AddStuffSerials
    internal IQueryable<IranKhodroSerialResult> AddIranKhodroSerialsProcess(
        int productionYearId,
        int customerStuffId,
        int customerStuffVersionId,
        DateTime? productionDateTime,
        int qty)
    {

      var customerStuff = GetCustomerStuff(id: customerStuffId);

      PersianCalendar persianCalendar = new PersianCalendar();
      TimeSpan timeSpan = DateTime.UtcNow.TimeOfDay;
      DateTime productionDateTimeObj = DateTime.UtcNow;

      #region Check Permission
      if (productionDateTime != null)
      {
        var checkPermissionResult = App.Internals.UserManagement.CheckPermission(
                                                      actionName: StaticActionName.EditProductionDateTimeIranKhdoroSerial,
                                                      actionParameters: null);

        if (checkPermissionResult.AccessType == AccessType.Allowed)
        {
          productionDateTimeObj = (DateTime)productionDateTime;
        }
      }
      #endregion

      productionDateTimeObj = productionDateTimeObj.Add(timeSpan);
      var productionDay = persianCalendar.GetDayOfYear(productionDateTimeObj);

      if (productionDay > 366)
      {
        throw new ProductionDayInIranKhodroSerialsNotValidException(productionDay: productionDay);
      }

      #region GetMaxIranKhodroSerial
      int maxIranKhodroSerialProduction = 0;
      var result = GetIranKhodroSerials(
                   e => e,
                   customerStuffId: customerStuffId,
                   customerStuffVersionId: customerStuffVersionId,
                   productionYearId: productionYearId,
                   productionDay: productionDay,
                   manufacturerCode: customerStuff.ManufacturerCode);
      if (result.Any())
      {
        maxIranKhodroSerialProduction = result.Max(m => m.ProductionSerial);
      }

      var iranKhodroSerials = new List<IranKhodroSerialResult>();
      for (var i = 1; i <= qty; i++)
      {
        maxIranKhodroSerialProduction++;
        if (maxIranKhodroSerialProduction > 9999)
        {
          throw new ProductionSerialInIranKhodroSerialIsNotValidException(maxIranKhodroSerialProduction);
        }

        #region AddIranKhodroSerial
        var iranKhodroSerial = AddIranKhodroSerials(
                                customerStuffId: customerStuffId,
                                customerStuffVersionId: customerStuffVersionId,
                                productionYearId: productionYearId,
                                productionDay: productionDay,
                                manufacturerCode: customerStuff.ManufacturerCode.ToUpper(),
                                technicalNumber: customerStuff.TechnicalNumber.ToUpper(),
                                productionDateTime: productionDateTimeObj,
                                productionSerial: maxIranKhodroSerialProduction);
        #endregion

        #region GetCustomerStuff
        var customerStuffRes = GetCustomerStuff(
                            id: customerStuffId);
        if (customerStuffRes == null)
          throw new CustomerStuffNotFoundException(id: customerStuffId);
        #endregion

        #region GetCustomerStuffVersion
        var customerStuffVersion = GetCustomerStuffVersion(
                                    id: customerStuffVersionId);
        if (customerStuffVersion == null)
          throw new CustomerStuffNotFoundException(id: customerStuffVersionId);

        #endregion

        #region GetProductionYear
        var productionYear = GetProductionYear(
            id: productionYearId);
        if (productionYear == null)
          throw new ProductionYearNotFoundException(id: productionYearId);
        #endregion


        string patternFormatSerialProduction = "0000";
        string maxSerialProductionStr = maxIranKhodroSerialProduction.ToString(patternFormatSerialProduction);

        string patternFormatProductionDay = "000";
        string productionDayStr = productionDay.ToString(patternFormatProductionDay);

        string concatenatedSerial = String.Concat(
                                                  productionYear.Code.ToUpper(),
                                                  customerStuffRes.Code.ToUpper(),
                                                  customerStuffVersion.Code.ToUpper(),
                                                  customerStuff.ManufacturerCode.ToUpper(),
                                                  productionDayStr,
                                                  maxSerialProductionStr);

        if (concatenatedSerial.Count() < 14 || concatenatedSerial.Count() > 14)
          throw new IranKhodroSerialIsNotValidException(concatenatedSerial);

        #region CheckSerialExistOrNot
        var iranKhodroSerialRes = GetIranKhodroSerial(
                                        iranKhodroSerial: concatenatedSerial);

        if (iranKhodroSerialRes != null)
          throw new IranKhodroSerialHasExistException(iranKhodroSerialRes.LinkSerial.LinkedSerial);
        #endregion

        IranKhodroSerialResult iranKhodroSerialResult = new IranKhodroSerialResult();
        iranKhodroSerialResult.Serial = concatenatedSerial;

        #region AddLinkSerial
        var linkSerial = AddLinkSerial(
            customerId: customerStuffRes.CustomerId,
            type: LinkSerialType.IranKhodro,
            linkedSerial: concatenatedSerial);

        EditIranKhodroSerial(
                              iranKhodroSerial: iranKhodroSerial,
                              linkSerial: linkSerial);
        #endregion

        iranKhodroSerials.Add(iranKhodroSerialResult);
      }
      #endregion

      var serials = iranKhodroSerials.Select(i => i.Serial).ToArray();
      return GetIranKhodroSerials(
                    selector: ToIranKhodroSerialResult,
                    serials: serials);

    }
    #endregion


    #region Add
    public IranKhodroSerial AddIranKhodroSerials(
        int customerStuffId,
        int customerStuffVersionId,
        DateTime productionDateTime,
        int productionDay,
        int productionYearId,
        string technicalNumber,
        string manufacturerCode,
        int productionSerial)
    {

      var iranKhodroSerial = repository.Create<IranKhodroSerial>();
      iranKhodroSerial.CustomerStuffId = customerStuffId;
      iranKhodroSerial.CustomerStuffVersionId = customerStuffVersionId;
      iranKhodroSerial.UserId = App.Providers.Security.CurrentLoginData.UserId;
      iranKhodroSerial.ProductionDateTime = productionDateTime;
      iranKhodroSerial.ProductionDay = productionDay;
      iranKhodroSerial.ProductionYearId = productionYearId;
      iranKhodroSerial.ProductionSerial = productionSerial;
      repository.Add(iranKhodroSerial);
      return iranKhodroSerial;
    }
    #endregion

    #region Edit
    internal IranKhodroSerial EditIranKhodroSerial(
        IranKhodroSerial iranKhodroSerial,
        TValue<LinkSerial> linkSerial = null,
        TValue<int> productionYearId = null,
        TValue<int> customerStuffId = null,
        TValue<int> customerStuffVersionId = null,
        TValue<DateTime> productionDateTime = null,
        TValue<bool> print = null,
        TValue<int> printQty = null,
        TValue<int> productionSerial = null)
    {


      if (productionYearId != null)
        iranKhodroSerial.ProductionYearId = productionYearId;
      if (customerStuffId != null)
        iranKhodroSerial.CustomerStuffId = customerStuffId;
      if (customerStuffVersionId != null)
        iranKhodroSerial.CustomerStuffVersionId = customerStuffVersionId;
      if (productionDateTime != null)
        iranKhodroSerial.ProductionDateTime = productionDateTime;
      if (productionSerial != null)
        iranKhodroSerial.ProductionSerial = productionSerial;
      if (linkSerial != null)
        iranKhodroSerial.LinkSerial = linkSerial;
      if (print != null)
        iranKhodroSerial.Print = print;
      if (printQty != null)
        iranKhodroSerial.PrintQty = printQty;


      repository.Update(iranKhodroSerial, iranKhodroSerial.RowVersion);
      return iranKhodroSerial;
    }
    #endregion

    #region Remove
    public void RemoveIranKhodroSerial(string serial)
    {


      var iranKhodroSerial = GetIranKhodroSerial(
                iranKhodroSerial: serial);

      //if (iranKhodroSerial.LinkSerial.LinkedSerial != null)
      //    throw new IranKhodroSerialHasBeenLinkedException(iranKhodroSerial: iranKhodroSerial.LinkSerial.LinkedSerial);

      DeleteIranKhodroSerial(iranKhodroSerial.LinkSerial.LinkedSerial);
    }
    #endregion

    #region Delete
    public void DeleteIranKhodroSerial(string serial)
    {

      var iranKhodroSerial = GetIranKhodroSerial(iranKhodroSerial: serial);
      repository.Delete(iranKhodroSerial);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<IranKhodroSerialResult> SortIranKhodroSerialResult(
        IQueryable<IranKhodroSerialResult> query, SortInput<IranKhodroSerialSortType> type)
    {
      switch (type.SortType)
      {
        case IranKhodroSerialSortType.ManufacturerCode:
          return query.OrderBy(a => a.ManufacturerCode, type.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<IranKhodroSerialResult> SearchIranKhodroSerialResult(
        IQueryable<IranKhodroSerialResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Serial.ToString().Contains(searchText) ||
                    item.ManufacturerCode.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion


    #region ToResult
    public Expression<Func<IranKhodroSerial, IranKhodroSerialResult>> ToIranKhodroSerialResult =

         iranKhodroSerial => new IranKhodroSerialResult()
         {
           Serial = iranKhodroSerial.LinkSerial.LinkedSerial,
           StuffId = iranKhodroSerial.CustomerStuff.StuffId,
           StuffCode = iranKhodroSerial.CustomerStuff.Stuff.Code,
           StuffName = iranKhodroSerial.CustomerStuff.Stuff.Name,
           CustomerId = iranKhodroSerial.CustomerStuff.CustomerId,
           CustomerCode = iranKhodroSerial.CustomerStuff.Customer.Code,
           CustomerName = iranKhodroSerial.CustomerStuff.Customer.Name,
           LinkSerialType = iranKhodroSerial.LinkSerial.Type,
           UserId = iranKhodroSerial.UserId,
           EmployeeFullName = iranKhodroSerial.User.Employee.FirstName + " " + iranKhodroSerial.User.Employee.LastName,
           ProductionYearId = iranKhodroSerial.ProductionYearId,
           ProductionYearCode = iranKhodroSerial.ProductionYear.Code,
           ProductionYearYear = iranKhodroSerial.ProductionYear.Year,
           CustomerStuffId = iranKhodroSerial.CustomerStuffId,
           CustomerStuffCode = iranKhodroSerial.CustomerStuff.Code,
           CustomerStuffName = iranKhodroSerial.CustomerStuff.Name,
           CustomerStuffType = iranKhodroSerial.CustomerStuff.Type,
           CustomerStuffVersionId = iranKhodroSerial.CustomerStuffVersionId,
           CustomerStuffVersionCode = iranKhodroSerial.CustomerStuffVersion.Code,
           CustomerStuffVersionName = iranKhodroSerial.CustomerStuffVersion.Name,
           ManufacturerCode = iranKhodroSerial.CustomerStuff.ManufacturerCode,
           TechnicalNumber = iranKhodroSerial.CustomerStuff.TechnicalNumber,
           DateTime = iranKhodroSerial.LinkSerial.DateTime,
           ProductionSerial = iranKhodroSerial.ProductionSerial,
           ProductionDateTime = iranKhodroSerial.ProductionDateTime,
           Print = iranKhodroSerial.Print,
           PrintQty = iranKhodroSerial.PrintQty,
           RowVersion = iranKhodroSerial.RowVersion
         };
    #endregion


    #region PrintIranKhdoroSerials
    internal void PrintIranKhdoroSerials(
        StuffSerialResult[] stuffSerialsList,
        SerialPrintType printType,
        int printerId,
        string reportName = null,
        bool printFooterText = false,
        bool printVersion = false,
        string billOfMaterialVersion = null
        )
    {

      #region Determine Printed Serials
      var serials = stuffSerialsList.Select(m => m.Serial).ToArray();
      var iranKhodroSerials = GetIranKhodroSerials(e => e, serials: serials);
      foreach (var iranKhodroSerial in iranKhodroSerials)
      {
        EditIranKhodroSerial(
                  iranKhodroSerial: iranKhodroSerial,
                  print: true,
                  printQty: iranKhodroSerial.PrintQty + 1);

      }
      #endregion

      #region Print
      PrintStuffSerials(
          printType: printType,
          stuffSerialsList: stuffSerialsList.AsQueryable(),
          printerId: printerId,
          reportName: reportName,
          printFooterText: printFooterText,
          printVersion: printVersion,
          billOfMaterialVersion: billOfMaterialVersion);
      #endregion
    }
    #endregion

  }
}
