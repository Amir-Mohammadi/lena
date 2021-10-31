using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.Ladings
{
  public class LadingCustomhouseLogResult
  {
    public int Id { get; set; }

    public int LadingCustomhouseStatusId { get; set; }

    public string LadingCustomhouseStatusCode { get; set; }

    public string LadingCustomhouseStatusName { get; set; }

    public DateTime DateTime { get; set; }

    public int UserId { get; set; }

    public string UserFullName { get; set; }

    public string Description { get; set; }

    public byte[] RowVersion { get; set; }
  }
}