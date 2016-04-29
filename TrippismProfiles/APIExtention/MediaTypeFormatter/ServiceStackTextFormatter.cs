using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TrippismProfiles
{
    /// <summary>
    /// It's used to ServiceStack Text Formatter.
    /// </summary>
    public class ServiceStackTextFormatter : MediaTypeFormatter
    {
        /// <summary>
        /// It's used to intialize ServiceStackTextFormatter variables.
        /// </summary>
        public ServiceStackTextFormatter()
        {
            JsConfig.DateHandler = JsonDateHandler.ISO8601;
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));

            SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true));
            SupportedEncodings.Add(new UnicodeEncoding(bigEndian: false, byteOrderMark: true, throwOnInvalidBytes: true));
        }
        /// <summary>
        /// It's used to validate ReadType.
        /// </summary>
        public override bool CanReadType(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            return true;
        }
        /// <summary>
        /// It's used to validate WriteType.
        /// </summary>
        public override bool CanWriteType(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            return true;
        }
        /// <summary>
        /// It's used to read from stream async.
        /// </summary>
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            var task = Task<object>.Factory.StartNew(() => JsonSerializer.DeserializeFromStream(type, readStream));
            return task;
        }
        /// <summary>
        /// It's used to write to stream async.
        /// </summary>
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            var task = Task.Factory.StartNew(() => JsonSerializer.SerializeToStream(value, type, writeStream));
            return task;
        }
    }
}