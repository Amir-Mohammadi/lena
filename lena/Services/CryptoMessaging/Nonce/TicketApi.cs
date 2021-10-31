using System;

namespace lena.Services.CryptoMessaging.Nonce
{
  public class TicketApi
  {
    public const string EmptyOwnerKey = "PUBLIC";
    private readonly NonceGenerator _nonceGenerator;
    private readonly ITicketApi _backendApi;

    public TicketApi(ITicketApi api)
    {
      _nonceGenerator = new NonceGenerator();
      _backendApi = api;
    }

    public string Next(string owner = EmptyOwnerKey)
    {
      var nonce = new Ticket
      {
        Owner = owner,
        Value = _nonceGenerator.Next()
      };
      _backendApi.Register(nonce);
      return nonce.Value;
    }

    public bool IsValid(string nonce, string owner = EmptyOwnerKey)
    {
      var non = new Ticket
      {
        Owner = owner,
        Value = nonce
      };
      return _backendApi.VerifyThenClean(non);
    }
  }
}