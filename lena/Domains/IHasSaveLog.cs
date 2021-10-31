using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public interface IHasSaveLog
  {
    DateTime DateTime { get; set; }
    int UserId { get; set; }
    User User { get; set; }
  }
}