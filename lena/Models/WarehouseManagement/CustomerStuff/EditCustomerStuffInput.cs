﻿using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models
{
  public class EditCustomerStuffInput
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int StuffId { get; set; }
    public int CustomerId { get; set; }
    public CustomerStuffType Type { get; set; }
    public string ManufacturerCode { get; set; }
    public string TechnicalNumber { get; set; } // شماره فنی
    public byte[] RowVersion { get; set; }
  }
}