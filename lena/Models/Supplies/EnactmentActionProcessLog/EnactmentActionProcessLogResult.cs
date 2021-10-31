using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.EnactmentActionProcessLog
{
  public class EnactmentActionProcessLogResult
  {
    public int Id { get; set; }

    public int EnactmentActionProcessId { get; set; }

    public string EnactmentActionProcessCode { get; set; }

    public string EnactmentActionProcessName { get; set; }

    public DateTime DateTime { get; set; }

    public int UserId { get; set; }

    public string UserFullName { get; set; }

    public string Description { get; set; }

    public byte[] RowVersion { get; set; }
  }
}