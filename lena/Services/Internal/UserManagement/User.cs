using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Core.Common;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Core.TokenManager;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.UserManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.UserManagement.User;
using System.Configuration;
// using System.Activities.Debugger;
using lena.Models.UserManagement.Membership;
using System.Text;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement
{
  public partial class UserManagement
  {
    public IQueryable<User> GetUsers(
        TValue<int> id = null,
        TValue<int[]> ids = null,
        TValue<string> userName = null,
        TValue<bool> isActive = null,
        TValue<bool> isDeleted = null)
    {
      var users = repository.GetQuery<User>();
      if (id != null)
        users = users.Where(i => i.Id == id);
      if (ids != null && ids.Value.Length != 0)
        users = users.Where(i => ids.Value.Contains(i.Id));
      if (userName != null)
        users = users.Where(i => i.UserName == userName);
      if (isActive != null)
        users = users.Where(i => i.IsActive == isActive);
      if (isDeleted != null)
        users = users.Where(i => i.IsDelete == isDeleted);
      return users;
    }
    public User GetUser(int id)
    {
      var user = GetUsers(id: id).SingleOrDefault();
      if (user == null)
        throw new UserNotFoundException(id);
      return user;
    }
    public User AddUserProcess(
        string userName,
        string password,
        string passwordConfimation,
        int? employeeId,
        bool isActive,
        bool isDeleted
       )
    {
      int Days = App.Internals.ApplicationSetting.PasswordValidity();
      var passwordExpirationDate = DateTime.Now.AddDays(Days);
      var employee = employeeId == null ? null : GetEmployee(employeeId.Value);
      if (GetUsers(userName: userName).SingleOrDefault() != null)
        throw new UserExistsException(userName);
      if (password != passwordConfimation)
        throw new UserPasswordConfirmationDoseNotMatchException(password, passwordConfimation);
      if (password.Length < 5)
        throw new PasswordInvalidRangeException(password);
      var user = AddUser(
                                       userName: userName,
                                       password: password,
                                       employee: employee,
                                       isActive: isActive,
                                       isDeleted: isDeleted,
                                       passwordExpirationDate: passwordExpirationDate);
      var allMembersGroup = App.Internals.ApplicationSetting.GetAllMembersGroupValue();
      SaveMembershipInput[] allGroupMemberShip = { new SaveMembershipInput() { UserGroupId = allMembersGroup, IsMember = true, UserId = user.Id } };
      SaveMemberships(
               membershipInputs: allGroupMemberShip
               );
      return user;
    }
    public User AddUser(
        string userName,
        string password,
        Employee employee,
        bool isActive,
        bool isDeleted,
        DateTime passwordExpirationDate)
    {
      var user = repository.Create<User>();
      user.UserName = userName;
      user.Password = Crypto.Sha1(password);
      user.IsActive = isActive;
      user.IsDelete = isDeleted;
      user.IsLocked = false;
      user.LockOutDateTime = DateTime.Now.ToUniversalTime();
      user.LoginFailedCount = 0;
      user.PasswordExpirationDate = passwordExpirationDate;
      user.Employee = employee;
      repository.Add(user);
      return user;
    }
    public User EditUserProcess(
        byte[] rowVersion,
        int id,
        TValue<string> userName = null,
        TValue<string> password = null,
        TValue<int?> employeeId = null,
        TValue<bool> isActive = null,
        TValue<bool> isDeleted = null,
        TValue<bool> isLocked = null,
        TValue<DateTime> lockOutTime = null,
        TValue<int> loginFailedCount = null)
    {
      var user = GetUser(id: id);
      if (user == null)
        throw new UserNotFoundException(id);
      if (userName != null)
      {
        var existUser = GetUsers(userName: userName);
        if (existUser.Count() > 1 || (existUser.Count() == 1 && existUser.FirstOrDefault()?.Id != id))
          throw new UserExistsException(userName);
      }
      if (employeeId != null)
      {
        if (employeeId.Value == null)
        {
          if (user.Employee != null)
            user.Employee.User = null;
          user.Employee = null;
        }
        else
          user.Employee = GetEmployee((int)employeeId.Value);
      }
      user = EditUser(
                      rowVersion: rowVersion,
                      user: user,
                      userName: userName,
                      password: password,
                      employeeId: employeeId,
                      isActive: isActive,
                      isDeleted: isDeleted,
                      isLocked: isLocked,
                      lockOutTime: lockOutTime,
                      loginFailedCount: loginFailedCount);
      return user;
    }
    public User EditUser(
        byte[] rowVersion,
        int id,
        TValue<string> userName = null,
        TValue<string> password = null,
        TValue<int?> employeeId = null,
        TValue<bool> isActive = null,
        TValue<bool> isDeleted = null,
        TValue<bool> isLocked = null,
        TValue<DateTime> lockOutTime = null,
        TValue<int> loginFailedCount = null,
        TValue<Guid?> documentId = null)
    {
      var user = GetUser(id: id);
      if (user == null)
        throw new UserNotFoundException(id);
      if (userName != null)
      {
        var existUser = GetUsers(userName: userName);
        if (existUser.Count() > 1 || (existUser.Count() == 1 && existUser.FirstOrDefault()?.Id != id))
          throw new UserExistsException(userName);
        user.UserName = userName;
      }
      if (password != null)
        user.Password = password;
      if (isActive != null)
        user.IsActive = isActive;
      if (isDeleted != null)
        user.IsDelete = isDeleted;
      if (lockOutTime != null)
        user.LockOutDateTime = lockOutTime;
      if (loginFailedCount != null)
        user.LoginFailedCount = loginFailedCount;
      if (employeeId != null)
      {
        if (employeeId.Value == null)
        {
          if (user.Employee != null)
            user.Employee.User = null;
          user.Employee = null;
        }
        else
          user.Employee = GetEmployee((int)employeeId.Value);
      }
      if (isLocked != null)
        user.IsLocked = isLocked;
      var securityStamp = JwtManager.generateSeurityStamp(user);
      App.Providers.Session.Set(SessionKey.SecurityStamp.ToString().KeyPrefix(user.Id.ToString()), securityStamp);
      repository.Update(user, rowVersion: rowVersion);
      return user;
    }
    public User EditUser(
         User user,
         byte[] rowVersion,
         TValue<string> userName = null,
         TValue<string> password = null,
         TValue<int?> employeeId = null,
         TValue<bool> isActive = null,
         TValue<bool> isDeleted = null,
         TValue<bool> isLocked = null,
         TValue<DateTime> lockOutTime = null,
         TValue<int> loginFailedCount = null
        )
    {
      if (userName != null)
        user.UserName = userName;
      if (password != null)
        user.Password = password;
      if (isActive != null)
        user.IsActive = isActive;
      if (isDeleted != null)
        user.IsDelete = isDeleted;
      if (lockOutTime != null)
        user.LockOutDateTime = lockOutTime;
      if (loginFailedCount != null)
        user.LoginFailedCount = loginFailedCount;
      if (isLocked != null)
        user.IsLocked = isLocked;
      repository.Update(user, rowVersion: rowVersion);
      return user;
    }
    public void DeleteUser(int id)
    {
      var user = GetUser(id);
      if (user == null)
        throw new UserNotFoundException(id);
      repository.Delete(user);
    }
    public void CheckExpiredPassword(string userName)
    {
      var user = GetUsers(userName: userName).FirstOrDefault();
      if (user == null)
        throw new UserNotFoundException(userName);
      if (user.PasswordExpirationDate > DateTime.Now)
        throw new PasswordNotExpiredException();
    }
    public IQueryable<UserResult> ToUserResultQuery(IQueryable<User> query)
    {
      var resultQuery = from item in query
                        let employee = item.Employee
                        select new UserResult()
                        {
                          Id = item.Id,
                          UserName = item.UserName,
                          EmployeeCode = employee.Code,
                          EmployeeId = employee.Id,
                          FirstName = employee.FirstName,
                          LastName = employee.LastName,
                          FullName = employee.FirstName + " " + employee.LastName,
                          IsActive = item.IsActive,
                          IsLocked = item.IsLocked,
                          LockOutDateTime = item.LockOutDateTime,
                          LoginFailedCount = item.LoginFailedCount,
                          Barcode = "ID" + employee.Code,
                          RowVersion = item.RowVersion
                        };
      return resultQuery;
    }
    public UserResult ToUserResult(User user)
    {
      var employee = user.Employee;
      byte[] file = null;
      var result = new UserResult()
      {
        Id = user.Id,
        UserName = user.UserName,
        EmployeeCode = employee?.Code,
        EmployeeId = employee?.Id,
        FirstName = employee?.FirstName,
        LastName = employee?.LastName,
        FullName = employee?.FirstName + " " + employee?.LastName,
        IsActive = user.IsActive,
        IsLocked = user.IsLocked,
        LockOutDateTime = user.LockOutDateTime,
        LoginFailedCount = user.LoginFailedCount,
        Barcode = "ID" + employee?.Code,
        CanAccessFromInternet = user.HasAccessFromInternet,
        RowVersion = user.RowVersion
      };
      return result;

    }
    public User ChangePassword(string password, string passwordConfimation, string oldPassword)
    {
      var session = App.Providers.Session.GetAs<LoginResult>(SessionKey.UserCredentials.ToString());
      if (session == null)
        throw new SessionNotFoundException();
      var userName = session.UserName;
      var user = GetUsers(userName: userName).FirstOrDefault();
      if (user == null)
        throw new UserNotFoundException(userName);
      if (password != passwordConfimation)
        throw new UserPasswordConfirmationDoseNotMatchException(password, passwordConfimation);
      if (!CheckPasswords(oldPassword, user.Password))
        throw new UserPasswordDoseNotMatchException(oldPassword);
      if (password.Length < 5)
        throw new PasswordInvalidRangeException(password);
      var result = RestPassword(user.Id, password, passwordConfimation);
      return result;
    }
    public User RestPassword(int userId, string password, string passwordConfimation)
    {
      var user = GetUser(userId);
      if (user == null)
        throw new UserNotFoundException(userId);
      if (password != passwordConfimation)
        throw new UserPasswordConfirmationDoseNotMatchException(password, passwordConfimation);
      if (password.Length < 5)
        throw new PasswordInvalidRangeException(password);
      var result = EditUser(rowVersion: user.RowVersion, id: userId, password: Crypto.Sha1(password));
      return result;
    }
    public LoginResult Login(string userName, string password)
    {
      var user = GetUsers(userName: userName).FirstOrDefault();
      if (user == null)
        throw new UserNotFoundException(userName);
      if (user.IsDelete)
        throw new UserIsDeletedException(user.Id, user.UserName);
      if (!user.IsActive)
        throw new UserIsNotActivatedException(user.Id, user.UserName);
      if (user.Employee != null && !user.Employee.IsActive)
        throw new EmployeeIsNotActivatedException(user.Id, user.UserName, user.Employee.Id);
      if (IsBanned(user))
        throw new UserLockedException();
      if (!CheckPasswords(password, user.Password))
      {
        //این تابع باید با تراکنش دیگری اجرا شود
        //TODO fix ssss
        IncreaseFialedLoginCount(userId: user.Id, rowVersion: user.RowVersion);
        throw new UserPasswordIsNotValidException();
      }
      if (user.IsLocked || user.LoginFailedCount > 0)
        UnlockUser(rowVersion: user.RowVersion, userId: user.Id);
      //int TerminalUser = App.Internals.ApplicationSetting.T
      if ((user.PasswordExpirationDate < DateTime.Now) && user.UserName != Models.StaticData.StaticVariables.TerminalUserName)
        throw new PasswordExpirationDateException(user.UserName);
      //generate token and refersh token 
      var timeout = App.Providers.Storage.TokenTimeout;
      var expiresIn = DateTime.Now.AddMinutes(timeout);
      var refreshToken = Extentions.GenrateRefreshToken();
      var stateKey = Guid.NewGuid();
      string securityStamp = String.Empty;
      var token = JwtManager.GenerateToken(user, stateKey, expiresIn, ref securityStamp);
      var loginResult = new LoginResult()
      {
        UserId = user.Id,
        UserName = user.UserName,
        UserFirstName = user.Employee?.FirstName ?? "",
        UserEmployeeCode = user.Employee?.Code ?? "",
        UserLastName = user.Employee?.LastName ?? "",
        Image = user.Employee?.Image ?? null,
        UserEmployeeId = user.Employee?.Id ?? null,
        DepartmentId = user.Employee?.DepartmentId,
        Token = token,
        RefreshToken = refreshToken,
        ExpiresIn = expiresIn
      };
      AddUserToken(
                userId: user.Id,
                token: token,
                refreshToken: refreshToken,
                expiresIn: expiresIn);
      App.Providers.Session.Set(SessionKey.UserCredentials.ToString().KeyPrefix(stateKey.ToString()), loginResult, expiresIn.ComputeTimeSpan());
      App.Providers.Session.Set(SessionKey.SecurityStamp.ToString().KeyPrefix(user.Id.ToString()), securityStamp);
      return loginResult;
    }
    public LoginResult RefreshToken(string token, string refreshToken)
    {
      var claims = JwtManager.GetPrincipal(token, false).Identity as ClaimsIdentity;
      int userId = int.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
      string tokenStamp = claims.FindFirst(x => x.Type == "securityStamp")?.Value;
      string mainStamp = App.Providers.Session.GetAs<string>(SessionKey.SecurityStamp.ToString().KeyPrefix(userId.ToString()));
      if (tokenStamp == null || mainStamp == null || !JwtManager.CheckStamp(Encoding.ASCII.GetBytes(tokenStamp), Encoding.ASCII.GetBytes(mainStamp)))
      {
        throw new UserIsAlteredException();
      }
      var user = GetUsers(id: userId).FirstOrDefault();
      var userToken = GetUserToken(userId: userId, token: token);
      if (refreshToken != userToken.RefreshToken)
        throw new InvalidRefreshTokenException();
      //generate token and refersh token 
      var timeout = App.Providers.Storage.TokenTimeout;
      var expiresIn = DateTime.Now.AddMinutes(timeout);
      var newRefreshToken = Extentions.GenrateRefreshToken();
      var stateKey = Guid.NewGuid();
      string securityStamp = String.Empty;
      var newToken = JwtManager.GenerateToken(user, stateKey, expiresIn, ref securityStamp);
      var loginResult = new LoginResult()
      {
        UserId = user.Id,
        UserName = user.UserName,
        UserFirstName = user.Employee?.FirstName ?? "",
        UserEmployeeCode = user.Employee?.Code ?? "",
        UserLastName = user.Employee?.LastName ?? "",
        Image = user.Employee?.Image ?? null,
        UserEmployeeId = user.Employee?.Id ?? null,
        DepartmentId = user.Employee?.DepartmentId,
        Token = newToken,
        RefreshToken = newRefreshToken,
        ExpiresIn = expiresIn
      };
      AddUserToken(
                userId: user.Id,
                token: newToken,
                refreshToken: newRefreshToken,
                expiresIn: expiresIn);
      RemoveUserToken(userToken: userToken);
      App.Providers.Session.Set(SessionKey.UserCredentials.ToString().KeyPrefix(stateKey.ToString()), loginResult, expiresIn.ComputeTimeSpan());
      App.Providers.Session.Set(SessionKey.SecurityStamp.ToString().KeyPrefix(user.Id.ToString()), securityStamp);
      return loginResult;
    }
    public User ChangeExpiredPassword(string userName, string oldPassword, string newPassword, string newPasswordConfirm)
    {
      var user = GetUsers(userName: userName).FirstOrDefault();
      if (!CheckPasswords(oldPassword, user.Password))
        throw new UserPasswordIsNotValidException();
      if (newPassword != newPasswordConfirm)
        throw new UserPasswordConfirmationDoseNotMatchException(newPassword, newPasswordConfirm);
      if (user.PasswordExpirationDate > DateTime.Now)
        throw new PasswordNotExpiredException();
      int Days = App.Internals.ApplicationSetting.PasswordValidity();
      user.Password = Crypto.Sha1(newPassword);
      user.PasswordExpirationDate = DateTime.Now.AddDays(Days);
      repository.Update(user, rowVersion: user.RowVersion);
      //generate token and refersh token
      var timeout = App.Providers.Storage.TokenTimeout;
      var expiresIn = DateTime.Now.AddMinutes(timeout);
      var refreshToken = Extentions.GenrateRefreshToken();
      var stateKey = Guid.NewGuid();
      string securityStamp = String.Empty;
      var token = JwtManager.GenerateToken(user, stateKey, expiresIn, ref securityStamp);
      var loginResult = new LoginResult()
      {
        UserId = user.Id,
        UserName = user.UserName,
        UserFirstName = user.Employee?.FirstName ?? "",
        UserEmployeeCode = user.Employee?.Code ?? "",
        UserLastName = user.Employee?.LastName ?? "",
        Image = user.Employee?.Image ?? null,
        UserEmployeeId = user.Employee?.Id ?? null,
        DepartmentId = user.Employee?.DepartmentId,
        Token = token,
        RefreshToken = refreshToken,
        ExpiresIn = expiresIn
      };
      AddUserToken(
                userId: user.Id,
                token: token,
                refreshToken: refreshToken,
                expiresIn: expiresIn);
      App.Providers.Session.Set(SessionKey.UserCredentials.ToString().KeyPrefix(stateKey.ToString()), loginResult, expiresIn.ComputeTimeSpan());
      App.Providers.Session.Set(SessionKey.SecurityStamp.ToString().KeyPrefix(user.Id.ToString()), securityStamp);
      return user;
    }
    public void Logout()
    {
      var loginResult = App.Providers.Session.GetAs<LoginResult>(SessionKey.UserCredentials.ToString());
      RemoveUserToken(userId: loginResult.UserId, token: loginResult.Token);
      var key = $"Deactive:{loginResult.Token.ComputeHashToken()}";
      var session = App.Providers.Session;
      session.Remove(SessionKey.UserCredentials.ToString().KeyPrefix(session.StateKey));
      App.Providers.Session.Set(key, string.Empty, loginResult.ExpiresIn.ComputeTimeSpan());
    }
    public User UnlockUser(byte[] rowVersion, int userId)
    {
      return EditUser(rowVersion: rowVersion, id: userId, loginFailedCount: 0, isLocked: false);
    }
    public User LockUser(byte[] rowVersion, int userId)
    {
      var modApplicationSettings = App.Internals.ApplicationSetting;
      var defLockOutTime = DateTime.Now.ToUniversalTime().AddHours(modApplicationSettings.GetUserDefaultLockOutTime());
      return
                EditUser(rowVersion: rowVersion, id: userId, isLocked: true, lockOutTime: defLockOutTime);
    }
    public User ActivateUser(byte[] rowVersion, int userId)
    {
      return EditUser(rowVersion: rowVersion, id: userId, loginFailedCount: 0, isActive: true);
    }
    public User DeactivateUser(byte[] rowVersion, int userId)
    {
      return EditUser(rowVersion: rowVersion, id: userId, loginFailedCount: 0, isActive: false);
    }
    public User RemoveUser(byte[] rowVersion, int userId)
    {
      return EditUser(rowVersion: rowVersion, id: userId, loginFailedCount: 0, isDeleted: true);
    }
    public User RestoreUser(byte[] rowVersion, int userId)
    {
      return EditUser(rowVersion: rowVersion, id: userId, loginFailedCount: 0, isDeleted: false);
    }
    public User ChangeUserName(byte[] rowVersion, int userId, string newUserName)
    {
      //var user = GetUser(userId);
      return EditUser(rowVersion: rowVersion, id: userId, loginFailedCount: 0, userName: newUserName);
    }
    protected bool IsBanned(User user)
    {
      return (user.IsLocked && !IsLockOutTimePassed(user));
    }
    protected bool IsLockOutTimePassed(User user)
    {
      var lockOutDateTime = user.LockOutDateTime;
      var now = DateTime.Now.ToUniversalTime();
      return now > lockOutDateTime;
    }
    protected bool CheckPasswords(string passwordFromUserInput, string passwordFromDatabase)
    {
      var passwordHash = Crypto.Sha1(passwordFromUserInput);
      return passwordHash.Equals(passwordFromDatabase)
          //&& passwordFromUserInput.Length >= 6 
          && passwordFromUserInput.Length < 32;
    }
    protected User IncreaseFialedLoginCount(int userId, byte[] rowVersion)
    {
      var user = GetUser(id: userId);
      var modApplicationSettings = App.Internals.ApplicationSetting;
      var newloginFailedCount = (user.LoginFailedCount) + 1;
      var result = EditUser(rowVersion: rowVersion, id: userId, loginFailedCount: newloginFailedCount);
      var maxFailedLoginCount = modApplicationSettings.GetUserMaxFailedLoginCount();
      if (user.LoginFailedCount >= maxFailedLoginCount)
        result = LockUser(rowVersion: result.RowVersion, userId: user.Id);
      return result;
    }
    internal UserResult GetCurrentUserCredentials(bool throwException = false)
    {
      var credentials = App.Providers.Session.GetAs<UserResult>(SessionKey.UserCredentials.ToString());
      if (credentials == null && throwException)
        throw new LoginException();
      return credentials;
    }
    public IQueryable<UserResult> SearchUser(
      IQueryable<UserResult> query,
      string searchText,
      AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                item.FirstName.Contains(searchText) ||
                item.LastName.Contains(searchText) ||
                item.UserName.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #region Sort
    public IOrderedQueryable<UserResult> SortUserResult(
        IQueryable<UserResult> query,
        SortInput<UserSortType> sort)
    {
      switch (sort.SortType)
      {
        case UserSortType.Username:
          return query.OrderBy(user => user.UserName, sort.SortOrder);
        case UserSortType.FullName:
          return query.OrderBy(user => user.FullName, sort.SortOrder);
        case UserSortType.IsActive:
          return query.OrderBy(user => user.IsActive, sort.SortOrder);
        case UserSortType.Locked:
          return query.OrderBy(user => user.IsLocked, sort.SortOrder);
        case UserSortType.LockOutDateTime:
          return query.OrderBy(user => user.LockOutDateTime, sort.SortOrder);
        case UserSortType.LoginFailedCount:
          return query.OrderBy(user => user.LoginFailedCount, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}