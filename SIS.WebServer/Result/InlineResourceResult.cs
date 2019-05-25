﻿using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Result
{
    public class InlineResourceResult : HttpResponse
    {
        public InlineResourceResult(byte[] content, HttpResponseStatusCode responseStatusCode)
        :base(responseStatusCode)
        {
            this.Headers.AddHeader(new HttpHeader(HttpHeader.ContentLength, content.ToString()));
            this.Headers.AddHeader(new HttpHeader(HttpHeader.ContentDisposition, "inline"));
            this.Content = content;

        }
        
    }
}