using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Terminal.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Services.CryptoMessaging.Nonce;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
using core.Data;
namespace lena.Services.Internals.Terminal
{
  public class TicketManagement : core.Autofac.ISingletonDependency
  {
    private readonly IRepository repository;
    public TicketManagement(IRepository repository)
    {
      this.repository = repository;
    }
    private readonly TicketApi _api;
    public TicketManagement()
    {
      _api = new TicketApi(new InternalTicketApi(this));
    }
    private class InternalTicketApi : ITicketApi
    {
      private readonly TicketManagement _ticketManagement;
      public InternalTicketApi(TicketManagement management)
      {
        _ticketManagement = management;
      }
      public bool VerifyThenClean(Ticket ticket)
      {
        return _ticketManagement.Verify(sessionId: ticket.Owner, value: ticket.Value);
      }
      public void Register(Ticket ticket)
      {
        _ticketManagement.AddTerminalTicket(sessionId: ticket.Owner, value: ticket.Value);
      }
    }
    public string Next()
    {
      string owner = "PUBLIC";
      if (App.Providers.Session.Contains(SessionKey.TerminalSecretKey.ToString()))
      {
        owner = App.Providers.Session.StateKey;
      }
      return _api.Next(owner);
    }
    public bool IsValid(string ticket, string owner = "PUBLIC")
    {
      return _api.IsValid(ticket, owner);
    }
    private bool Verify(string sessionId, string value)
    {
      var ticket = GetTerminalTicketRegisteries(sessionId: sessionId, value: value)
                .FirstOrDefault();
      if (ticket == null)
      {
        return false;
      }
      var now = DateTime.Now.ToUniversalTime().Subtract(ticket.Date).Seconds;
      if (now > 60)
      {
        return false;
      }
      DeleteTerminalTicket(id: ticket.Id);
      return true;
    }
    private TerminalTicketRegistery AddTerminalTicket(
        string sessionId, string value)
    {
      var ticket = repository.Create<TerminalTicketRegistery>();
      ticket.Date = DateTime.Now.ToUniversalTime();
      ticket.SessionId = sessionId;
      ticket.Value = value;
      repository.Add(ticket);
      return ticket;
    }
    private IQueryable<TerminalTicketRegistery> GetTerminalTicketRegisteries(TValue<int> id = null, TValue<string> sessionId = null,
        TValue<string> value = null)
    {
      var query = repository.GetQuery<TerminalTicketRegistery>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (sessionId != null)
        query = query.Where(i => i.SessionId == sessionId);
      if (value != null)
        query = query.Where(i => i.Value == value);
      return query;
    }
    private TerminalTicketRegistery GetTerminalTicketRegistery(int id)
    {
      var ticket = GetTerminalTicketRegisteries(id: id).FirstOrDefault();
      if (ticket == null)
      {
        throw new TerminalTicketRegisteryNotFoundException();
      }
      return ticket;
    }
    private void DeleteTerminalTicket(int id)
    {
      var ticket = GetTerminalTicketRegistery(id: id);
      repository.Delete(ticket);
    }
  }
}