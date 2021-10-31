using lena.Domains.Enums;
namespace lena.Models.Common
{
  public class DeleteRecordInput : DeleteRecordInput<int>
  {

  }

  public class DeleteRecordInput<T>
  {
    public T Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
