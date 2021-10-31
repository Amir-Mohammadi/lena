using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using lena.Services.Common;
namespace lena.Services.Core.Foundation
{
  #region Base
  public abstract class InternalServiceException : Exception
  {
    private bool isInitialized = false;
    public int TrackerId { get; set; } = -1;
    protected InternalServiceException()
    {
      Init();
    }
    protected InternalServiceException(string message, Exception e) : base(message, e)
    {
      Init();
    }
    protected InternalServiceException(string message) : base(message)
    {
      Init();
    }
    public void Init()
    {
      if (isInitialized) return;
      isInitialized = true;
      App.Providers.PersistentLogger.Steps.Add(new Provider.Logger.PersistentLogStep(Provider.Logger.PersistentLogStepType.Error,
          new
          {
            Transactions = App.Providers.UncommitedTransactionAgent.UncommitedTransactions,
            Exception = this
          }
          , 6));
      TrackerId = App.Providers.PersistentLogger.Finish();
      App.Providers.Diagnostics.Diagnose();
    }
    public override string ToString()
    {
      var myType = GetType();
      var exType = typeof(Exception);
      var myProps = new List<PropertyInfo>(myType.GetProperties());
      var exPrpos = new List<PropertyInfo>(exType.GetProperties());
      var props = (from prop in myProps let match = exPrpos.Any(t1 => prop.Name.Contains(t1.Name)) where !match select prop).ToList();
      var output = (from prop in props let propValue = prop.GetValue(this, null) select $"'{prop.Name}' : '{propValue}'").ToList();
      return "[\n\t'" + GetType().ToUnderScores().ToUpper() + "' : { " + string.Join(" , ", output) + " } \n]";
    }
  }
  #endregion
}