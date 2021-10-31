﻿using System;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderItemBlock
{
  public class FullOrderItemBlockResult
  {
    public int Id { get; set; }
    public string Description { get; set; }
    public string OrderItemCode { get; set; }
    public double OrderItemQty { get; set; }
    public string OrderItemUnit { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public string OrderTypeName { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string StuffNoun { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime DeliveryDate { get; set; }
    public DateTime SaleBlockDate { get; set; }
    public double Qty { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
