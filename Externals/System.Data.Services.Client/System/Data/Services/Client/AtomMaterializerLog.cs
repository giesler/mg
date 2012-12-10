namespace System.Data.Services.Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;

    internal class AtomMaterializerLog
    {
        private readonly Dictionary<string, AtomEntry> appendOnlyEntries;
        private readonly DataServiceContext context;
        private readonly Dictionary<string, AtomEntry> foundEntriesWithMedia;
        private readonly Dictionary<string, AtomEntry> identityStack;
        private object insertRefreshObject;
        private readonly List<LinkDescriptor> links;
        private readonly MergeOption mergeOption;

        internal AtomMaterializerLog(DataServiceContext context, MergeOption mergeOption)
        {
            Debug.Assert(context != null, "context != null");
            this.appendOnlyEntries = new Dictionary<string, AtomEntry>(EqualityComparer<string>.Default);
            this.context = context;
            this.mergeOption = mergeOption;
            this.foundEntriesWithMedia = new Dictionary<string, AtomEntry>(EqualityComparer<string>.Default);
            this.identityStack = new Dictionary<string, AtomEntry>(EqualityComparer<string>.Default);
            this.links = new List<LinkDescriptor>();
        }

        internal void AddedLink(AtomEntry source, string propertyName, object target)
        {
            Debug.Assert(source != null, "source != null");
            Debug.Assert(propertyName != null, "propertyName != null");
            if (this.Tracking && (ShouldTrackWithContext(source) && ShouldTrackWithContext(target)))
            {
                LinkDescriptor item = new LinkDescriptor(source.ResolvedObject, propertyName, target, EntityStates.Added);
                this.links.Add(item);
            }
        }

        private void ApplyMediaEntryInformation(AtomEntry entry, EntityDescriptor descriptor)
        {
            Debug.Assert(entry != null, "entry != null");
            Debug.Assert(descriptor != null, "descriptor != null");
            if ((entry.MediaEditUri != null) || (entry.MediaContentUri != null))
            {
                if (entry.MediaEditUri != null)
                {
                    descriptor.EditStreamUri = new Uri(this.context.BaseUriWithSlash, entry.MediaEditUri);
                }
                if (entry.MediaContentUri != null)
                {
                    descriptor.ReadStreamUri = new Uri(this.context.BaseUriWithSlash, entry.MediaContentUri);
                }
                descriptor.StreamETag = entry.StreamETagText;
            }
        }

        internal void ApplyToContext()
        {
            Debug.Assert((this.mergeOption != MergeOption.OverwriteChanges) || (this.foundEntriesWithMedia.Count == 0), "mergeOption != MergeOption.OverwriteChanges || foundEntriesWithMedia.Count == 0 - we only use the 'entries-with-media' lookaside when we're not in overwrite mode, otherwise we track everything through identity stack");
            if (this.Tracking)
            {
                EntityDescriptor entityDescriptor;
                foreach (KeyValuePair<string, AtomEntry> pair in this.identityStack)
                {
                    AtomEntry entry = pair.Value;
                    if ((entry.CreatedByMaterializer || (entry.ResolvedObject == this.insertRefreshObject)) || entry.ShouldUpdateFromPayload)
                    {
                        entityDescriptor = new EntityDescriptor(pair.Key, entry.QueryLink, entry.EditLink, entry.ResolvedObject, null, null, null, entry.ETagText, EntityStates.Unchanged);
                        entityDescriptor = this.context.InternalAttachEntityDescriptor(entityDescriptor, false);
                        entityDescriptor.State = EntityStates.Unchanged;
                        this.ApplyMediaEntryInformation(entry, entityDescriptor);
                        entityDescriptor.ServerTypeName = entry.TypeName;
                    }
                    else
                    {
                        EntityStates states;
                        this.context.TryGetEntity(pair.Key, entry.ETagText, this.mergeOption, out states);
                    }
                }
                foreach (AtomEntry entry in this.foundEntriesWithMedia.Values)
                {
                    Debug.Assert(entry.ResolvedObject != null, "entry.ResolvedObject != null -- otherwise it wasn't found");
                    entityDescriptor = this.context.GetEntityDescriptor(entry.ResolvedObject);
                    this.ApplyMediaEntryInformation(entry, entityDescriptor);
                }
                foreach (LinkDescriptor descriptor2 in this.links)
                {
                    if (EntityStates.Added == descriptor2.State)
                    {
                        if ((EntityStates.Deleted == this.context.GetEntityDescriptor(descriptor2.Target).State) || (EntityStates.Deleted == this.context.GetEntityDescriptor(descriptor2.Source).State))
                        {
                            this.context.DeleteLink(descriptor2.Source, descriptor2.SourceProperty, descriptor2.Target);
                        }
                        else
                        {
                            this.context.AttachLink(descriptor2.Source, descriptor2.SourceProperty, descriptor2.Target, this.mergeOption);
                        }
                    }
                    else if (EntityStates.Modified == descriptor2.State)
                    {
                        object target = descriptor2.Target;
                        if (MergeOption.PreserveChanges == this.mergeOption)
                        {
                            LinkDescriptor descriptor3 = this.context.GetLinks(descriptor2.Source, descriptor2.SourceProperty).FirstOrDefault<LinkDescriptor>();
                            if ((descriptor3 != null) && (null == descriptor3.Target))
                            {
                                continue;
                            }
                            if (((target != null) && (EntityStates.Deleted == this.context.GetEntityDescriptor(target).State)) || (EntityStates.Deleted == this.context.GetEntityDescriptor(descriptor2.Source).State))
                            {
                                target = null;
                            }
                        }
                        this.context.AttachLink(descriptor2.Source, descriptor2.SourceProperty, target, this.mergeOption);
                    }
                    else
                    {
                        Debug.Assert(EntityStates.Detached == descriptor2.State, "not detached link");
                        this.context.DetachLink(descriptor2.Source, descriptor2.SourceProperty, descriptor2.Target);
                    }
                }
            }
        }

        internal void Clear()
        {
            this.foundEntriesWithMedia.Clear();
            this.identityStack.Clear();
            this.links.Clear();
            this.insertRefreshObject = null;
        }

        internal void CreatedInstance(AtomEntry entry)
        {
            Debug.Assert(entry != null, "entry != null");
            Debug.Assert(entry.ResolvedObject != null, "entry.ResolvedObject != null -- otherwise, what did we create?");
            Debug.Assert(entry.CreatedByMaterializer, "entry.CreatedByMaterializer -- otherwise we shouldn't be calling this");
            if (ShouldTrackWithContext(entry))
            {
                this.identityStack.Add(entry.Identity, entry);
                if (this.mergeOption == MergeOption.AppendOnly)
                {
                    this.appendOnlyEntries.Add(entry.Identity, entry);
                }
            }
        }

        internal void FoundExistingInstance(AtomEntry entry)
        {
            Debug.Assert(entry != null, "entry != null");
            Debug.Assert(ShouldTrackWithContext(entry), "Existing entries should be entity");
            if (this.mergeOption == MergeOption.OverwriteChanges)
            {
                this.identityStack[entry.Identity] = entry;
            }
            else
            {
                bool? nullable;
                if (this.Tracking && ((nullable = entry.MediaLinkEntry).GetValueOrDefault() && nullable.HasValue))
                {
                    this.foundEntriesWithMedia[entry.Identity] = entry;
                }
            }
        }

        internal void FoundTargetInstance(AtomEntry entry)
        {
            Debug.Assert(entry != null, "entry != null");
            Debug.Assert(entry.ResolvedObject != null, "entry.ResolvedObject != null -- otherwise this is not a target");
            if (ShouldTrackWithContext(entry))
            {
                this.context.AttachIdentity(entry.Identity, entry.QueryLink, entry.EditLink, entry.ResolvedObject, entry.ETagText);
                this.identityStack.Add(entry.Identity, entry);
                this.insertRefreshObject = entry.ResolvedObject;
            }
        }

        internal void RemovedLink(AtomEntry source, string propertyName, object target)
        {
            Debug.Assert(source != null, "source != null");
            Debug.Assert(propertyName != null, "propertyName != null");
            if (ShouldTrackWithContext(source) && ShouldTrackWithContext(target))
            {
                Debug.Assert(this.Tracking, "this.Tracking -- otherwise there's an 'if' missing (it happens to be that the assert holds for all current callers");
                LinkDescriptor item = new LinkDescriptor(source.ResolvedObject, propertyName, target, EntityStates.Detached);
                this.links.Add(item);
            }
        }

        internal void SetLink(AtomEntry source, string propertyName, object target)
        {
            Debug.Assert(source != null, "source != null");
            Debug.Assert(propertyName != null, "propertyName != null");
            if (this.Tracking && (ShouldTrackWithContext(source) && ShouldTrackWithContext(target)))
            {
                Debug.Assert(this.Tracking, "this.Tracking -- otherwise there's an 'if' missing (it happens to be that the assert holds for all current callers");
                LinkDescriptor item = new LinkDescriptor(source.ResolvedObject, propertyName, target, EntityStates.Modified);
                this.links.Add(item);
            }
        }

        private static bool ShouldTrackWithContext(AtomEntry entry)
        {
            Debug.Assert(entry.ActualType != null, "Entry with no type added to log");
            return entry.ActualType.IsEntityType;
        }

        private static bool ShouldTrackWithContext(object entity)
        {
            return ((entity == null) || ClientType.Create(entity.GetType()).IsEntityType);
        }

        internal bool TryResolve(AtomEntry entry, out AtomEntry existingEntry)
        {
            Debug.Assert(entry != null, "entry != null");
            Debug.Assert(entry.Identity != null, "entry.Identity != null");
            if (this.identityStack.TryGetValue(entry.Identity, out existingEntry))
            {
                return true;
            }
            if (this.appendOnlyEntries.TryGetValue(entry.Identity, out existingEntry))
            {
                EntityStates states;
                this.context.TryGetEntity(entry.Identity, entry.ETagText, this.mergeOption, out states);
                if (states == EntityStates.Unchanged)
                {
                    return true;
                }
                this.appendOnlyEntries.Remove(entry.Identity);
            }
            existingEntry = null;
            return false;
        }

        internal bool Tracking
        {
            get
            {
                return (this.mergeOption != MergeOption.NoTracking);
            }
        }
    }
}

