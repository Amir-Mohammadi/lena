﻿using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.WarehouseIssue
{
  public class AcceptWarehouseIssueInput
  {

    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string Description { get; set; }
    public int FromWarehouseId { get; set; }
    public int? ToWarehouseId { get; set; }
  }
}
