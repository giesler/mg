using System;
using System.Collections.Generic;
using System.Data.Services.Common;

namespace Microsoft.Live
{
    public enum SyndicationItemPropertyEx
{
    CustomProperty,
    AuthorEmail,
    AuthorName,
    AuthorUri,
    ContributorEmail,
    ContributorName,
    ContributorUri,
    Updated,
    LinkHref,
    LinkRel,
    LinkType,
    LinkTitle,
    Id,
    Published,
    Rights,
    Summary,
    Title,
    CategoryName,
    CategoryScheme,
    CategoryLabel
}



    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=true)]
public sealed class EntityPropertyMappingExAttribute : Attribute
{
    // Fields
    private const string AtomNamespacePrefix = "atom";
    private readonly KeyValuePair<string, string> criteria;
    private readonly bool keepInContent;
    private string sourcePath;
    private readonly string targetNamespacePrefix;
    private readonly string targetNamespaceUri;
    private readonly string targetPath;
    private readonly SyndicationItemPropertyEx targetSyndicationItem;
    private readonly SyndicationTextContentKind targetTextContentKind;

    // Methods
    public EntityPropertyMappingExAttribute(string sourcePath, SyndicationItemPropertyEx targetSyndicationItem, SyndicationTextContentKind targetTextContentKind, bool keepInContent)
    {
        if (string.IsNullOrEmpty(sourcePath))
        {
            throw new ArgumentException("EntityPropertyMapping_EpmAttribute(\"sourcePath\")");
        }
        this.sourcePath = sourcePath;
        this.targetPath = SyndicationItemPropertyExToPath(targetSyndicationItem);
        this.targetSyndicationItem = targetSyndicationItem;
        this.targetTextContentKind = targetTextContentKind;
        this.targetNamespacePrefix = "atom";
        this.targetNamespaceUri = "http://www.w3.org/2005/Atom";
        this.keepInContent = keepInContent;
    }

    public EntityPropertyMappingExAttribute(string sourcePath, string targetPath, string targetNamespacePrefix, string targetNamespaceUri, bool keepInContent)
    {
        Uri uri;
        if (string.IsNullOrEmpty(sourcePath))
        {
            throw new ArgumentException("EntityPropertyMapping_EpmAttribute(\"sourcePath\")");
        }
        this.sourcePath = sourcePath;
        if (string.IsNullOrEmpty(targetPath))
        {
            throw new ArgumentException("EntityPropertyMapping_EpmAttribute(\"targetPath\")");
        }
        if (targetPath[0] == '@')
        {
            throw new ArgumentException("EpmTargetTree_InvalidTargetPath(targetPath)");
        }
        this.targetPath = targetPath;
        this.targetSyndicationItem = SyndicationItemPropertyEx.CustomProperty;
        this.targetTextContentKind = SyndicationTextContentKind.Plaintext;
        this.targetNamespacePrefix = targetNamespacePrefix;
        if (string.IsNullOrEmpty(targetNamespaceUri))
        {
            throw new ArgumentException("EntityPropertyMapping_EpmAttribute(\"targetNamespaceUri\")");
        }
        this.targetNamespaceUri = targetNamespaceUri;
        if (!Uri.TryCreate(targetNamespaceUri, UriKind.Absolute, out uri))
        {
            throw new ArgumentException("EntityPropertyMapping_TargetNamespaceUriNotValid(targetNamespaceUri)");
        }
        this.keepInContent = keepInContent;
    }

    public EntityPropertyMappingExAttribute(string sourcePath, SyndicationItemPropertyEx targetSyndicationItem, SyndicationTextContentKind targetTextContentKind, bool keepInContent, string attributeMatchName, string attributeMatchValue)
    {
        if (string.IsNullOrEmpty(sourcePath))
        {
            throw new ArgumentException("EntityPropertyMapping_EpmAttribute(\"sourcePath\")");
        }
        this.sourcePath = sourcePath;
        this.targetPath = SyndicationItemPropertyExToPath(targetSyndicationItem);
        this.targetSyndicationItem = targetSyndicationItem;
        this.targetTextContentKind = targetTextContentKind;
        this.targetNamespacePrefix = "atom";
        this.targetNamespaceUri = "http://www.w3.org/2005/Atom";
        this.keepInContent = keepInContent;
        this.criteria = new KeyValuePair<string, string>(attributeMatchName, attributeMatchValue);
    }

    internal static string SyndicationItemPropertyExToPath(SyndicationItemPropertyEx targetSyndicationItem)
    {
        switch (targetSyndicationItem)
        {
            case SyndicationItemPropertyEx.AuthorEmail:
                return "author/email";

            case SyndicationItemPropertyEx.AuthorName:
                return "author/name";

            case SyndicationItemPropertyEx.AuthorUri:
                return "author/uri";

            case SyndicationItemPropertyEx.ContributorEmail:
                return "contributor/email";

            case SyndicationItemPropertyEx.ContributorName:
                return "contributor/name";

            case SyndicationItemPropertyEx.ContributorUri:
                return "contributor/uri";

            case SyndicationItemPropertyEx.Updated:
                return "updated";

            case SyndicationItemPropertyEx.LinkHref:
                return "link/@href";

            case SyndicationItemPropertyEx.LinkRel:
                return "link/@rel";

            case SyndicationItemPropertyEx.LinkType:
                return "link/@type";

            case SyndicationItemPropertyEx.LinkTitle:
                return "link/@title";

            case SyndicationItemPropertyEx.Id:
                return "id";

            case SyndicationItemPropertyEx.Published:
                return "published";

            case SyndicationItemPropertyEx.Rights:
                return "rights";

            case SyndicationItemPropertyEx.Summary:
                return "summary";

            case SyndicationItemPropertyEx.Title:
                return "title";

            case SyndicationItemPropertyEx.CategoryName:
                return "category/@term";

            case SyndicationItemPropertyEx.CategoryScheme:
                return "category/@scheme";

            case SyndicationItemPropertyEx.CategoryLabel:
                return "category/@label";
        }
        throw new ArgumentException("EntityPropertyMapping_EpmAttribute(\"targetSyndicationItem\")");
    }

    // Properties
    public KeyValuePair<string, string> Criteria
    {
        get
        {
            return this.criteria;
        }
    }

    public bool KeepInContent
    {
        get
        {
            return this.keepInContent;
        }
    }

    public string SourcePath
    {
        get
        {
            return this.sourcePath;
        }
        set
        {
            this.sourcePath = value;
        }
    }

    public string TargetNamespacePrefix
    {
        get
        {
            return this.targetNamespacePrefix;
        }
    }

    public string TargetNamespaceUri
    {
        get
        {
            return this.targetNamespaceUri;
        }
    }

    public string TargetPath
    {
        get
        {
            return this.targetPath;
        }
    }

    public SyndicationItemPropertyEx TargetSyndicationItem
    {
        get
        {
            return this.targetSyndicationItem;
        }
    }

    public SyndicationTextContentKind TargetTextContentKind
    {
        get
        {
            return this.targetTextContentKind;
        }
    }
}

 

}
