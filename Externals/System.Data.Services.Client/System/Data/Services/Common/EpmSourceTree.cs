namespace System.Data.Services.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data.Services.Client;
    using System.Diagnostics;
    using System.Linq;

    internal sealed class EpmSourceTree
    {
        private readonly EpmTargetTree epmTargetTree;
        private readonly EpmSourcePathSegment root = new EpmSourcePathSegment("");

        internal EpmSourceTree(EpmTargetTree epmTargetTree)
        {
            this.epmTargetTree = epmTargetTree;
        }

        internal void Add(EntityPropertyMappingInfo epmInfo)
        {
            String sourceName = epmInfo.Attribute.SourcePath;
            EpmSourcePathSegment currentProperty = this.Root;
            IList<EpmSourcePathSegment> activeSubProperties = currentProperty.SubProperties;
            EpmSourcePathSegment foundProperty = null;

            Debug.Assert(!String.IsNullOrEmpty(sourceName), "Must have been validated during EntityPropertyMappingAttribute construction");
            foreach (String propertyName in sourceName.Split('/'))
            {
                if (propertyName.Length == 0)
                {
                    throw new InvalidOperationException(Strings.EpmSourceTree_InvalidSourcePath(epmInfo.DefiningType.Name, sourceName));
                }

                foundProperty = activeSubProperties.SingleOrDefault(e => e.PropertyName == propertyName);
                if (foundProperty != null)
                {
                    currentProperty = foundProperty;
                }
                else
                {
                    currentProperty = new EpmSourcePathSegment(propertyName);
                    activeSubProperties.Add(currentProperty);
                }

                activeSubProperties = currentProperty.SubProperties;
            }

            if (foundProperty != null)
            {
                Debug.Assert(Object.ReferenceEquals(foundProperty, currentProperty), "currentProperty variable should have been updated already to foundProperty");

                if (foundProperty.EpmInfo.DefiningType.Name == epmInfo.DefiningType.Name)
                {
                    throw new InvalidOperationException(Strings.EpmSourceTree_DuplicateEpmAttrsWithSameSourceName(epmInfo.Attribute.SourcePath, epmInfo.DefiningType.Name));
                }
                this.epmTargetTree.Remove(foundProperty.EpmInfo);
            }

            currentProperty.EpmInfo = epmInfo;
            this.epmTargetTree.Add(epmInfo);
        }

        internal EpmSourcePathSegment Root
        {
            get
            {
                return this.root;
            }
        }
    }
}

