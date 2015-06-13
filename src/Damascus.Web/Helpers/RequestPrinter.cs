namespace Damascus.Web
{
    using System;
    using System.IO;
    using Microsoft.AspNet.Mvc;
    using Microsoft.AspNet.Http;
    
    /// <summary>
    /// Extension methods for HTTP Request.
    /// <remarks>
    /// See the HTTP 1.1 specification http://www.w3.org/Protocols/rfc2616/rfc2616.html
    /// for details of implementation decisions.
    /// </remarks>
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Dump the raw http request to a string. 
        /// </summary>
        /// <param name="request">The <see cref="HttpRequest"/> that should be dumped.       </param>
        /// <returns>The raw HTTP request.</returns>
        public static string ToRaw(this HttpRequest request)
        {
            StringWriter writer = new StringWriter();

            WriteStartLine(request, writer);
            WriteHeaders(request, writer);
            WriteBody(request, writer);

            return writer.ToString();
        }

        private static void WriteStartLine(HttpRequest request, StringWriter writer)
        {
            const string SPACE = " ";

            writer.Write(request.Method);
            writer.Write(SPACE + request.Path);
            writer.WriteLine(SPACE + request.Protocol);
        }

        private static void WriteHeaders(HttpRequest request, StringWriter writer)
        {
            foreach (var key in request.Headers)
            {
                writer.WriteLine(string.Format("{0}: {1}", key.Key, string.Join("," , key.Value)));
            }

            writer.WriteLine();
        }
        
        
        private static void WriteBody(HttpRequest request, StringWriter writer)
        {
            using(var reader = new StreamReader(request.Body)){
                writer.WriteLine(reader.ReadToEnd());    
            }
        }
        
    }
}