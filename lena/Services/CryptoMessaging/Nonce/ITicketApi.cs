namespace lena.Services.CryptoMessaging.Nonce
{
  public interface ITicketApi
  {
    bool VerifyThenClean(Ticket ticket);
    void Register(Ticket ticket);
  }
}