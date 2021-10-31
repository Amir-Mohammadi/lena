﻿using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models
{
  public class CooperatorContactResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsMain { get; set; }
    public string ContactText { get; set; }
    public int ContactTypeId { get; set; }
    public string ContactTypeName { get; set; }
    public int? CooperatorId { get; set; }
    public CooperatorContactType CooperatorContactType { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
