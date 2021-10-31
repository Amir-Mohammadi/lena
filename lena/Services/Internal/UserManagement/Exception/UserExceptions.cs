using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement.Exception
{
  #region User Exceptions

  public abstract class UserModuleException : InternalServiceException
  {
    protected UserModuleException(int userId, string userName)
    {
      UserId = userId;
      UserName = userName;
    }

    protected UserModuleException(int userId)
    {
      UserId = userId;
    }

    protected UserModuleException(string userName)
    {
      UserName = userName;
    }

    public int UserId { get; }

    public string UserName { get; }
  }



  public class UserPasswordIsNotValidException : InternalServiceException
  {
  }

  public class UserLockedException : InternalServiceException
  {
  }
  public class UserNotFoundException : UserModuleException
  {
    public int[] Test { get; set; } = new int[] { 1, 5, 3, 6 };
    public UserNotFoundException(int userId) : base(userId, "")
    {

    }

    public UserNotFoundException(string userName) : base(userName)
    {
    }
  }

  public class PasswordNotExpiredException : InternalServiceException
  {

  }
  public class UserNameNotValidException : UserModuleException
  {
    public UserNameNotValidException(string userName) : base(userName)
    {

    }
  }

  public class PasswordInvalidRangeException : InternalServiceException
  {
    public string Password { get; }
    public PasswordInvalidRangeException(string password)
    {
      this.Password = password;
    }
  }

  public class UserExistsException : UserModuleException
  {

    public UserExistsException(string userName) : base(-1, userName)
    {

    }
  }

  public class UserPasswordDoseNotMatchException : InternalServiceException
  {
    public string OldPassword { get; }

    public UserPasswordDoseNotMatchException(string oldPassword)
    {
      this.OldPassword = oldPassword;
    }
  }

  public class UserPasswordConfirmationDoseNotMatchException : InternalServiceException
  {
    public string Password { get; }
    public string PasswordConfirmation { get; }

    public UserPasswordConfirmationDoseNotMatchException(string password, string passwordConfirmation)
    {
      this.Password = password;
      this.PasswordConfirmation = passwordConfirmation;
    }

  }

  public class UserIsNotActivatedException : UserModuleException
  {

    public UserIsNotActivatedException(int userId, string userName) : base(userId, userName)
    {
    }
  }

  public class UserIsDeletedException : UserModuleException
  {
    public UserIsDeletedException(int userId, string userName) : base(userId, userName)
    {
    }

  }

  public class PasswordExpirationDateException : UserModuleException
  {
    public PasswordExpirationDateException(string userName) : base(userName)
    {
    }
  }
  #endregion
}
