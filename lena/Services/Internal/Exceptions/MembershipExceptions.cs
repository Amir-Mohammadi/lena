
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  #region Membership Exceptions

  public abstract class MembershipException : InternalServiceException
  {
    protected MembershipException(int id)
    {
      Id = id;
    }


    public int Id { get; }
  }

  public class MembershipNotFoundException : MembershipException
  {
    public MembershipNotFoundException(int id) : base(id)
    {
    }

  }

  public class MemebershipHasRelationshipWithUsers : MembershipException
  {
    public MemebershipHasRelationshipWithUsers(int id) : base(id)
    {
    }


  }

  public class MemebershipHasRelationshipWithUserGroup : MembershipException
  {
    public MemebershipHasRelationshipWithUserGroup(int id) : base(id)
    {
    }


  }



  #endregion
}
