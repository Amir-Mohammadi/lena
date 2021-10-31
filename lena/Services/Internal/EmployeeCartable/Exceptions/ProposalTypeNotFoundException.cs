﻿using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class ProposalTypeNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public ProposalTypeNotFoundException(int id)
    {
      Id = id;
    }
  }
}
