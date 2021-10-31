using lena.Domains.Enums;
namespace lena.Models.Common
{
  public class Key<T>
  {
    public Key(T id)
    {
      this.Id = id;
    }
    public T Id { get; set; }
  }
}