namespace System.Data.Services.Client.Xml
{
    using System;
    using System.Data.Services.Client;
    using System.Diagnostics;
    using System.Xml;

    [DebuggerDisplay("XmlAtomErrorReader {NodeType} {Name} {Value}")]
    internal class XmlAtomErrorReader : XmlWrappingReader
    {
        internal XmlAtomErrorReader(XmlReader baseReader) : base(baseReader)
        {
            Debug.Assert(baseReader != null, "baseReader != null");
            base.Reader = baseReader;
        }

        public override bool Read()
        {
            bool flag = base.Read();
            if ((this.NodeType == XmlNodeType.Element) && Util.AreSame(base.Reader, "error", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata"))
            {
                throw new DataServiceClientException(Strings.Deserialize_ServerException(ReadErrorMessage(base.Reader)));
            }
            return flag;
        }

        internal static string ReadElementString(XmlReader reader, bool checkNullAttribute)
        {
            Debug.Assert(reader != null, "reader != null");
            Debug.Assert(XmlNodeType.Element == reader.NodeType, "not positioned on Element");
            string str = null;
            bool flag = checkNullAttribute && !Util.DoesNullAttributeSayTrue(reader);
            if (reader.IsEmptyElement)
            {
                return (flag ? string.Empty : null);
            }
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Text:
                    case XmlNodeType.CDATA:
                    case XmlNodeType.SignificantWhitespace:
                        if (null != str)
                        {
                            throw Error.InvalidOperation(Strings.Deserialize_MixedTextWithComment);
                        }
                        str = reader.Value;
                        break;

                    case XmlNodeType.Comment:
                    case XmlNodeType.Whitespace:
                        break;

                    case XmlNodeType.EndElement:
                        return (str ?? (flag ? string.Empty : null));

                    default:
                        throw Error.InvalidOperation(Strings.Deserialize_ExpectingSimpleValue);
                }
            }
            throw Error.InvalidOperation(Strings.Deserialize_ExpectingSimpleValue);
        }

        private static string ReadErrorMessage(XmlReader reader)
        {
            Debug.Assert(reader != null, "reader != null");
            Debug.Assert(reader.NodeType == XmlNodeType.Element, "reader.NodeType == XmlNodeType.Element");
            Debug.Assert(reader.LocalName == "error", "reader.LocalName == XmlConstants.XmlErrorElementName");
            int num = 1;
            while ((num > 0) && reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (!reader.IsEmptyElement)
                    {
                        num++;
                    }
                    if ((num == 2) && Util.AreSame(reader, "message", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata"))
                    {
                        return ReadElementString(reader, false);
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    num--;
                }
            }
            return string.Empty;
        }
    }
}

