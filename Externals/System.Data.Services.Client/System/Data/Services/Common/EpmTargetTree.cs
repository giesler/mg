namespace System.Data.Services.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data.Services.Client;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;

    internal sealed class EpmTargetTree
    {
        public EpmTargetPathSegment NonSyndicationRoot { get; set; }
        public EpmTargetPathSegment SyndicationRoot { get; set; }
        //[CompilerGenerated]
        //private EpmTargetPathSegment <NonSyndicationRoot>k__BackingField;
        //[CompilerGenerated]
        //private EpmTargetPathSegment <SyndicationRoot>k__BackingField;
        private int countOfNonContentProperties;

        internal EpmTargetTree()
        {
            this.SyndicationRoot = new EpmTargetPathSegment();
            this.NonSyndicationRoot = new EpmTargetPathSegment();
        }

        internal void Add(EntityPropertyMappingInfo epmInfo)
        {
            string targetPath = epmInfo.Attribute.TargetPath;
            bool isSyndication = epmInfo.Attribute.TargetSyndicationItem != SyndicationItemProperty.CustomProperty;
            string namespaceUri = epmInfo.Attribute.TargetNamespaceUri;
            string targetNamespacePrefix = epmInfo.Attribute.TargetNamespacePrefix;
            EpmTargetPathSegment parentSegment = isSyndication ? this.SyndicationRoot : this.NonSyndicationRoot;
            IList<EpmTargetPathSegment> subSegments = parentSegment.SubSegments;
            Debug.Assert(!string.IsNullOrEmpty(targetPath), "Must have been validated during EntityPropertyMappingAttribute construction");
            string[] strArray = targetPath.Split(new char[] { '/' });
            for (int i = 0; i < strArray.Length; i++)
            {
                //<>c__DisplayClass3 class2;
                //<>c__DisplayClass1 class3;
                //<>c__DisplayClass1 CS$<>8__locals2 = class3;
                string targetSegment = strArray[i];
                       if ((targetSegment[0] == '@') && (i != (strArray.Length - 1)))
        {
            throw new InvalidOperationException(Strings.EpmTargetTree_AttributeInMiddle(targetSegment));
        }
        EpmTargetPathSegment segment2 = subSegments.SingleOrDefault<EpmTargetPathSegment>(delegate (EpmTargetPathSegment segment) {
            return (segment.SegmentName == targetSegment) && (isSyndication || (segment.SegmentNamespaceUri == namespaceUri));
        });

                if (segment2 != null)
                {
                    parentSegment = segment2;
                }
                else
                {
                    parentSegment = new EpmTargetPathSegment(targetSegment, namespaceUri, targetNamespacePrefix, parentSegment);
                    if (targetSegment[0] == '@')
                    {
                        subSegments.Insert(0, parentSegment);
                    }
                    else
                    {
                        subSegments.Add(parentSegment);
                    }
                }
                subSegments = parentSegment.SubSegments;
            }
            if (parentSegment.HasContent)
            {
                throw new ArgumentException(Strings.EpmTargetTree_DuplicateEpmAttrsWithSameTargetName(GetPropertyNameFromEpmInfo(parentSegment.EpmInfo), parentSegment.EpmInfo.DefiningType.Name, parentSegment.EpmInfo.Attribute.SourcePath, epmInfo.Attribute.SourcePath));
            }
            if (!epmInfo.Attribute.KeepInContent)
            {
                this.countOfNonContentProperties++;
            }
            parentSegment.EpmInfo = epmInfo;
            if (HasMixedContent(this.NonSyndicationRoot, false))
            {
                throw new InvalidOperationException(Strings.EpmTargetTree_InvalidTargetPath(targetPath));
            }
        }

        private static string GetPropertyNameFromEpmInfo(EntityPropertyMappingInfo epmInfo)
        {
            if (epmInfo.Attribute.TargetSyndicationItem != SyndicationItemProperty.CustomProperty)
            {
                return epmInfo.Attribute.TargetSyndicationItem.ToString();
            }
            return epmInfo.Attribute.TargetPath;
        }

        private static bool HasMixedContent(EpmTargetPathSegment currentSegment, bool ancestorHasContent)
        {
             foreach (EpmTargetPathSegment segment in currentSegment.SubSegments.Where<EpmTargetPathSegment>(delegate (EpmTargetPathSegment s) {
        return !s.IsAttribute;
    }))
    {
        if (segment.HasContent && ancestorHasContent)
        {
            return true;
        }
        if (HasMixedContent(segment, segment.HasContent || ancestorHasContent))
        {
            return true;
        }
    }
    return false;

            //if (CS$<>9__CachedAnonymousMethodDelegateb == null)
            //{
            //    CS$<>9__CachedAnonymousMethodDelegateb = new Func<EpmTargetPathSegment, bool>(null, (IntPtr) <HasMixedContent>b__a);
            //}
            //foreach (EpmTargetPathSegment segment in Enumerable.Where<EpmTargetPathSegment>(currentSegment.SubSegments, CS$<>9__CachedAnonymousMethodDelegateb))
            //{
            //    if (segment.HasContent && ancestorHasContent)
            //    {
            //        return true;
            //    }
            //    if (HasMixedContent(segment, segment.HasContent || ancestorHasContent))
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }

        internal void Remove(EntityPropertyMappingInfo epmInfo)
        {
            string targetPath = epmInfo.Attribute.TargetPath;
            bool isSyndication = epmInfo.Attribute.TargetSyndicationItem != SyndicationItemProperty.CustomProperty;
            string namespaceUri = epmInfo.Attribute.TargetNamespaceUri;
            EpmTargetPathSegment item = isSyndication ? this.SyndicationRoot : this.NonSyndicationRoot;
            List<EpmTargetPathSegment> subSegments = item.SubSegments;
            Debug.Assert(!string.IsNullOrEmpty(targetPath), "Must have been validated during EntityPropertyMappingAttribute construction");
            string[] strArray = targetPath.Split(new char[] { '/' });
            for (int i = 0; i < strArray.Length; i++)
            {
                string targetSegment = strArray[i];
                if (targetSegment.Length == 0)
                {
                    throw new InvalidOperationException(Strings.EpmTargetTree_InvalidTargetPath(targetPath));
                }
                if ((targetSegment[0] == '@') && (i != (strArray.Length - 1)))
                {
                    throw new InvalidOperationException(Strings.EpmTargetTree_AttributeInMiddle(targetSegment));
                }
                EpmTargetPathSegment segment2 = subSegments.FirstOrDefault<EpmTargetPathSegment>(delegate(EpmTargetPathSegment segment)
                {
                    return (segment.SegmentName == targetSegment) && (isSyndication || (segment.SegmentNamespaceUri == namespaceUri));
                });
                if (segment2 != null)
                {
                    item = segment2;
                }
                else
                {
                    return;
                }
                subSegments = item.SubSegments;
            }
            if (item.HasContent)
            {
                if (!item.EpmInfo.Attribute.KeepInContent)
                {
                    this.countOfNonContentProperties--;
                }
                do
                {
                    EpmTargetPathSegment parentSegment = item.ParentSegment;
                    parentSegment.SubSegments.Remove(item);
                    item = parentSegment;
                }
                while (((item.ParentSegment != null) && !item.HasContent) && (item.SubSegments.Count == 0));
            }

        }

        internal bool IsV1Compatible
        {
            get
            {
                return (this.countOfNonContentProperties == 0);
            }
        }

        //internal EpmTargetPathSegment NonSyndicationRoot
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<NonSyndicationRoot>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<NonSyndicationRoot>k__BackingField = value;
        //    }
        //}

        //internal EpmTargetPathSegment SyndicationRoot
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<SyndicationRoot>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<SyndicationRoot>k__BackingField = value;
        //    }
        //}
    }
}

