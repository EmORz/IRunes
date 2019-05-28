using SIS.HTTP.Enums;
using SIS.WebServer.Attributes;

namespace SIS.MvcFramework.Attributes
{
    public class HttpPostAttribute : BaseHttpAttribute
    {
        public override HttpRequestMethod Method => HttpRequestMethod.Post;
    }
}