//TODO fixit sssss
// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection;
// using System.Text.RegularExpressions;
// using System.Web.Http;
// //using System.Web.Http.Results;
// using Newtonsoft.Json;
// using lena.Models.Common;
// //using lena.Services.Common.Helpers;
// namespace lena.Services.Common.Helpers
// {
//   public class ApiHelper
//   {
//     public static dynamic InvokeApiAction(string apiRelativeUrl, params object[] parameters)
//     {
//       var parts = apiRelativeUrl.Split('/');
//       if (string.IsNullOrEmpty(apiRelativeUrl) || parts.Length == 0 || parts.Length > 2)
//         throw new Exception("Api relative url is not valid. valid format is ControllerName/ActionName");
//       return InvokeApiAction(
//           controllerName: parts[0],
//           actionName: parts[1],
//           parameters: parameters);
//     }
//     public static dynamic InvokeApiAction(string controllerName, string actionName, params object[] parameters)
//     {
//       Assembly asm = AppDomain.CurrentDomain.GetAssemblies().
//      SingleOrDefault(assembly => assembly.GetName().Name == "Parlar.API");// Assembly.GetExecutingAssembly();
//       var fullControllerName = controllerName + "Controller";
//       var controllerType = asm
//           .GetTypes().FirstOrDefault(type => typeof(ApiController).IsAssignableFrom(type) && type.Name == fullControllerName);
//       if (controllerType == null)
//         throw new Exception($"Controller '{controllerName}' not found! ");
//       //var controllerInstance = asm.CreateInstance(fullControllerName, true);
//       var controllerInstance = Activator.CreateInstance(controllerType, false);
//       var methodType = controllerType
//           .GetMethods().FirstOrDefault(method => method.Name == actionName && method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)));
//       if (methodType == null)
//         throw new Exception($"Action '{actionName}' not found!");
//       object invokeResult = null;
//       var methodDefinitionParams = methodType.GetParameters();
//       if (parameters.Length != 0 && methodDefinitionParams.Length != 0)
//       {
//         if (parameters[0] is string)
//         {
//           var json = parameters[0].ToString();
//           //Remove PaginInput 
//           json = Regex.Replace(json, "\"PagingInput\":.*?},", "");
//           var methodParam = JsonConvert.DeserializeObject(json, methodDefinitionParams[0].ParameterType);
//           invokeResult = methodType.Invoke(controllerInstance, new object[] { methodParam });
//         }
//         else
//           invokeResult = methodType.Invoke(controllerInstance, new[] { parameters[0] });
//       }
//       else if (methodDefinitionParams.Length != 0 && parameters.Length == 0)
//         throw new Exception("Controller action paramethers can not be null");
//       else //if (methodDefinitionParams.Length == 0)
//         invokeResult = methodType.Invoke(controllerInstance, null);
//       //if (invokeResult != null)
//       //    ConvertUtcDateToLocal(invokeResult);
//       return invokeResult;
//     }
//     public static List<ApiMethodResult> GetAssemblyApiMethods(Assembly asm)
//     {
//       var totalControllersMethods = asm.GetTypes()
//           .Where(type => typeof(ApiController).IsAssignableFrom(type))
//           .SelectMany(type => type.GetMethods())
//           .Where(method => method.IsPublic
//                            && method.DeclaringType != null
//                            && method.DeclaringType.FullName != typeof(ApiController).FullName
//                            && method.DeclaringType.FullName != typeof(object).FullName
//                            && !method.IsDefined(typeof(NonActionAttribute))
//                            && Consts.SecurityCheckIgnoreList.All(x => x != method.Name)
//                            )
//           .Select(x => new ApiMethodResult()
//           {
//             GroupName = x.DeclaringType?.Name.Replace("Controller", ""),
//             Name = x.Name,
//             ActionName = $"/api/{x.DeclaringType?.Name.Replace("Controller", "")}/{x.Name}",
//           })
//           .ToList();
//       return totalControllersMethods;
//     }
//   }
//   public class ApiMethodResult
//   {
//     public string GroupName { get; set; }
//     public string Name { get; set; }
//     public string ActionName { get; set; }
//   }
// }