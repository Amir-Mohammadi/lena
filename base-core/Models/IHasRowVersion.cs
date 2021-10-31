namespace core.Models

{
  public interface IHasRowVersion
  {
    byte[] RowVersion { get; set; }
  }
}