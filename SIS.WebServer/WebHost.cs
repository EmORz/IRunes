using System;
using System.Linq;
using System.Reflection;
using SIS.HTTP.Enums;
using SIS.HTTP.Responses.Contracts;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Routing;
using SIS.WebServer.Attributes;

namespace SIS.WebServer
{
    public static class WebHost
    {
        public static void Start(IMvcApplication application)
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();
            AutoRegisterRoutes(application, serverRoutingTable);
    
            
            application.Configure(serverRoutingTable);
            application.ConfigureServices();
            var server = new Server(8000, serverRoutingTable);
            server.Run();

        }

        private static void AutoRegisterRoutes( IMvcApplication application, IServerRoutingTable serverRoutingTable)
        {
            //todo refactoring
            var controllers = application.GetType().Assembly.GetTypes().Where(type =>
                type.IsClass && !type.IsAbstract && typeof(Controller).IsAssignableFrom(type));

            foreach (var controller in controllers)
            {
                var actions = controller.GetMethods(
                    BindingFlags.DeclaredOnly
                    |BindingFlags.Public
                    |BindingFlags.Instance
                    ).Where(type => type.DeclaringType==controller && !type.IsSpecialName);

                foreach (var methodInfo in actions)
                {
                    var path = ($"/{controller.Name.Replace("Controller", "")}/{methodInfo.Name}");
                    var attribute =
                        methodInfo.GetCustomAttributes().Where(
                            x => x.GetType().IsSubclassOf(typeof(BaseHttpAttribute))).LastOrDefault() as BaseHttpAttribute;
                   // Console.WriteLine(attribute?.AttributeType.Name);
                    var httpMethod = HttpRequestMethod.Get;

                    if (attribute != null)
                    {
                        httpMethod = attribute.Method;
                    }
                    if (attribute?.Url != null)
                    {
                        path = attribute.Url;
                    }
                    if (attribute?.ActionName != null)
                    {
                        path = ($"/{controller.Name.Replace("Controller", "")}/{attribute.ActionName}");
                    }

                    serverRoutingTable.Add(httpMethod, path, request =>
                    {
                        var controllerInstance = Activator.CreateInstance(controller);
                       var response = methodInfo.Invoke(controllerInstance, new[] {request}) as IHttpResponse;
                       return response;
                    });
                    Console.WriteLine(httpMethod + " " + path);


                }

            }
        }
        
    }
}