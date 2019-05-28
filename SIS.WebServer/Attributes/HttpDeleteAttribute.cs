﻿using SIS.HTTP.Enums;
using SIS.WebServer.Attributes;

namespace SIS.MvcFramework.Attributes
{
    public class HttpDeleteAttribute : BaseHttpAttribute
    {
        public override HttpRequestMethod Method => HttpRequestMethod.Delete;
    }
}