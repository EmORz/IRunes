using System;
using SIS.HTTP.Enums;

namespace SIS.WebServer.Attributes
{
    public abstract class BaseHttpAttribute: Attribute
    {
        public string ActionName { get; set; }

        public string Url { get; set; }

        public abstract HttpRequestMethod Method { get;}   
        
    }
}