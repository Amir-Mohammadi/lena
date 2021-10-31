using System;
using System.Linq;
using lena.Services.Core;
using lena.Services.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models;
using lena.Models.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance
{
  public partial class QualityAssurance
  {
    #region Get IndicatorCompletingQualityControlDocumentsPercentage  
    // محاسبه درصد مستندات آپلود شده کنترل کیفی
    public IndicatorCompletingQualityControlDocumentsPercentageResult GetIndicatorCompletingQualityControlDocumentsPercentage(
    TValue<DateTime> fromDate = null,
    TValue<DateTime> toDate = null)
    {


      var qualityControls = App.Internals.QualityControl.GetQualityControls(
                e => e,
                qualityControlType: QualityControlType.ReceiptQualityControl,
                isDelete: false)


            .Where(i => i.DateTime >= fromDate && i.DateTime <= toDate);

      var total = qualityControls.Count();

      var needToQualityControlDocumentUpload = qualityControls.Where(i => i.Stuff.NeedToQualityControlDocumentUpload);

      var needToQualityControlDocumentUploadCount = (double)needToQualityControlDocumentUpload.Count();

      var uploadedDocument = (double)needToQualityControlDocumentUpload.Where(i => i.DocumentId != null).Count();
      var completingQualityControlDocumentsPercentage = 0.0;
      if (needToQualityControlDocumentUploadCount != 0)
      {
        completingQualityControlDocumentsPercentage = (uploadedDocument / needToQualityControlDocumentUploadCount) * 100;
      }

      var indicatorRejectedPurchaseResult = new IndicatorCompletingQualityControlDocumentsPercentageResult
      {
        Amount = completingQualityControlDocumentsPercentage
      };
      return indicatorRejectedPurchaseResult;
    }
    #endregion


  }

}
