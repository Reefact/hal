#region Usings declarations

using System.Collections;
using System.Dynamic;
using System.Net;
using System.Reflection;
using System.Text;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Reefact.Hateoas.Hal.Builders;

#endregion

namespace Reefact.Hateoas.Hal.AspNetCore {

    /// <summary>
    ///     Represents the filter attribute that applies HAL document on the response JSON.
    /// </summary>
    public class SupportsHalAttribute : ResultFilterAttribute {

        #region Statics members declarations

        private static (string?, string?, string?) InferGetByIdControllerActionName(ResultExecutingContext context, Type idPropertyType) {
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor) {
                var methods = from m in controllerActionDescriptor.ControllerTypeInfo.GetMethods()
                              let parameters = m.GetParameters()                                                                                              // gets parameter definition of the method
                              let customHttpAttributes = m.GetCustomAttributes(false).Where(attr => attr.GetType().IsSubclassOf(typeof(HttpMethodAttribute))) // gets the decorated HttpMethodAttributes
                              where m.IsPublic                                                                                                           &&   // the method should be public
                                    (!customHttpAttributes.Any() || customHttpAttributes.Any(a => a.GetType() == typeof(HttpGetAttribute)))              &&   // the method should either be decorated by the HttpGetAttribute, or not decorated by any HttpMethodAttribute
                                    (!m.DeclaringType?.Namespace?.StartsWith("Microsoft") ?? true)                                                       &&   // ignore the methods from Microsoft namespace (which is tricky, but helpful for filtering out unnecessary methods)
                                    (typeof(IActionResult).IsAssignableFrom(m.ReturnType) || typeof(Task<IActionResult>).IsAssignableFrom(m.ReturnType)) &&   // the method return type should be of IActionResult or Task<IActionResult>
                                    parameters.Length           == 1                                                                                     &&   // method should have only 1 parameter
                                    parameters[0].ParameterType == idPropertyType                                                                             // the type of the parameter should be the same as the Id property type
                              select new {
                                  Method    = m,
                                  Parameter = parameters[0]
                              };
                if (methods.Any()) {
                    MethodInfo?    getByIdMethod          = null;
                    ParameterInfo? getByIdMethodParameter = null;
                    if (methods.Count() > 1) {
                        var methodSig = methods.FirstOrDefault(m => m.Method.IsDefined(typeof(GetByIdMethodImplAttribute), false));
                        if (methodSig != null) {
                            getByIdMethod          = methodSig.Method;
                            getByIdMethodParameter = methodSig.Parameter;
                        }
                    } else {
                        getByIdMethod          = methods.First().Method;
                        getByIdMethodParameter = methods.First().Parameter;
                    }

                    return (controllerActionDescriptor.ControllerName, getByIdMethod?.Name, getByIdMethodParameter?.Name);
                }
            }

            return (default, default, default);
        }

        private static bool IsPagedResult(IActionResult result, out IPagedResult? pagedResult, out Type? entityType) {
            bool isPagedResult = result is OkObjectResult okObj      &&
                                 okObj.Value != null                 &&
                                 okObj.Value.GetType().IsGenericType &&
                                 okObj.Value.GetType().GetGenericTypeDefinition() == typeof(PagedResult<>);
            if (isPagedResult) {
                entityType  = ((OkObjectResult)result).Value?.GetType().GetGenericArguments().First();
                pagedResult = (result as OkObjectResult)?.Value as IPagedResult;
            } else {
                entityType  = null;
                pagedResult = null;
            }

            return isPagedResult;
        }

        private static string MakeCamelCase(string src) {
            return $"{src[0].ToString().ToLower()}{src[1..]}";
        }

        #endregion

        #region Fields declarations

        private readonly JsonSerializerSettings        _jsonSerializerSettings;
        private readonly IUrlHelperFactory             _urlHelperFactory;
        private readonly IWebHostEnvironment           _hostingEnvironment;
        private readonly ILogger<SupportsHalAttribute> _logger;
        private readonly SupportsHalOptions            _options;

