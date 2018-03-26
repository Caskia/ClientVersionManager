using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace ClientVersionManager.Routes
{
    public class AppNameRouteConstraint : IRouteConstraint
    {
        private readonly IConfiguration _configuration;

        public AppNameRouteConstraint()
        {
            _configuration = Startup.Configuration;
        }

        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            // check nulls
            object value;
            if (values.TryGetValue(routeKey, out value) && value != null)
            {
                return _configuration.AsEnumerable()
                    .Select(c => c.Key)
                    .FirstOrDefault(c => c.StartsWith($"App:{value}")) != null;
            }

            return false;
        }
    }
}