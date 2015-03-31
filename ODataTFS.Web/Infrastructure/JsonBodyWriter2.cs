using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
namespace Microsoft.Samples.DPE.ODataTFS.Web.Infrastructure
{
    /// <summary>
    /// Custom BodyWriter for creating JSONP Responses.
    /// note: copied out of OData toolkit source as it was marked internal
    /// </summary>
    internal class JsonBodyWriter2 : BodyWriter
    {
        private readonly string content;
        private readonly Encoding contentEncoding;

        /// <summary>
        /// Initializes a new instance of the JsonBodyWriter class.
        /// </summary>
        /// <param name="content">Content to be written.</param>
        internal JsonBodyWriter2(string content)
            : this(content, Encoding.UTF8)
        {
        }

        /// <summary>
        /// Initializes a new instance of the JsonBodyWriter class.
        /// </summary>
        /// <param name="content">Content to be written.</param>
        /// <param name="contentEncoding">Encoding to be used on the writing.</param>
        internal JsonBodyWriter2(string content, Encoding contentEncoding)
            : base(false)
        {
            this.content = content;
            this.contentEncoding = contentEncoding;
        }

        /// <summary>
        /// Writes the given content to output writer.
        /// </summary>
        /// <param name="writer">Output writer instance.</param>
        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            var buffer = this.contentEncoding.GetBytes(this.content);

            writer.WriteStartElement("Binary");
            writer.WriteBase64(buffer, 0, buffer.Length);
            writer.WriteEndElement();
        }
    }
}