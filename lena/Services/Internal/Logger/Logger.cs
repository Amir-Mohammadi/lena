using lena.Services.Core;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.ApplicationBase.Logger;
using lena.Services.CryptoMessaging.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Logger
{
  public partial class Logger
  {


    public LoggerConnection Connect()
    {

      //var handshake = new Handshake();
      //handshake.ComputeSharedKey(publickey);
      //var secret = handshake.SharedKeyBase64;
      var owner = App.Providers.Session.StateKey;
      var guid = App.Providers.Logger.New(secret: "", owner: owner);
      return new LoggerConnection() { Guid = guid, PublicKey = "" };
    }

    public void Disconnect(string id)
    {

      var owner = App.Providers.Session.StateKey;
      App.Providers.Logger.Remove(id: id, owner: owner);
    }
  }
}
