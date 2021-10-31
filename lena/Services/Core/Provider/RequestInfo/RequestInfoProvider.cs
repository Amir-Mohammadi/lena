using System.Collections.Generic;
using System.Diagnostics;

namespace lena.Services.Core.Provider.RequestInfo
{

  public abstract class RequestInfoProvider
  {
    public RequestInfoProvider()
    {
      Timer.Start();
    }
    public abstract string Url { get; }

    public abstract string Method { get; }

    public abstract object Parameters { get; }

    public abstract string ClientAddress { get; }

    public object Response { get; set; } = null;
    public bool IsFailed { get; set; } = false;

    public Stopwatch Timer = new Stopwatch();

  }
}
