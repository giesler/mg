namespace System.Data.Services.Common
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Xml;

    internal abstract class EpmContentSerializerBase
    {
        public object Element { get; set; }
        public EpmTargetPathSegment Root { get; set; }
        public bool Success { get; set; }
        public XmlWriter Target { get; set; }
        //[CompilerGenerated]
        //private object <Element>k__BackingField;
        //[CompilerGenerated]
        //private EpmTargetPathSegment <Root>k__BackingField;
        //[CompilerGenerated]
        //private bool <Success>k__BackingField;
        //[CompilerGenerated]
        //private XmlWriter <Target>k__BackingField;

        protected EpmContentSerializerBase(EpmTargetTree tree, bool isSyndication, object element, XmlWriter target)
        {
            this.Root = isSyndication ? tree.SyndicationRoot : tree.NonSyndicationRoot;
            this.Element = element;
            this.Target = target;
            this.Success = false;
        }

        internal void Serialize()
        {
            foreach (EpmTargetPathSegment segment in this.Root.SubSegments)
            {
                this.Serialize(segment, EpmSerializationKind.All);
            }
            this.Success = true;
        }

        protected virtual void Serialize(EpmTargetPathSegment targetSegment, EpmSerializationKind kind)
        {
            IEnumerable<EpmTargetPathSegment> subSegments;
            switch (kind)
            {
                case EpmSerializationKind.Attributes:
                    subSegments = targetSegment.SubSegments.Where<EpmTargetPathSegment>(delegate(EpmTargetPathSegment s)
                    {
                        return s.IsAttribute;
                    });
                    break;

                case EpmSerializationKind.Elements:
                    subSegments = targetSegment.SubSegments.Where<EpmTargetPathSegment>(delegate(EpmTargetPathSegment s)
                    {
                        return !s.IsAttribute;
                    });
                    break;

                default:
                    Debug.Assert(kind == EpmSerializationKind.All, "Must serialize everything");
                    subSegments = targetSegment.SubSegments;
                    break;
            }
            foreach (EpmTargetPathSegment segment in subSegments)
            {
                this.Serialize(segment, kind);
            }

        }

        //protected object Element
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Element>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<Element>k__BackingField = value;
        //    }
        //}

        //protected EpmTargetPathSegment Root
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Root>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<Root>k__BackingField = value;
        //    }
        //}

        //protected bool Success
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Success>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<Success>k__BackingField = value;
        //    }
        //}

        //protected XmlWriter Target
        //{
        //    [CompilerGenerated]
        //    get
        //    {
        //        return this.<Target>k__BackingField;
        //    }
        //    [CompilerGenerated]
        //    private set
        //    {
        //        this.<Target>k__BackingField = value;
        //    }
        //}
    }
}

