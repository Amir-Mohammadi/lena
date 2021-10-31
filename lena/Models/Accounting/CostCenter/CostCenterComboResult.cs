﻿using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.Accounting.CostCenter
{
  public class CostCenterComboResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public CostCenterStatus Status { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }


  }
}
