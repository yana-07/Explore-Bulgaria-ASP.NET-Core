using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace ExploreBulgaria.Web.Extensions
{
    public static class ControllerExtensions
    {
        private static readonly ConcurrentDictionary<string, string> actionNameCache
            = new ConcurrentDictionary<string, string>();

        public static IActionResult RedirectTo<TController>(
            this Controller controller,
            Expression<Action<TController>> redirectExpression)
        {
            if (redirectExpression.Body.NodeType != ExpressionType.Call)
            {
                throw new InvalidOperationException($"The provided expression is not a valid method call: {redirectExpression.Body}");
            }

            var methodCallExpr = (MethodCallExpression)redirectExpression.Body;

            var routeValues = ExtractRouteValues(methodCallExpr);

            var actionName = GetActionName(methodCallExpr);

            var controllerName = typeof(TController).Name.Replace(nameof(Controller), string.Empty);

            return controller.RedirectToAction(actionName, controllerName, routeValues);
        }

        private static string GetActionName(MethodCallExpression methodCallExpr)
        {
            var cacheKey = $"{methodCallExpr.Method.Name}_{methodCallExpr.Object?.Type.Name}";

            return actionNameCache.GetOrAdd(cacheKey, _ =>
            {
                var actionName = methodCallExpr.Method.Name;

                var actionNameAttribute = methodCallExpr
                    .Method
                    .GetCustomAttributes(true)
                    .OfType<ActionNameAttribute>()
                    .FirstOrDefault()
                    ?.Name;

                return actionNameAttribute ?? actionName;
            });           
        }

        private static RouteValueDictionary ExtractRouteValues(MethodCallExpression methodCallExpr)
        {
            var names = methodCallExpr.Method
                .GetParameters()
                .Select(p => p.Name)
                .ToArray();

            var values = methodCallExpr.Arguments
                .Select(arg =>
                {
                    if (arg.NodeType == ExpressionType.Constant)
                    {
                        var constantExpr = (ConstantExpression)arg;
                        return constantExpr.Value;
                    }

                    // () => (object)arg = Func<object>
                    var convertExpr = Expression.Convert(arg, typeof(object));
                    var lambdaExpr = Expression.Lambda<Func<object>>(convertExpr);
                    return lambdaExpr.Compile().Invoke();
                })
                .ToArray();

            var routeValueDictionary = new RouteValueDictionary();

            for (int i = 0; i < names.Length; i++)
            {
                routeValueDictionary.Add(names[i], values[i]);
            }

            return routeValueDictionary;
        }
    }
}