using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Core.Exceptions;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Printers;
using lena.Models.WarehouseManagement.StuffSerial;
//using lena.Services.Common.Helpers;
//using lena.Services.PrinterManagment;
using Printer = lena.Domains.Printer;
using System.Threading.Tasks;
using lena.Domains.Enums;
using lena.Services.PrinterManagment;

namespace lena.Services.Internals.PrinterManagement
{
  public partial class PrinterManagement
  {
    public Printer GetPrinter(int id)
    {
      var record = repository.GetQuery<Printer>().FirstOrDefault(x => x.Id == id);
      if (record == null)
        throw new RecordNotFoundException(id, typeof(Printer));
      return record;
    }
    public Printer AddPrinter(AddPrinterInput input)
    {
      var printer = repository.Create<Printer>();
      printer.NameInSystem = input.NameInSystem;
      printer.NetworkAddress = input.NetworkAddress;
      printer.IsActive = input.IsActive;
      printer.IsColored = input.IsColored;
      printer.Location = input.Location;
      printer.Logo = input.Logo;
      printer.Manufacture = input.Manufacture;
      printer.Model = input.Model;
      printer.Description = input.Description;
      printer.PrinterType = input.PrinterType;
      printer.ModuleName = input.ModuleName;
      printer.SupportLan = input.SupportLan;
      printer.IsSupportTemplatePrint = input.IsSupportTemplatePrint;
      printer.CreatorUserId = App.Providers.Security.CurrentLoginData.UserId;
      printer.CreationTime = DateTime.Now.ToUniversalTime();
      printer.Setting = input.Setting;
      repository.Add(printer);
      return printer;
    }
    public Printer EditPrinter(EditPrinterInput input)
    {
      var printer = GetPrinter(id: input.Id);
      printer.NameInSystem = input.NameInSystem;
      printer.NetworkAddress = input.NetworkAddress;
      printer.IsActive = input.IsActive;
      printer.IsColored = input.IsColored;
      printer.Location = input.Location;
      printer.Logo = input.Logo;
      printer.SupportLan = input.SupportLan;
      printer.Manufacture = input.Manufacture;
      printer.Model = input.Model;
      printer.Description = input.Description;
      printer.PrinterType = input.PrinterType;
      printer.IsSupportTemplatePrint = input.IsSupportTemplatePrint;
      printer.ModuleName = input.ModuleName;
      printer.CreatorUserId = App.Providers.Security.CurrentLoginData.UserId;
      printer.CreationTime = DateTime.Now.ToUniversalTime();
      printer.Setting = input.Setting;
      repository.Update<Printer>(printer, input.RowVersion);
      return printer;
    }
    public Printer DeletePrinter(int id)
    {
      var printer = GetPrinter(id: id);
      repository.Delete(printer);
      return printer;
    }
    public string GetPrinterDefaultJsonSetting(PrinterType printerType)
    {
      var setting = new PrinterSetting();
      //Todo add setting by printer type
      var json = JsonConvert.SerializeObject(setting);
      return json;
    }
    public IQueryable<Printer> GetPrinters()
    {
      return repository.GetQuery<Printer>();
    }
    public IQueryable<PrinterResult> GetPrinterResults()
    {
      var query = (from printer in repository.GetQuery<Printer>()
                   let user = printer.CreatorUser
                   select new PrinterResult()
                   {
                     Id = printer.Id,
                     Name = printer.Manufacture + "-" + printer.Model + "-" + printer.PrinterType,
                     NameInSystem = printer.NameInSystem,
                     NetworkAddress = printer.NetworkAddress,
                     IsActive = printer.IsActive,
                     IsColored = printer.IsColored,
                     Location = printer.Location,
                     Logo = printer.Logo,
                     Manufacture = printer.Manufacture,
                     Model = printer.Model,
                     PrinterType = printer.PrinterType,
                     CreationTime = printer.CreationTime,
                     CreatorUserId = printer.CreatorUserId,
                     CreatorUserName = user.Employee.FirstName + " " + user.Employee.LastName,
                     SupportLan = printer.SupportLan,
                     ModuleName = printer.ModuleName,
                     Description = printer.Description,
                     IsSupportTemplatePrint = printer.IsSupportTemplatePrint,
                     Setting = printer.Setting,
                     RowVersion = printer.RowVersion
                   });
      return query;
    }
    public IQueryable<PrinterResult> SearchPrinterResultQuery(
        IQueryable<PrinterResult> query,
        AdvanceSearchItem[] advanceSearchItems,
        string searchText)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from printer in query
                where printer.Id.ToString().Contains(searchText) ||
                      printer.Location.Contains(searchText) ||
                      printer.Name.Contains(searchText) ||
                      printer.NameInSystem.Contains(searchText) ||
                      printer.NetworkAddress.ToString().Contains(searchText) ||
                      printer.Manufacture.Contains(searchText) ||
                      printer.Model.Contains(searchText) ||
                      printer.Description.Contains(searchText) ||
                      printer.CreatorUserName.Contains(searchText)
                select printer;
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    public void PrintBarcodes(
        IQueryable<StuffSerial> stuffSerials,
        int printerId,
        SerialPrintType printType,
        bool printFooterText,
        bool printVersion = false,
        string billOfMaterialVersion = null)
    {
      int columnCount = 1;
      switch (printType)
      {
        case SerialPrintType.OneColumn:
          columnCount = 1;
          break;
        case SerialPrintType.TwoColumn:
          columnCount = 2;
          break;
        case SerialPrintType.TreeColumn:
          columnCount = 3;
          break;
        default:
          columnCount = 3;
          break;
      }
      var printerEntity = App.Internals.PrinterManagment.GetPrinter(printerId);
      if (string.IsNullOrEmpty(printerEntity.ModuleName))
        throw new System.Exception(
                  $"ModuleName is not defined for printer '{printerEntity.NameInSystem}' with id {printerId}.");
      var printer = PrinterFactory.CreateBarcodePrinterModule(printerEntity.ModuleName, printerEntity.NameInSystem);
      //var printer = PrinterFactory.CreateBarcodePrinterModule(printerEntity.ModuleName, string.IsNullOrEmpty(printerEntity.NetworkAddress) ? printerEntity.NameInSystem : printerEntity.NetworkAddress);
      printer.Initialize();
      //Todo Put barcode printer setting in db and user able change setting from ui.
      if (printerEntity.Setting != null)
      {
        printer.Setting = (PrinterSetting)JsonConvert.DeserializeObject(printerEntity.Setting, typeof(PrinterSetting));
      }
      else
      {
        //TSC Setting
        printer.Setting.PaperWidth = 97;
        printer.Setting.PaperHeight = 30;
        printer.Setting.LabelWidth = 30;
        printer.Setting.LabelHeight = 10;
        printer.Setting.VerticalGap = 2;
        printer.Setting.HorizontalGap = 2;
        printer.Setting.PaperMargin = 2;
        printer.Setting.ReferencePoint = new System.Drawing.PointF(1, 0.5F);
      }
      var barcodeFooterText = App.Internals.ApplicationSetting.GetBarcodeFooterText();
      if (printVersion && billOfMaterialVersion != null)
      {
        printFooterText = true;
        barcodeFooterText = billOfMaterialVersion;
      }
      var barcodes = new List<BarcodeElement>();
      foreach (var item in stuffSerials)
      {
        var barcode = new BarcodeElement()
        {
          Barcode = item.Serial,
          AppendCrcToBarcode = true,
          BarcodeType = "128",
          AutoSetLocation = true,
          BarcodeHeight = 4,
          PrintFooterText = printFooterText
        };
        barcode.Info.Location = new System.Drawing.PointF(0, 0);
        barcode.Info.Text = barcodeFooterText; //$"Parlar.ir {DateTimeHelper.GetPersianDateTimeYear(DateTime.Now).ToString("D4").Substring(2, 2)}";
        barcode.Series.Location = new System.Drawing.PointF(0, 0);
        barcode.Series.Text = "-" + item.SerialProfileCode.ToString();
        barcodes.Add(barcode);
      }
      //#if !DEBUG
      printer.Print(barcodes.ToArray(), columnCount);
      //#endif
    }
    public void PrintBarcodesWithResult(
        IQueryable<StuffSerialResult> stuffSerials,
        int printerId,
        SerialPrintType printType,
        bool printFooterText,
        bool printVersion,
        string billOfMaterialVersion)
    {
      int columnCount = 1;
      switch (printType)
      {
        case SerialPrintType.OneColumn:
          columnCount = 1;
          break;
        case SerialPrintType.TwoColumn:
          columnCount = 2;
          break;
        case SerialPrintType.TreeColumn:
          columnCount = 3;
          break;
        default:
          columnCount = 3;
          break;
      }
      var printerEntity = App.Internals.PrinterManagment.GetPrinter(printerId);
      if (string.IsNullOrEmpty(printerEntity.ModuleName))
        throw new System.Exception(
                  $"ModuleName is not defined for printer '{printerEntity.NameInSystem}' with id {printerId}.");
      var printer = PrinterFactory.CreateBarcodePrinterModule(printerEntity.ModuleName, printerEntity.NameInSystem);
      if (!string.IsNullOrEmpty(printerEntity.Setting))
        printer.Setting = (PrinterSetting)JsonConvert.DeserializeObject(printerEntity.Setting, typeof(PrinterSetting));
      else
      {
        //TSC Setting
        printer.Setting.PaperWidth = 97;
        printer.Setting.LabelWidth = 30;
        printer.Setting.LabelHeight = 10;
        printer.Setting.VerticalGap = 2;
        printer.Setting.HorizontalGap = 2;
        printer.Setting.PaperMargin = 2;
        printer.Setting.ShiftDistance = 10;
        printer.Setting.ReferencePoint = new System.Drawing.PointF(1, 0.5F);
      }
      //printer.Initialize();
      var barcodeFooterText = App.Internals.ApplicationSetting.GetBarcodeFooterText();
      if (printVersion)
      {
        printFooterText = true;
        barcodeFooterText = billOfMaterialVersion;
      }
      var barcodes = new List<BarcodeElement>();
      foreach (var item in stuffSerials)
      {
        var barcode = new BarcodeElement()
        {
          Barcode = item.Serial,
          AppendCrcToBarcode = true,
          BarcodeType = "128",
          AutoSetLocation = true,
          BarcodeHeight = 4,
          PrintFooterText = printFooterText
        };
        barcode.Info.Location = new System.Drawing.PointF(0, 0);
        barcode.Info.Text = barcodeFooterText; //$"Parlar.ir {DateTimeHelper.GetPersianDateTimeYear(DateTime.Now).ToString("D4").Substring(2, 2)}";
        barcode.Series.Location = new System.Drawing.PointF(0, 0);
        barcode.Series.Text = "-" + item.SerialProfileCode.ToString();
        barcodes.Add(barcode);
      }
      //#if !DEBUG
      printer.Print(barcodes.ToArray(), columnCount);
      //#endif
    }
    #region Sort
    public IOrderedQueryable<PrinterResult> SortPrinterResult(
        IQueryable<PrinterResult> query,
        SortInput<PrinterSortTypes> sortInput)
    {
      switch (sortInput.SortType)
      {
        case PrinterSortTypes.Id:
          return query.OrderBy(r => r.Id, sortInput.SortOrder);
        case PrinterSortTypes.NameInSystem:
          return query.OrderBy(r => r.NameInSystem, sortInput.SortOrder);
        case PrinterSortTypes.Name:
          return query.OrderBy(r => r.Name, sortInput.SortOrder);
        case PrinterSortTypes.ModuleName:
          return query.OrderBy(r => r.ModuleName, sortInput.SortOrder);
        case PrinterSortTypes.NetworkAddress:
          return query.OrderBy(r => r.NetworkAddress, sortInput.SortOrder);
        case PrinterSortTypes.Manufacture:
          return query.OrderBy(r => r.Manufacture, sortInput.SortOrder);
        case PrinterSortTypes.Model:
          return query.OrderBy(r => r.Model, sortInput.SortOrder);
        case PrinterSortTypes.Logo:
          return query.OrderBy(r => r.Logo, sortInput.SortOrder);
        case PrinterSortTypes.Location:
          return query.OrderBy(r => r.Location, sortInput.SortOrder);
        case PrinterSortTypes.PrinterType:
          return query.OrderBy(r => r.PrinterType, sortInput.SortOrder);
        case PrinterSortTypes.IsColored:
          return query.OrderBy(r => r.IsColored, sortInput.SortOrder);
        case PrinterSortTypes.IsActive:
          return query.OrderBy(r => r.IsActive, sortInput.SortOrder);
        case PrinterSortTypes.IsSupportTemplatePrint:
          return query.OrderBy(r => r.IsSupportTemplatePrint, sortInput.SortOrder);
        case PrinterSortTypes.SupportLan:
          return query.OrderBy(r => r.SupportLan, sortInput.SortOrder);
        case PrinterSortTypes.CreationTime:
          return query.OrderBy(r => r.CreationTime, sortInput.SortOrder);
        case PrinterSortTypes.CreatorUserName:
          return query.OrderBy(r => r.CreatorUserName, sortInput.SortOrder);
        default:
          throw new ArgumentOutOfRangeException(nameof(sortInput.SortOrder), sortInput.SortType, null);
      }
    }
    #endregion
  }
}