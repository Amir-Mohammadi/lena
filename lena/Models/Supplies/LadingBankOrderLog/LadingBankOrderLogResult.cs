using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Ladings
{
  public class LadingBankOrderLogResult
  {
    public int Id { get; set; }

    public int LadingBankOrderStatusId { get; set; }

    public string LadingBankOrderStatusCode { get; set; }

    public string LadingBankOrderStatusName { get; set; }

    public DateTime DateTime { get; set; }

    public int UserId { get; set; }

    public string UserFullName { get; set; }

    public string Description { get; set; }

    public byte[] RowVersion { get; set; }
  }
}