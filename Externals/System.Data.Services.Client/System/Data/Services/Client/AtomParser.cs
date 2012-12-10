namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Data.Services.Client.Xml;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;

    [DebuggerDisplay("AtomParser {kind} {reader}")]
    internal class AtomParser
    {
        private string currentDataNamespace;
        private AtomEntry entry;
        private readonly Func<XmlReader, KeyValuePair<XmlReader, object>> entryCallback;
        private AtomFeed feed;
        private AtomDataKind kind;
        private XmlReader reader;
        private readonly Stack<XmlReader> readers;
        private readonly string typeScheme;

        internal AtomParser(XmlReader reader, Func<XmlReader, KeyValuePair<XmlReader, object>> entryCallback, string typeScheme, string currentDataNamespace)
        {
            Debug.Assert(reader != null, "reader != null");
            Debug.Assert(typeScheme != null, "typeScheme != null");
            Debug.Assert(entryCallback != null, "entryCallback != null");
            Debug.Assert(!string.IsNullOrEmpty(currentDataNamespace), "currentDataNamespace is empty or null");
            this.reader = reader;
            this.readers = new Stack<XmlReader>();
            this.entryCallback = entryCallback;
            this.typeScheme = typeScheme;
            this.currentDataNamespace = currentDataNamespace;
            Debug.Assert(this.kind == AtomDataKind.None, "this.kind == AtomDataKind.None -- otherwise not initialized correctly");
        }

        private Uri ConvertHRefAttributeValueIntoURI(string href)
        {
            Uri relativeUri = Util.CreateUri(href, UriKind.RelativeOrAbsolute);
            if (!(relativeUri.IsAbsoluteUri || string.IsNullOrEmpty(this.reader.BaseURI)))
            {
                relativeUri = new Uri(Util.CreateUri(this.reader.BaseURI, UriKind.RelativeOrAbsolute), relativeUri);
            }
            return relativeUri;
        }

        private static bool IsAllowedContentType(string contentType)
        {
            return (string.Equals("application/xml", contentType, StringComparison.OrdinalIgnoreCase) || string.Equals("application/atom+xml", contentType, StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsAllowedLinkType(string linkType, out bool isFeed)
        {
            isFeed = string.Equals("application/atom+xml;type=feed", linkType, StringComparison.OrdinalIgnoreCase);
            return (isFeed || string.Equals("application/atom+xml;type=entry", linkType, StringComparison.OrdinalIgnoreCase));
        }

        private void ParseCurrentContent(AtomEntry targetEntry)
        {
            Debug.Assert(targetEntry != null, "targetEntry != null");
            Debug.Assert(this.reader.NodeType == XmlNodeType.Element, "this.reader.NodeType == XmlNodeType.Element");
            string uriString = this.reader.GetAttributeEx("src", "http://www.w3.org/2005/Atom");
            if (uriString != null)
            {
                if (!this.reader.IsEmptyElement)
                {
                    throw Error.InvalidOperation(Strings.Deserialize_ExpectedEmptyMediaLinkEntryContent);
                }
                targetEntry.MediaLinkEntry = true;
                targetEntry.MediaContentUri = new Uri(uriString, UriKind.RelativeOrAbsolute);
            }
            else
            {
                if (!(!targetEntry.MediaLinkEntry.HasValue ? true : !targetEntry.MediaLinkEntry.Value))
                {
                    throw Error.InvalidOperation(Strings.Deserialize_ContentPlusPropertiesNotAllowed);
                }
                targetEntry.MediaLinkEntry = false;
                if (IsAllowedContentType(this.reader.GetAttributeEx("type", "http://www.w3.org/2005/Atom")) && !this.reader.IsEmptyElement)
                {
                    if (ReadChildElement(this.reader, "properties", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata"))
                    {
                        this.ReadCurrentProperties(targetEntry.DataValues);
                    }
                    else if (this.reader.NodeType != XmlNodeType.EndElement)
                    {
                        throw Error.InvalidOperation(Strings.Deserialize_NotApplicationXml);
                    }
                }
            }
        }

        private void ParseCurrentEntry(out AtomEntry targetEntry)
        {
            Debug.Assert(this.reader.NodeType == XmlNodeType.Element, "this.reader.NodeType == XmlNodeType.Element");
            KeyValuePair<XmlReader, object> pair = this.entryCallback.Invoke(this.reader);
            Debug.Assert(pair.Key != null, "callbackResult.Key != null");
            this.readers.Push(this.reader);
            this.reader = pair.Key;
            this.reader.Read();
            Debug.Assert(this.reader.LocalName == "entry", "this.reader.LocalName == 'entry' - otherwise we're not reading the subtree");
            bool flag = false;
            targetEntry = new AtomEntry();
            targetEntry.DataValues = new List<AtomContentProperty>();
            targetEntry.Tag = pair.Value;
            targetEntry.ETagText = this.reader.GetAttribute("etag", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
            while (this.reader.Read())
            {
                if (!ShouldIgnoreNode(this.reader) && (this.reader.NodeType == XmlNodeType.Element))
                {
                    int depth = this.reader.Depth;
                    string localName = this.reader.LocalName;
                    string namespaceURI = this.reader.NamespaceURI;
                    if (namespaceURI == "http://www.w3.org/2005/Atom")
                    {
                        if ((localName == "category") && (targetEntry.TypeName == null))
                        {
                            if (this.reader.GetAttributeEx("scheme", "http://www.w3.org/2005/Atom") == this.typeScheme)
                            {
                                targetEntry.TypeName = this.reader.GetAttributeEx("term", "http://www.w3.org/2005/Atom");
                            }
                        }
                        else if (localName == "content")
                        {
                            flag = true;
                            this.ParseCurrentContent(targetEntry);
                        }
                        else if ((localName == "id") && (targetEntry.Identity == null))
                        {
                            string str4 = Util.ReferenceIdentity(ReadElementStringForText(this.reader));
                            if (!Util.CreateUri(str4, UriKind.RelativeOrAbsolute).IsAbsoluteUri)
                            {
                                throw Error.InvalidOperation(Strings.Context_TrackingExpectsAbsoluteUri);
                            }
                            targetEntry.Identity = str4;
                        }
                        else if (localName == "link")
                        {
                            this.ParseCurrentLink(targetEntry);
                        }
                    }
                    else if ((namespaceURI == "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata") && (localName == "properties"))
                    {
                        if (!(!targetEntry.MediaLinkEntry.HasValue ? true : targetEntry.MediaLinkEntry.Value))
                        {
                            throw Error.InvalidOperation(Strings.Deserialize_ContentPlusPropertiesNotAllowed);
                        }
                        targetEntry.MediaLinkEntry = true;
                        if (!this.reader.IsEmptyElement)
                        {
                            this.ReadCurrentProperties(targetEntry.DataValues);
                        }
                    }
                    SkipToEndAtDepth(this.reader, depth);
                }
            }
            if (targetEntry.Identity == null)
            {
                throw Error.InvalidOperation(Strings.Deserialize_MissingIdElement);
            }
            if (!flag)
            {
                throw Error.BatchStreamContentExpected(BatchStreamState.GetResponse);
            }
            this.reader = this.readers.Pop();
        }

        private void ParseCurrentFeedCount()
        {
            long num;
            if (this.feed == null)
            {
                throw new InvalidOperationException(Strings.AtomParser_FeedCountNotUnderFeed);
            }
            if (this.feed.Count.HasValue)
            {
                throw new InvalidOperationException(Strings.AtomParser_ManyFeedCounts);
            }
            if (!long.TryParse(MaterializeAtom.ReadElementString(this.reader, true), NumberStyles.Integer, CultureInfo.InvariantCulture, out num))
            {
                throw new FormatException(Strings.MaterializeFromAtom_CountFormatError);
            }
            if (num < 0L)
            {
                throw new FormatException(Strings.MaterializeFromAtom_CountFormatError);
            }
            this.feed.Count = new long?(num);
        }

        private void ParseCurrentFeedPagingLinks()
        {
            Debug.Assert(this.feed != null, "Trying to parser paging links but feed is null.");
            if (this.feed.NextLink != null)
            {
                throw new InvalidOperationException(Strings.AtomMaterializer_DuplicatedNextLink);
            }
            string attribute = this.reader.GetAttribute("href");
            if (attribute == null)
            {
                throw new InvalidOperationException(Strings.AtomMaterializer_LinksMissingHref);
            }
            this.feed.NextLink = this.ConvertHRefAttributeValueIntoURI(attribute);
        }

        private void ParseCurrentLink(AtomEntry targetEntry)
        {
            Debug.Assert(targetEntry != null, "targetEntry != null");
            Debug.Assert(this.reader.NodeType == XmlNodeType.Element, "this.reader.NodeType == XmlNodeType.Element -- otherwise we shouldn't try to parse a link");
            Debug.Assert(this.reader.LocalName == "link", "this.reader.LocalName == 'link' -- otherwise we shouldn't try to parse a link");
            string attribute = this.reader.GetAttribute("rel");
            if (attribute != null)
            {
                string str2;
                if ((attribute == "edit") && (targetEntry.EditLink == null))
                {
                    str2 = this.reader.GetAttribute("href");
                    if (string.IsNullOrEmpty(str2))
                    {
                        throw Error.InvalidOperation(Strings.Context_MissingEditLinkInResponseBody);
                    }
                    targetEntry.EditLink = this.ConvertHRefAttributeValueIntoURI(str2);
                }
                else if ((attribute == "self") && (targetEntry.QueryLink == null))
                {
                    str2 = this.reader.GetAttribute("href");
                    if (string.IsNullOrEmpty(str2))
                    {
                        throw Error.InvalidOperation(Strings.Context_MissingSelfLinkInResponseBody);
                    }
                    targetEntry.QueryLink = this.ConvertHRefAttributeValueIntoURI(str2);
                }
                else if ((attribute == "edit-media") && (targetEntry.MediaEditUri == null))
                {
                    str2 = this.reader.GetAttribute("href");
                    if (string.IsNullOrEmpty(str2))
                    {
                        throw Error.InvalidOperation(Strings.Context_MissingEditMediaLinkInResponseBody);
                    }
                    targetEntry.MediaEditUri = this.ConvertHRefAttributeValueIntoURI(str2);
                    targetEntry.StreamETagText = this.reader.GetAttribute("etag", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
                }
                if (!this.reader.IsEmptyElement)
                {
                    bool flag;
                    string nameFromAtomLinkRelationAttribute = UriUtil.GetNameFromAtomLinkRelationAttribute(attribute);
                    if (((nameFromAtomLinkRelationAttribute != null) && IsAllowedLinkType(this.reader.GetAttribute("type"), out flag)) && ReadChildElement(this.reader, "inline", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata"))
                    {
                        bool isEmptyElement = this.reader.IsEmptyElement;
                        object obj2 = null;
                        if (!isEmptyElement)
                        {
                            AtomFeed currentFeed = null;
                            AtomEntry currentEntry = null;
                            List<AtomEntry> list = null;
                            Debug.Assert(this.reader is XmlWrappingReader, "reader must be a instance of XmlWrappingReader");
                            XmlReader reader = XmlWrappingReader.CreateReader(this.reader.BaseURI, this.reader.ReadSubtree());
                            reader.Read();
                            Debug.Assert(reader.LocalName == "inline", "nestedReader.LocalName == 'inline'");
                            AtomParser parser = new AtomParser(reader, this.entryCallback, this.typeScheme, this.currentDataNamespace);
                            while (parser.Read())
                            {
                                switch (parser.DataKind)
                                {
                                    case AtomDataKind.Entry:
                                        currentEntry = parser.CurrentEntry;
                                        if (list == null)
                                        {
                                            break;
                                        }
                                        list.Add(currentEntry);
                                        goto Label_033D;

                                    case AtomDataKind.Feed:
                                        list = new List<AtomEntry>();
                                        currentFeed = parser.CurrentFeed;
                                        obj2 = currentFeed;
                                        goto Label_033D;

                                    case AtomDataKind.PagingLinks:
                                        goto Label_033D;

                                    default:
                                        throw new InvalidOperationException(Strings.AtomParser_UnexpectedContentUnderExpandedLink);
                                }
                                obj2 = currentEntry;
                            Label_033D:;
                            }
                            if (currentFeed != null)
                            {
                                Debug.Assert(currentFeed.Entries == null, "nestedFeed.Entries == null -- otherwise someone initialized this for us");
                                currentFeed.Entries = list;
                            }
                        }
                        AtomContentProperty item = new AtomContentProperty();
                        item.Name = nameFromAtomLinkRelationAttribute;
                        if (isEmptyElement || (obj2 == null))
                        {
                            item.IsNull = true;
                            if (flag)
                            {
                                item.Feed = new AtomFeed();
                                item.Feed.Entries = Enumerable.Empty<AtomEntry>();
                            }
                            else
                            {
                                item.Entry = new AtomEntry();
                                item.Entry.IsNull = true;
                            }
                        }
                        else
                        {
                            item.Feed = obj2 as AtomFeed;
                            item.Entry = obj2 as AtomEntry;
                        }
                        targetEntry.DataValues.Add(item);
                    }
                }
            }
        }

        private static AtomDataKind ParseStateForReader(XmlReader reader)
        {
            Debug.Assert(reader != null, "reader != null");
            Debug.Assert((reader.NodeType == XmlNodeType.Element) || (reader.NodeType == XmlNodeType.EndElement), "reader.NodeType == XmlNodeType.Element || EndElement -- otherwise can't determine");
            AtomDataKind custom = AtomDataKind.Custom;
            string localName = reader.LocalName;
            string namespaceURI = reader.NamespaceURI;
            if (Util.AreSame("http://www.w3.org/2005/Atom", namespaceURI))
            {
                if (Util.AreSame("entry", localName))
                {
                    return AtomDataKind.Entry;
                }
                if (Util.AreSame("feed", localName))
                {
                    return AtomDataKind.Feed;
                }
                if (Util.AreSame("link", localName) && Util.AreSame("next", reader.GetAttribute("rel")))
                {
                    custom = AtomDataKind.PagingLinks;
                }
                return custom;
            }
            if (Util.AreSame("http://schemas.microsoft.com/ado/2007/08/dataservices/metadata", namespaceURI) && Util.AreSame("count", localName))
            {
                custom = AtomDataKind.FeedCount;
            }
            return custom;
        }

        internal bool Read()
        {
            if (this.DataKind != AtomDataKind.Finished)
            {
                while (this.reader.Read())
                {
                    if (ShouldIgnoreNode(this.reader))
                    {
                        continue;
                    }
                    Debug.Assert((this.reader.NodeType == XmlNodeType.Element) || (this.reader.NodeType == XmlNodeType.EndElement), "this.reader.NodeType == XmlNodeType.Element || this.reader.NodeType == XmlNodeType.EndElement -- otherwise we should have ignored or thrown");
                    AtomDataKind kind = ParseStateForReader(this.reader);
                    if (this.reader.NodeType == XmlNodeType.EndElement)
                    {
                        break;
                    }
                    switch (kind)
                    {
                        case AtomDataKind.Custom:
                            if (this.DataKind != AtomDataKind.None)
                            {
                                break;
                            }
                            this.kind = AtomDataKind.Custom;
                            return true;

                        case AtomDataKind.Entry:
                            this.kind = AtomDataKind.Entry;
                            this.ParseCurrentEntry(out this.entry);
                            return true;

                        case AtomDataKind.Feed:
                            if (this.DataKind != AtomDataKind.None)
                            {
                                throw new InvalidOperationException(Strings.AtomParser_FeedUnexpected);
                            }
                            this.feed = new AtomFeed();
                            this.kind = AtomDataKind.Feed;
                            return true;

                        case AtomDataKind.FeedCount:
                            this.ParseCurrentFeedCount();
                            goto Label_0170;

                        case AtomDataKind.PagingLinks:
                            if (this.feed == null)
                            {
                                throw new InvalidOperationException(Strings.AtomParser_PagingLinkOutsideOfFeed);
                            }
                            goto Label_0150;

                        default:
                            Debug.Assert(false, "Atom Parser is in a wrong state...Did you add a new AtomDataKind?");
                            goto Label_0170;
                    }
                    MaterializeAtom.SkipToEnd(this.reader);
                    continue;
                Label_0150:
                    this.kind = AtomDataKind.PagingLinks;
                    this.ParseCurrentFeedPagingLinks();
                    return true;
                Label_0170:;
                }
                this.kind = AtomDataKind.Finished;
                this.entry = null;
            }
            return false;
        }

        private static bool ReadChildElement(XmlReader reader, string localName, string namespaceUri)
        {
            Debug.Assert(localName != null, "localName != null");
            Debug.Assert(namespaceUri != null, "namespaceUri != null");
            Debug.Assert(!reader.IsEmptyElement, "!reader.IsEmptyElement");
            Debug.Assert(reader.NodeType != XmlNodeType.EndElement, "reader.NodeType != XmlNodeType.EndElement");
            return (reader.Read() && reader.IsStartElement(localName, namespaceUri));
        }

        private void ReadCurrentProperties(List<AtomContentProperty> values)
        {
            Debug.Assert(values != null, "values != null");
            Debug.Assert(this.reader.NodeType == XmlNodeType.Element, "this.reader.NodeType == XmlNodeType.Element");
            while (this.reader.Read())
            {
                if (!ShouldIgnoreNode(this.reader))
                {
                    if (this.reader.NodeType == XmlNodeType.EndElement)
                    {
                        break;
                    }
                    if (this.reader.NodeType == XmlNodeType.Element)
                    {
                        AtomContentProperty item = this.ReadPropertyValue();
                        if (item != null)
                        {
                            values.Add(item);
                        }
                    }
                }
            }
        }

        internal AtomContentProperty ReadCurrentPropertyValue()
        {
            Debug.Assert(this.kind == AtomDataKind.Custom, "this.kind == AtomDataKind.Custom -- otherwise caller shouldn't invoke ReadCurrentPropertyValue");
            return this.ReadPropertyValue();
        }

        internal string ReadCustomElementString()
        {
            Debug.Assert(this.kind == AtomDataKind.Custom, "this.kind == AtomDataKind.Custom -- otherwise caller shouldn't invoke ReadCustomElementString");
            return MaterializeAtom.ReadElementString(this.reader, true);
        }

        private static string ReadElementStringForText(XmlReader reader)
        {
            Debug.Assert(reader != null, "reader != null");
            if (reader.IsEmptyElement)
            {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            int depth = reader.Depth;
            while (reader.Read())
            {
                if (reader.Depth == depth)
                {
                    Debug.Assert(reader.NodeType == XmlNodeType.EndElement, "reader.NodeType == XmlNodeType.EndElement -- otherwise XmlReader is acting odd");
                    break;
                }
                if ((reader.NodeType == XmlNodeType.SignificantWhitespace) || (reader.NodeType == XmlNodeType.Text))
                {
                    builder.Append(reader.Value);
                }
            }
            return builder.ToString();
        }

        private AtomContentProperty ReadPropertyValue()
        {
            Debug.Assert(this.reader != null, "reader != null");
            Debug.Assert(this.reader.NodeType == XmlNodeType.Element, "reader.NodeType == XmlNodeType.Element -- otherwise caller is confused as to where the reader is");
            if (!this.IsDataWebElement)
            {
                SkipToEndAtDepth(this.reader, this.reader.Depth);
                return null;
            }
            AtomContentProperty property = new AtomContentProperty();
            property.Name = this.reader.LocalName;
            property.TypeName = this.reader.GetAttributeEx("type", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata");
            property.IsNull = Util.DoesNullAttributeSayTrue(this.reader);
            property.Text = property.IsNull ? null : string.Empty;
            if (!this.reader.IsEmptyElement)
            {
                int depth = this.reader.Depth;
                while (this.reader.Read())
                {
                    this.ReadPropertyValueIntoResult(property);
                    if (this.reader.Depth == depth)
                    {
                        return property;
                    }
                }
            }
            return property;
        }

        private void ReadPropertyValueIntoResult(AtomContentProperty property)
        {
            Debug.Assert(this.reader != null, "reader != null");
            Debug.Assert(property != null, "property != null");
            switch (this.reader.NodeType)
            {
                case XmlNodeType.Element:
                {
                    if (!string.IsNullOrEmpty(property.Text))
                    {
                        throw Error.InvalidOperation(Strings.Deserialize_ExpectingSimpleValue);
                    }
                    property.EnsureProperties();
                    AtomContentProperty item = this.ReadPropertyValue();
                    if (item != null)
                    {
                        property.Properties.Add(item);
                    }
                    return;
                }
                case XmlNodeType.Text:
                case XmlNodeType.CDATA:
                case XmlNodeType.SignificantWhitespace:
                    if (!string.IsNullOrEmpty(property.Text))
                    {
                        throw Error.InvalidOperation(Strings.Deserialize_MixedTextWithComment);
                    }
                    property.Text = this.reader.Value;
                    return;

                case XmlNodeType.ProcessingInstruction:
                case XmlNodeType.Comment:
                case XmlNodeType.Whitespace:
                case XmlNodeType.EndElement:
                    return;
            }
            throw Error.InvalidOperation(Strings.Deserialize_ExpectingSimpleValue);
        }

        internal void ReplaceReader(XmlReader newReader)
        {
            Debug.Assert(newReader != null, "newReader != null");
            this.reader = newReader;
        }

        private static bool ShouldIgnoreNode(XmlReader reader)
        {
            Debug.Assert(reader != null, "reader != null");
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                case XmlNodeType.EndElement:
                    return false;

                case XmlNodeType.Text:
                case XmlNodeType.SignificantWhitespace:
                    Error.ThrowInternalError(InternalError.UnexpectedXmlNodeTypeWhenReading);
                    break;

                case XmlNodeType.CDATA:
                case XmlNodeType.EntityReference:
                case XmlNodeType.EndEntity:
                    Error.ThrowInternalError(InternalError.UnexpectedXmlNodeTypeWhenReading);
                    break;
            }
            return true;
        }

        private static void SkipToEndAtDepth(XmlReader reader, int depth)
        {
            Debug.Assert(reader != null, "reader != null");
            Debug.Assert(reader.Depth >= depth, "reader.Depth >= depth");
            while ((reader.Depth != depth) || ((reader.NodeType != XmlNodeType.EndElement) && ((reader.NodeType != XmlNodeType.Element) || !reader.IsEmptyElement)))
            {
                reader.Read();
            }
        }

        internal static KeyValuePair<XmlReader, object> XElementBuilderCallback(XmlReader reader)
        {
            Debug.Assert(reader != null, "reader != null");
            Debug.Assert(reader is XmlWrappingReader, "reader must be a instance of XmlWrappingReader");
            string baseURI = reader.BaseURI;
            XElement element = XElement.Load(reader.ReadSubtree(), 0);
            return new KeyValuePair<XmlReader, object>(XmlWrappingReader.CreateReader(baseURI, element.CreateReader()), element);
        }

        internal AtomEntry CurrentEntry
        {
            get
            {
                return this.entry;
            }
        }

        internal AtomFeed CurrentFeed
        {
            get
            {
                return this.feed;
            }
        }

        internal AtomDataKind DataKind
        {
            get
            {
                return this.kind;
            }
        }

        internal bool IsDataWebElement
        {
            get
            {
                return (this.reader.NamespaceURI == this.currentDataNamespace);
            }
        }
    }
}

