using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Core.Provider.Logger
{
  public interface ILogger
  {
    int Log(object payload);
  }
}