        #endregion

        #region Constructors declarations

        /// <summary>
        ///     Initializes a new instance of <c>SupportsHalAttribute</c> class.
        /// </summary>
        /// <param name="options">The options that is used for configuring the HAL support.</param>
        /// <param name="jsonSerializerSettings">The options that is used for configuring json serializer settings.</param>
        public SupportsHalAttribute(IOptions<SupportsHalOptions> options, IOptions<JsonSerializerSettings> jsonSerializerSettings, ILogger<SupportsHalAttribute> logger, IWebHostEnvironment hostingEnvironment, IUrlHelperFactory urlHelperFactory) {
            (Order, _options, _jsonSerializerSettings, _logger, _hostingEnvironment, _urlHelperFactory) = (2, options.Value, jsonSerializerSettings.Value, logger, hostingEnvironment, urlHelperFactory);
        }

        #endregion

        /// <inheritdoc />
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next) {
            ControllerActionDescriptor? controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (!_options.Enabled) {
                if (IsPagedResult(context.Result, out IPagedResult? pagedResult, out _)) {
                    string        dataCollectionName = MakeCamelCase(controllerActionDescriptor?.ControllerName ?? "data");
                    ExpandoObject resultObj          = new();
                    resultObj.TryAdd("page", pagedResult!.PageNumber);
                    resultObj.TryAdd("size", pagedResult!.PageSize);
                    resultObj.TryAdd("totalPages", pagedResult!.TotalPages);
                    resultObj.TryAdd("totalCount", pagedResult!.TotalRecords);
                    resultObj.TryAdd(dataCollectionName, pagedResult!);
                    context.Result = new OkObjectResult(resultObj);
                }

                await base.OnResultExecutionAsync(context, next);
            } else {
                int      originalStatusCode = (int)HttpStatusCode.OK;
                string   selfLinkItem       = context.HttpContext.Request.GetEncodedUrl();
                Resource resource;
                if (IsPagedResult(context.Result, out IPagedResult? pagedResult, out Type? entityType)) {
                    IPagedResult state         = pagedResult!;
                    int          size          = state.PageSize;
                    int          number        = state.PageNumber;
                    long         totalElements = state.TotalRecords;
                    long         totalPages    = state.TotalPages;

                    LinkItemBuilder linkItemBuilder = new ResourceBuilder()
                                                      .WithState(new { page = new { number, size, totalElements, totalPages } })
                                                      .AddSelfLink().WithLinkItem(selfLinkItem);

                    string firstLinkItem = GenerateLink(context, new Dictionary<string, StringValues> { { "page", 1.ToString() } });
                    linkItemBuilder = linkItemBuilder.AddLink("first").WithLinkItem(firstLinkItem);

                    string lastLinkItem = GenerateLink(context, new Dictionary<string, StringValues> { { "page", totalPages.ToString() } });
                    linkItemBuilder = linkItemBuilder.AddLink("last").WithLinkItem(lastLinkItem);
                    if (number > 1 && number <= totalPages) {
                        string? prevLinkItem = GenerateLink(context, new Dictionary<string, StringValues> { { "page", (number - 1).ToString() } });
                        linkItemBuilder = linkItemBuilder.AddLink("prev").WithLinkItem(prevLinkItem);
                    }

                    if (number >= 1 && number < totalPages) {
                        string? nextLinkItem = GenerateLink(context, new Dictionary<string, StringValues> { { "page", (number + 1).ToString() } });
                        linkItemBuilder = linkItemBuilder.AddLink("next").WithLinkItem(nextLinkItem);
                    }

                    string   embeddedResourceName = MakeCamelCase(controllerActionDescriptor?.ControllerName ?? "data");
                    IBuilder resourceBuilder;
                    if (HasIdProperty(entityType)) {
                        TypeInfo             controllerTypeInfo           = controllerActionDescriptor!.ControllerTypeInfo;
                        CustomAttributeData? controllerRouteAttributeType = controllerTypeInfo.CustomAttributes?.FirstOrDefault(x => x.AttributeType == typeof(RouteAttribute));
                        string?              controllerRouteName          = controllerRouteAttributeType?.ConstructorArguments[0].Value?.ToString() ?? controllerActionDescriptor?.ControllerName;
                        if (!string.IsNullOrEmpty(controllerRouteName) && controllerRouteName.Contains("[controller]")) {
                            controllerRouteName = controllerRouteName.Replace("[controller]", controllerActionDescriptor?.ControllerName);
                        }

                        IEnumerable<JObject> newState = ConvertListToHalResourceObjects(context, state);

                        resourceBuilder = linkItemBuilder.AddEmbedded(embeddedResourceName)
                                                         .Resource(new ResourceBuilder().WithState(newState));
                    } else {
                        resourceBuilder = linkItemBuilder.AddEmbedded(embeddedResourceName)
                                                         .Resource(new ResourceBuilder().WithState(state));
                    }
                    resource = resourceBuilder.Build();
                } else if (context.Result is ObjectResult objectResult && objectResult.Value != null) {
                    originalStatusCode = objectResult.StatusCode ?? (int)HttpStatusCode.OK;
                    if (originalStatusCode < 200 || originalStatusCode > 299) {
                        await base.OnResultExecutionAsync(context, next);

                        return;
                    }

                    IBuilder? resourceBuilder;
                    if (objectResult is CreatedAtActionResult createdAtActionResult) {
                        IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(context);
                        selfLinkItem = urlHelper.ActionLink(createdAtActionResult.ActionName, createdAtActionResult.ControllerName, createdAtActionResult.RouteValues) ?? selfLinkItem;
                    }
                    if (objectResult.Value is not string && objectResult.Value is IEnumerable enumerableResult) {
                        IEnumerable<JObject> newState = ConvertListToHalResourceObjects(context, enumerableResult);
                        resourceBuilder = new ResourceBuilder()
                                         .WithState(new { count = enumerableResult.Cast<object>().Count() })
                                         .AddSelfLink().WithLinkItem(selfLinkItem)
                                         .AddEmbedded(MakeCamelCase(controllerActionDescriptor?.ControllerName ?? "data"))
                                         .Resource(new ResourceBuilder().WithState(newState));
                    } else {
                        resourceBuilder = new ResourceBuilder()
                                         .WithState(objectResult.Value)
                                         .AddSelfLink().WithLinkItem(selfLinkItem);
                    }

                    resource = resourceBuilder.Build();
                } else {
                    await base.OnResultExecutionAsync(context, next);

                    return;
                }

                string json  = resource.ToString(_jsonSerializerSettings);
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                context.HttpContext.Response.ContentLength = bytes.Length;
                context.HttpContext.Response.ContentType   = "application/hal+json; charset=utf-8";
                using MemoryStream ms = new(bytes);
                context.HttpContext.Response.StatusCode = originalStatusCode;
                await ms.CopyToAsync(context.HttpContext.Response.Body);
            }
        }

        private IEnumerable<JObject> ConvertListToHalResourceObjects(ResultExecutingContext context, IEnumerable state) {
            JObject[] emptyResult = Array.Empty<JObject>();
            if (!state.Cast<object>().Any()) {
                return emptyResult;
            }

            object? firstObj = state.Cast<object>().First();
            // assumes that all objects in the list are having the same Id property
            if (TryGetIdPropertyValue(firstObj, out _, out Type? firstObjIdPropertyType) && firstObjIdPropertyType is not null) {
                var            newState               = new List<JObject>();
                JsonSerializer entityObjectSerializer = JsonSerializer.Create(_jsonSerializerSettings);

                (string? controllerName, string? getByIdMethodName, string? getByIdParamName) = InferGetByIdControllerActionName(context, firstObjIdPropertyType);

                foreach (object? obj in state) {
                    if (obj != null && TryGetIdPropertyValue(obj, out object? idValue, out _)) {
                        JObject jobj = JObject.FromObject(obj, entityObjectSerializer);
                        if (!string.IsNullOrEmpty(controllerName)    &&
                            !string.IsNullOrEmpty(getByIdMethodName) &&
                            !string.IsNullOrEmpty(getByIdParamName)  &&
                            idValue is not null                      &&
                            context.Controller is ControllerBase ctlr) {
                            var dict = new Dictionary<string, object> {
                                { getByIdParamName, idValue }
                            };
                            string? pathOverride = ctlr.Url.Action(getByIdMethodName, controllerName, dict);
                            jobj.Add("_links", JToken.FromObject(new {
                                self = new {
                                    href = $"{GenerateLink(context, pathOverride: pathOverride)}"
                                }
                            }));
                        } else {
                            _logger.LogDebug("Unable to locate the controller action that can return the object by a given ID. Use the link to the current resource as the self link.");
                            jobj.Add("_links", JToken.FromObject(new {
                                self = new {
                                    href = $"{GenerateLink(context)}"
                                }
                            }));
                        }

                        JObject? modifiedObj = jobj.ToObject<JObject>();
                        if (modifiedObj != null) {
                            newState.Add(modifiedObj);
                        }
                    }
                }

                return newState;
            }

            return emptyResult;
        }

        private string GenerateLink(ResultExecutingContext context, IEnumerable<KeyValuePair<string, StringValues>>? querySubstitution = null, string? pathOverride = default) {
            HttpRequest request = context.HttpContext.Request;
            string      scheme;
            if (_options.UseHttpsScheme.HasValue) {
                scheme = _options.UseHttpsScheme.Value ? "https" : request.Scheme;
            } else {
                scheme = _hostingEnvironment.IsProduction() ? "https" : request.Scheme;
            }

            HostString host     = request.Host;
            PathString pathBase = string.IsNullOrEmpty(pathOverride) ? request.PathBase : PathString.Empty;
            PathString path     = request.Path;
            if (!string.IsNullOrEmpty(pathOverride)) {
                path = pathOverride;
            }

            var substQuery = new Dictionary<string, StringValues>();

            if (querySubstitution != null) {
                if (request.Query?.Count > 0) {
                    request.Query.ToList().ForEach(q => substQuery.Add(q.Key, q.Value));
                    foreach (KeyValuePair<string, StringValues> subst in querySubstitution) {
                        if (substQuery.ContainsKey(subst.Key)) {
                            substQuery[subst.Key] = subst.Value;
                        } else {
                            substQuery.Add(subst.Key, subst.Value);
                        }
                    }
                } else {
                    querySubstitution.ToList().ForEach(kvp => substQuery.Add(kvp.Key, kvp.Value));
                }
            }

            return UriHelper.BuildAbsolute(scheme, host, pathBase, path, QueryString.Create(substQuery));
        }

        private bool HasIdProperty(Type? entityType) {
            return entityType != null &&
                   (from property in entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    where property.Name == _options.IdPropertyName &&
                          property.CanRead
                    select property).Count() == 1;
        }

        private bool TryGetIdPropertyValue(object? obj, out object? val, out Type? idPropertyType) {
            if (obj == null) {
                val            = null;
                idPropertyType = null;

                return false;
            }

            PropertyInfo? idProperty = (from property in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        where property.Name == _options.IdPropertyName && property.CanRead
                                        select property).FirstOrDefault();
            if (idProperty == null) {
                val            = null;
                idPropertyType = null;

                return false;
            }

            val            = idProperty.GetValue(obj);
            idPropertyType = idProperty.PropertyType;

            return true;
        }

    }

}