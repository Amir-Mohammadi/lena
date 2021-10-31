using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using lena.Models.Users;
using core.Authentication;
using core.Crypto;
using lena.Services.Core;
using core.Messaging;
using core.Setting;
using core.StateManager;
using core.Autofac;
namespace lena.Services
{
  public interface IAuthenticationService : IScopedDependency
  {
    Task<AuthenticationResult> Authenticate(string phone, CancellationToken cancellationToken = default);
    Task<AuthenticationResult> VerifyAuthenticate(string verificationCode, string verificationToken, CancellationToken cancellationToken = default);
    Task<AuthenticationResult> Register(string firstName, string lastName, string email, string registerToken, CancellationToken cancellationToken = default);
    Task Logout();
    Task<ProfileResult> GetUserProfile(CancellationToken cancellationToken);
    Task EditUserProfile(string firstName, string lastName, string email, CancellationToken cancellationToken);
  }
  public class AuthenticationService : IAuthenticationService
  {
    // #region Fields
    // private readonly ISiteSettingProvider siteSettingProvider;
    // private readonly ILocalizer localizer;
    // private readonly IStateManagerService stateManagerService;
    // private readonly ILenaErrorFactory lenaErrorFactory;
    // private readonly IMessagingService messagingService;
    // private readonly ICryptoService cryptoService;
    // private readonly ICurrentContext currentContext;
    // private readonly ITokenManagerService tokenManagerService;
    // #endregion
    // public AuthenticationService(ISiteSettingProvider siteSettingProvider,
    //                              ILocalizer localizer,
    //                              IStateManagerService stateManagerService,
    //                              ILenaErrorFactory lenaErrorFactory,
    //                              IMessagingService messagingService,
    //                              ICryptoService cryptoService,
    //                              ICurrentContext currentContext,
    //                              ITokenManagerService tokenManagerService)
    // {
    //   this.siteSettingProvider = siteSettingProvider;
    //   this.customerService = customerService;
    //   this.localizer = localizer;
    //   this.stateManagerService = stateManagerService;
    //   this.lenaErrorFactory = lenaErrorFactory;
    //   this.messagingService = messagingService;
    //   this.cryptoService = cryptoService;
    //   this.currentContext = currentContext;
    //   this.tokenManagerService = tokenManagerService;
    // }
    // public async Task<AuthenticationResult> Authenticate(string phone, CancellationToken cancellationToken = default)
    // {
    //   await SendCode(phone: phone, cancellationToken: cancellationToken);
    //   var claims = new List<Claim>
    //   {
    //     new Claim("phone", phone),
    //     new Claim("token-type",  TokenType.Verify.ToString())
    //   };
    //   var token = await tokenManagerService.GenerateToken(tokenSetting: siteSettingProvider.SiteSetting.AuthTokenSetting,
    //                                                       claims: claims.ToArray());
    //   return new AuthenticationResult()
    //   {
    //     Token = token,
    //     TokenType = TokenType.Verify,
    //     RefreshToken = null
    //   };
    // }
    // public async Task<AuthenticationResult> VerifyAuthenticate(string verificationCode,
    //                                                            string verificationToken,
    //                                                            CancellationToken cancellationToken = default)
    // {
    //   var claimsPrincipal = await tokenManagerService.ValidateToken(token: verificationToken,
    //                                                                 tokenSetting: siteSettingProvider.SiteSetting.AuthTokenSetting);
    //   var tokenTypeValue = claimsPrincipal.FindFirstValue("token-type");
    //   if (tokenTypeValue != TokenType.Verify.ToString())
    //     throw lenaErrorFactory.InvalidToken();
    //   var phone = claimsPrincipal.FindFirstValue("phone");
    //   await VerifyPhone(phone: phone,
    //                     code: verificationCode);
    //   var customer = await customerService.GetCustomerByPhone(phone: phone,
    //                                                           cancellationToken: cancellationToken);
    //   if (customer != null)
    //   {
    //     return await Login(customer: customer);
    //   }
    //   var tokenType = TokenType.Register;
    //   var tokenSetting = siteSettingProvider.SiteSetting.AuthTokenSetting;
    //   var claims = new List<Claim>
    //   {
    //     new Claim("phone", phone),
    //     new Claim("token-type", tokenType.ToString()),
    //   };
    //   var token = await tokenManagerService.GenerateToken(tokenSetting: tokenSetting,
    //                                                       claims: claims.ToArray());
    //   return new AuthenticationResult()
    //   {
    //     Token = token,
    //     RefreshToken = null,
    //     TokenType = tokenType
    //   };
    // }
    // private async Task<AuthenticationResult> Login()
    // {
    //   var tokenSetting = siteSettingProvider.SiteSetting.TokenSetting;
    //   var securityStamp = tokenManagerService.GenerateSecurityStamp(user: customer);
    //   var claims = new List<Claim>
    //   {
    //     new Claim("phone", customer.Phone),
    //     new Claim("user-id", customer.Id.Id),
    //     new Claim("security-stamp", securityStamp),
    //     new Claim("token-type", TokenType.Auth.ToString()),
    //   };
    //   await tokenManagerService.SetSecurityStamp(userId: customer.Id.Id,
    //                                              securityStamp: securityStamp);
    //   var token = await tokenManagerService.GenerateToken(tokenSetting: tokenSetting,
    //                                                       claims: claims.ToArray());
    //   //TODO refresh token
    //   return new AuthenticationResult()
    //   {
    //     Token = token,
    //     TokenType = TokenType.Auth,
    //     RefreshToken = token
    //   };
    // }
    // public async Task Logout()
    // {
    //   var hashToken = cryptoService.Hash(currentContext.GetUserToken());
    //   await tokenManagerService.DeactivateToken(token: hashToken,
    //                                             expireDate: currentContext.GetUserTokenExpiration());
    // }
    // public async Task<string> SendCode(string phone, CancellationToken cancellationToken = default)
    // {
    //   var random = new Random();
    //   string code = random.Next(10000, 99999).ToString();
    //   var expireTimeSpan = TimeSpan.FromMinutes(siteSettingProvider.SiteSetting.VerificationCodeExpireMinutes);
    //   await stateManagerService.SetState(key: phone,
    //                                      value: code,
    //                                      timeSpan: expireTimeSpan);
    //   var template = localizer["fa"]["OTP-Template"].Value;
    //   var message = template.Replace("{Code}", code);
    //   await messagingService.SendSMS(phone: phone,
    //                                  message: message,
    //                                  cancellationToken: cancellationToken);
    //   return code;
    // }
    // public async Task VerifyPhone(string phone, string code)
    // {
    //   var verification = await stateManagerService.GetState<string>(phone);
    //   if (verification == null || verification != code)
    //   {
    //     throw lenaErrorFactory.InValidVerificationCode();
    //   }
    //   await stateManagerService.RemoveState(phone);
    // }
    // public async Task<AuthenticationResult> Register(string firstName,
    //                                                  string lastName,
    //                                                  string email,
    //                                                  string registerToken,
    //                                                  CancellationToken cancellationToken = default)
    // {
    //   var claimsPrincipal = await tokenManagerService.ValidateToken(token: registerToken,
    //                                                                 tokenSetting: siteSettingProvider.SiteSetting.AuthTokenSetting);
    //   var tokenTypeValue = claimsPrincipal.FindFirstValue("token-type");
    //   if (tokenTypeValue != TokenType.Register.ToString())
    //     throw lenaErrorFactory.InvalidToken();
    //   var phone = claimsPrincipal.FindFirstValue("phone");
    //   var customer = await customerService.Register(phone: phone,
    //                                                 firstName: firstName,
    //                                                 lastName: lastName,
    //                                                 email: email,
    //                                                 cancellationToken: cancellationToken);
    //   // return await Login(customer: customer);
    //   return null;
    // }
    // public async Task<ProfileResult> GetUserProfile(CancellationToken cancellationToken)
    // {
    //   var customer = await customerService.GetCurrentCustomer(cancellationToken: cancellationToken);
    //   return new ProfileResult
    //   {
    //     Id = customer.Id.Id,
    //     Phone = customer.Phone,
    //     Email = customer.Email,
    //     FirstName = customer.AdditionalInfo.GetValueOrDefault("FirstName"),
    //     LastName = customer.AdditionalInfo.GetValueOrDefault("LastName"),
    //   };
    // }
    // public async Task EditUserProfile(string firstName, string lastName, string email, CancellationToken cancellationToken)
    // {
    //   var customer = await customerService.GetCurrentCustomer(cancellationToken: cancellationToken);
    //   customer.Email = email;
    //   if (customer.AdditionalInfo.ContainsKey("FirstName"))
    //     customer.AdditionalInfo["FirstName"] = firstName;
    //   else
    //     customer.AdditionalInfo.Add("FirstName", firstName);
    //   if (customer.AdditionalInfo.ContainsKey("LastName"))
    //     customer.AdditionalInfo["LastName"] = lastName;
    //   else
    //     customer.AdditionalInfo.Add("LastName", lastName);
    //   await customerService.SaveCustomer(customer: customer,
    //                                      cancellationToken: cancellationToken);
    // }
    public Task<AuthenticationResult> Authenticate(string phone, CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }
    public Task EditUserProfile(string firstName, string lastName, string email, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
    public Task<ProfileResult> GetUserProfile(CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
    public Task Logout()
    {
      throw new NotImplementedException();
    }
    public Task<AuthenticationResult> Register(string firstName, string lastName, string email, string registerToken, CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }
    public Task<AuthenticationResult> VerifyAuthenticate(string verificationCode, string verificationToken, CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }
  }
}