#region Usings declarations

#endregion

namespace Reefact.Hateoas.Hal.AspNetCore {

    /// <summary>
    ///     Represents that the decorated methods in a ASP.NET Core Web API controller
    ///     could return the resource by using its identifier.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class GetByIdMethodImplAttribute : Attribute { }

}