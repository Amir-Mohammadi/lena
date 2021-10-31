using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class SerialBuffer : IEntity
  {
    protected internal SerialBuffer()
    {
    }
    public int Id { get; set; }
    public double RemainingAmount { get; set; }
    public int ProductionTerminalId { get; set; }
    public SerialBufferType SerialBufferType { get; set; }
    public int BaseTransactionId { get; set; }
    public byte[] RowVersion { get; set; }
    public double DamagedAmount { get; set; }
    public double ShortageAmount { get; set; }
    public virtual BaseTransaction BaseTransaction { get; set; }
    public virtual ProductionTerminal ProductionTerminal { get; set; }
  }
}