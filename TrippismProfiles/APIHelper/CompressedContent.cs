using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TrippismProfiles
{

    /// <summary>
    /// This class is used for content compression
    /// </summary>
        public class CompressedContent : HttpContent
        {
            private readonly string _encodingType;
            private readonly HttpContent _originalContent;

            /// <summary>
            /// This method is used for content compression
            /// </summary>
            public CompressedContent(HttpContent content, string encodingType = "gzip")
            {
                if (content == null)
                {
                    throw new ArgumentNullException("content");
                }
                _originalContent = content;
                _encodingType = encodingType.ToLowerInvariant();
                foreach (var header in _originalContent.Headers)
                {
                    Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
                Headers.ContentEncoding.Add(encodingType);
            }
            /// <summary>
            /// This method is used for compute lenth
            /// </summary>
            protected override bool TryComputeLength(out long length)
            {
                length = -1;
                return false;
            }
            /// <summary>
            /// This method is used for serialization
            /// </summary>
            protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
            {
                Stream compressedStream = null;
                switch (_encodingType)
                {
                    case "gzip":
                        compressedStream = new GZipStream(stream, CompressionMode.Compress, true);
                        break;
                    case "deflate":
                        compressedStream = new DeflateStream(stream, CompressionMode.Compress, true);
                        break;
                    default:
                        compressedStream = stream;
                        break;
                }
                return _originalContent.CopyToAsync(compressedStream).ContinueWith(tsk =>
                {
                    if (compressedStream != null)
                    {
                        compressedStream.Dispose();
                    }
                });
            }
        }
}