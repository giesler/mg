namespace System.Data.Services.Client
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    internal sealed class BindingGraph
    {
        private Graph graph;
        private BindingObserver observer;

        public BindingGraph(BindingObserver observer)
        {
            this.observer = observer;
            this.graph = new Graph();
        }

        public bool AddCollection(object source, string sourceProperty, object collection, string collectionEntitySet)
        {
            Debug.Assert(collection != null, "'collection' can not be null");
            Debug.Assert(BindingEntityInfo.IsDataServiceCollection(collection.GetType()), "Argument 'collection' must be an DataServiceCollection<T> of entity type T");
            if (this.graph.ExistsVertex(collection))
            {
                return false;
            }
            Vertex vertex = this.graph.AddVertex(collection);
            vertex.IsCollection = true;
            vertex.EntitySet = collectionEntitySet;
            ICollection is2 = collection as ICollection;
            if (source != null)
            {
                vertex.Parent = this.graph.LookupVertex(source);
                vertex.ParentProperty = sourceProperty;
                this.graph.AddEdge(source, collection, sourceProperty);
                Type collectionEntityType = BindingUtils.GetCollectionEntityType(collection.GetType());
                Debug.Assert(collectionEntityType != null, "Collection must at least be inherited from DataServiceCollection<T>");
                if (!typeof(INotifyPropertyChanged).IsAssignableFrom(collectionEntityType))
                {
                    throw new InvalidOperationException(Strings.DataBinding_NotifyPropertyChangedNotImpl(collectionEntityType));
                }
                typeof(BindingGraph).GetMethod("SetObserver", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(new Type[] { collectionEntityType }).Invoke(this, new object[] { is2 });
            }
            else
            {
                this.graph.Root = vertex;
            }
            Debug.Assert((vertex.Parent != null) || vertex.IsRootCollection, "If parent is null, then collectionVertex should be a root collection");
            this.AttachCollectionNotification(collection);
            foreach (object obj2 in is2)
            {
                this.AddEntity(source, sourceProperty, obj2, collectionEntitySet, collection);
            }
            return true;
        }

        public void AddComplexProperty(object source, string sourceProperty, object target)
        {
            Vertex vertex = this.graph.LookupVertex(source);
            Debug.Assert(vertex != null, "Must have a valid parent entity for complex properties.");
            Debug.Assert(target != null, "Must have non-null complex object reference.");
            if (this.graph.LookupVertex(target) != null)
            {
                throw new InvalidOperationException(Strings.DataBinding_ComplexObjectAssociatedWithMultipleEntities(target.GetType()));
            }
            Vertex vertex2 = this.graph.AddVertex(target);
            vertex2.Parent = vertex;
            vertex2.IsComplex = true;
            if (!this.AttachEntityOrComplexObjectNotification(target))
            {
                throw new InvalidOperationException(Strings.DataBinding_NotifyPropertyChangedNotImpl(target.GetType()));
            }
            this.graph.AddEdge(source, target, sourceProperty);
            this.AddFromProperties(target);
        }

        public bool AddEntity(object source, string sourceProperty, object target, string targetEntitySet, object edgeSource)
        {
            Vertex vertex = this.graph.LookupVertex(edgeSource);
            Debug.Assert(vertex != null, "Must have a valid edge source");
            Vertex vertex2 = null;
            bool flag = false;
            if (target != null)
            {
                vertex2 = this.graph.LookupVertex(target);
                if (vertex2 == null)
                {
                    vertex2 = this.graph.AddVertex(target);
                    vertex2.EntitySet = BindingEntityInfo.GetEntitySet(target, targetEntitySet);
                    if (!this.AttachEntityOrComplexObjectNotification(target))
                    {
                        throw new InvalidOperationException(Strings.DataBinding_NotifyPropertyChangedNotImpl(target.GetType()));
                    }
                    flag = true;
                }
                if (this.graph.ExistsEdge(edgeSource, target, vertex.IsCollection ? null : sourceProperty))
                {
                    throw new InvalidOperationException(Strings.DataBinding_EntityAlreadyInCollection(target.GetType()));
                }
                this.graph.AddEdge(edgeSource, target, vertex.IsCollection ? null : sourceProperty);
            }
            if (!vertex.IsCollection)
            {
                this.observer.HandleUpdateEntityReference(source, sourceProperty, vertex.EntitySet, target, (vertex2 == null) ? null : vertex2.EntitySet);
            }
            else
            {
                Debug.Assert(target != null, "Target must be non-null when adding to collections");
                this.observer.HandleAddEntity(source, sourceProperty, (vertex.Parent != null) ? vertex.Parent.EntitySet : null, edgeSource as ICollection, target, vertex2.EntitySet);
            }
            if (flag)
            {
                this.AddFromProperties(target);
            }
            return flag;
        }

        private void AddFromProperties(object entity)
        {
            foreach (BindingEntityInfo.BindingPropertyInfo info in BindingEntityInfo.GetObservableProperties(entity.GetType()))
            {
                object target = info.PropertyInfo.GetValue(entity);
                if (target != null)
                {
                    switch (info.PropertyKind)
                    {
                        case BindingPropertyKind.BindingPropertyKindEntity:
                            this.AddEntity(entity, info.PropertyInfo.PropertyName, target, null, entity);
                            break;

                        case BindingPropertyKind.BindingPropertyKindCollection:
                            this.AddCollection(entity, info.PropertyInfo.PropertyName, target, null);
                            break;

                        default:
                            Debug.Assert(info.PropertyKind == BindingPropertyKind.BindingPropertyKindComplex, "Must be complex type if PropertyKind is not entity or collection.");
                            this.AddComplexProperty(entity, info.PropertyInfo.PropertyName, target);
                            break;
                    }
                }
            }
        }

        private void AttachCollectionNotification(object target)
        {
            Debug.Assert(target != null, "Argument 'target' cannot be null");

            INotifyCollectionChanged notify = target as INotifyCollectionChanged;
            Debug.Assert(notify != null, "DataServiceCollection must implement INotifyCollectionChanged");

            notify.CollectionChanged -= this.observer.OnCollectionChanged;
            notify.CollectionChanged += this.observer.OnCollectionChanged;
        }

        private bool AttachEntityOrComplexObjectNotification(object target)
        {
            Debug.Assert(target != null, "Argument 'target' cannot be null");

            INotifyPropertyChanged notify = target as INotifyPropertyChanged;
            if (notify != null)
            {
                notify.PropertyChanged -= this.observer.OnPropertyChanged;
                notify.PropertyChanged += this.observer.OnPropertyChanged;
                return true;
            }

            return false;
        }

        private void DetachNotifications(object target)
        {
            Debug.Assert(target != null, "Argument 'target' cannot be null");

            this.DetachCollectionNotifications(target);

            INotifyPropertyChanged notifyPropertyChanged = target as INotifyPropertyChanged;
            if (notifyPropertyChanged != null)
            {
                notifyPropertyChanged.PropertyChanged -= this.observer.OnPropertyChanged;
            }
        }

        private void DetachCollectionNotifications(object target)
        {
            Debug.Assert(target != null, "Argument 'target' cannot be null");

            INotifyCollectionChanged notifyCollectionChanged = target as INotifyCollectionChanged;
            if (notifyCollectionChanged != null)
            {
                notifyCollectionChanged.CollectionChanged -= this.observer.OnCollectionChanged;
            }
        }

        public void GetAncestorEntityForComplexProperty(ref object entity, ref string propertyName, ref object propertyValue)
        {
            Vertex childVertex = this.graph.LookupVertex(entity);
            Debug.Assert(childVertex != null, "Must have a vertex in the graph corresponding to the entity.");
            Debug.Assert(childVertex.IsComplex == true, "Vertex must correspond to a complex object.");

            while (childVertex.IsComplex)
            {
                propertyName = childVertex.IncomingEdges[0].Label;
                propertyValue = childVertex.Item;

                Debug.Assert(childVertex.Parent != null, "Complex properties must always have parent vertices.");
                entity = childVertex.Parent.Item;

                childVertex = childVertex.Parent;
            }
        }

        public IEnumerable<object> GetCollectionItems(object collection)
        {
            Vertex collectionVertex = this.graph.LookupVertex(collection);
            Debug.Assert(collectionVertex != null, "Must be tracking the vertex for the collection");
            foreach (Edge collectionEdge in collectionVertex.OutgoingEdges.ToList())
            {
                yield return collectionEdge.Target.Item;
            }
        }

        public void GetEntityCollectionInfo(object collection, out object source, out string sourceProperty, out string sourceEntitySet, out string targetEntitySet)
        {
            Debug.Assert(collection != null, "Argument 'collection' cannot be null.");
            Debug.Assert(this.graph.ExistsVertex(collection), "Vertex corresponding to 'collection' must exist in the graph.");
            this.graph.LookupVertex(collection).GetEntityCollectionInfo(out source, out sourceProperty, out sourceEntitySet, out targetEntitySet);
        }

        public bool IsTracking(object item)
        {
            return this.graph.ExistsVertex(item);
        }

        public void Remove(object item, object parent, string parentProperty)
        {
            Vertex vertexToRemove = this.graph.LookupVertex(item);
            if (vertexToRemove == null)
            {
                return;
            }

            Debug.Assert(!vertexToRemove.IsRootCollection, "Root collections are never removed");

            Debug.Assert(parent != null, "Parent has to be present.");

            if (parentProperty != null)
            {
                BindingEntityInfo.BindingPropertyInfo bpi = BindingEntityInfo.GetObservableProperties(parent.GetType())
                                                                             .Single(p => p.PropertyInfo.PropertyName == parentProperty);
                Debug.Assert(bpi.PropertyKind == BindingPropertyKind.BindingPropertyKindCollection, "parentProperty must refer to an DataServiceCollection");

                parent = bpi.PropertyInfo.GetValue(parent);
            }

            object source = null;
            string sourceProperty = null;
            string sourceEntitySet = null;
            string targetEntitySet = null;

            this.GetEntityCollectionInfo(
                    parent,
                    out source,
                    out sourceProperty,
                    out sourceEntitySet,
                    out targetEntitySet);

            targetEntitySet = BindingEntityInfo.GetEntitySet(item, targetEntitySet);

            this.observer.HandleDeleteEntity(
                            source,
                            sourceProperty,
                            sourceEntitySet,
                            parent as ICollection,
                            item,
                            targetEntitySet);

            this.graph.RemoveEdge(parent, item, null);
        }

        public void RemoveCollection(object collection)
        {
            Vertex vertex = this.graph.LookupVertex(collection);
            Debug.Assert(vertex != null, "Must be tracking the vertex for the collection");
            foreach (Edge edge in vertex.OutgoingEdges.ToList<Edge>())
            {
                this.graph.RemoveEdge(collection, edge.Target.Item, null);
            }
            this.RemoveUnreachableVertices();
        }

        public void RemoveNonTrackedEntities()
        {
            foreach (var entity in this.graph.Select(o => BindingEntityInfo.IsEntityType(o.GetType()) && !this.observer.IsContextTrackingEntity(o)))
            {
                this.graph.ClearEdgesForVertex(this.graph.LookupVertex(entity));
            }
            
            this.RemoveUnreachableVertices();
        }

        public void RemoveRelation(object source, string relation)
        {
            
           Edge edge = this.graph
                            .LookupVertex(source)
                            .OutgoingEdges
                            .SingleOrDefault(e => e.Source.Item == source && e.Label == relation);
            if (edge != null)
            {
                this.graph.RemoveEdge(edge.Source.Item, edge.Target.Item, edge.Label);
            }

            this.RemoveUnreachableVertices();
        }

        public void RemoveUnreachableVertices()
        {
            this.graph.RemoveUnreachableVertices(new Action<object>(this.DetachNotifications));
        }

        public void Reset()
        {
            this.graph.Reset(new Action<object>(this.DetachNotifications));
        }

        private void SetObserver<T>(ICollection collection)
        {
            DataServiceCollection<T> services = collection as DataServiceCollection<T>;
            services.Observer = this.observer;
        }

       

        internal sealed class Edge : IEquatable<BindingGraph.Edge>
        {
            public string Label { get; set; }
            public Vertex Source { get; set; }
            public Vertex Target { get; set; }
            //[CompilerGenerated]
            //private string <Label>k__BackingField;
            //[CompilerGenerated]
            //private BindingGraph.Vertex <Source>k__BackingField;
            //[CompilerGenerated]
            //private BindingGraph.Vertex <Target>k__BackingField;

            public bool Equals(BindingGraph.Edge other)
            {
                return ((((other != null) && object.ReferenceEquals(this.Source, other.Source)) && object.ReferenceEquals(this.Target, other.Target)) && (this.Label == other.Label));
            }

            //public string Label
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<Label>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    set
            //    {
            //        this.<Label>k__BackingField = value;
            //    }
            //}

            //public BindingGraph.Vertex Source
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<Source>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    set
            //    {
            //        this.<Source>k__BackingField = value;
            //    }
            //}

            //public BindingGraph.Vertex Target
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<Target>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    set
            //    {
            //        this.<Target>k__BackingField = value;
            //    }
            //}
        }

        internal sealed class Graph
        {
            private BindingGraph.Vertex root;
            private Dictionary<object, BindingGraph.Vertex> vertices = new Dictionary<object, BindingGraph.Vertex>(ReferenceEqualityComparer<object>.Instance);

            public BindingGraph.Edge AddEdge(object source, object target, string label)
            {
                BindingGraph.Vertex vertex = this.vertices[source];
                BindingGraph.Vertex vertex2 = this.vertices[target];
                BindingGraph.Edge edge2 = new BindingGraph.Edge();
                edge2.Source = vertex;
                edge2.Target = vertex2;
                edge2.Label = label;
                BindingGraph.Edge item = edge2;
                vertex.OutgoingEdges.Add(item);
                vertex2.IncomingEdges.Add(item);
                return item;
            }

            public BindingGraph.Vertex AddVertex(object item)
            {
                BindingGraph.Vertex vertex = new BindingGraph.Vertex(item);
                this.vertices.Add(item, vertex);
                return vertex;
            }

            public void ClearEdgesForVertex(BindingGraph.Vertex v)
            {
                foreach (BindingGraph.Edge edge in v.OutgoingEdges.Concat<BindingGraph.Edge>(v.IncomingEdges).ToList<BindingGraph.Edge>())
                {
                    this.RemoveEdge(edge.Source.Item, edge.Target.Item, edge.Label);
                }
            }

            public bool ExistsEdge(object source, object target, string label)
            {
                Edge e = new Edge { Source = this.vertices[source], Target = this.vertices[target], Label = label };
                return this.vertices[source].OutgoingEdges.Any(r => r.Equals(e));
            }

            public bool ExistsVertex(object item)
            {
                BindingGraph.Vertex vertex;
                return this.vertices.TryGetValue(item, out vertex);
            }

            public BindingGraph.Vertex LookupVertex(object item)
            {
                BindingGraph.Vertex vertex;
                this.vertices.TryGetValue(item, out vertex);
                return vertex;
            }

            public void RemoveEdge(object source, object target, string label)
            {
                BindingGraph.Vertex vertex = this.vertices[source];
                BindingGraph.Vertex vertex2 = this.vertices[target];
                BindingGraph.Edge edge2 = new BindingGraph.Edge();
                edge2.Source = vertex;
                edge2.Target = vertex2;
                edge2.Label = label;
                BindingGraph.Edge item = edge2;
                vertex.OutgoingEdges.Remove(item);
                vertex2.IncomingEdges.Remove(item);
            }

            public void RemoveUnreachableVertices(Action<object> detachAction)
            {
                try
                {
                    foreach (BindingGraph.Vertex vertex in this.UnreachableVertices())
                    {
                        this.ClearEdgesForVertex(vertex);
                        detachAction(vertex.Item);
                        this.vertices.Remove(vertex.Item);
                    }
                }
                finally
                {
                    foreach (BindingGraph.Vertex vertex in this.vertices.Values)
                    {
                        vertex.Color = VertexColor.White;
                    }
                }
            }

            public void Reset(Action<object> action)
            {
                foreach (object obj2 in this.vertices.Keys)
                {
                    action(obj2);
                }
                this.vertices.Clear();
            }

            public IList<object> Select(Func<object, bool> filter)
            {
                return Enumerable.Where<object>(this.vertices.Keys, filter).ToList<object>();
            }

            private IEnumerable<BindingGraph.Vertex> UnreachableVertices()
            {
                Queue<Vertex> q = new Queue<Vertex>();
                
                this.Root.Color = VertexColor.Gray;
                q.Enqueue(this.Root);
                
                while (q.Count != 0)
                {
                    Vertex current = q.Dequeue();
                    
                    foreach (Edge e in current.OutgoingEdges)
                    {
                        if (e.Target.Color == VertexColor.White)
                        {
                            e.Target.Color = VertexColor.Gray;
                            q.Enqueue(e.Target);
                        }
                    }
                    
                    current.Color = VertexColor.Black;
                }
                
                return this.vertices.Values.Where(v => v.Color == VertexColor.White).ToList();
            }

            public BindingGraph.Vertex Root
            {
                get
                {
                    Debug.Assert(this.root != null, "Must have a non-null root vertex when this call is made.");
                    return this.root;
                }
                set
                {
                    Debug.Assert(this.root == null, "Must only initialize root vertex once.");
                    Debug.Assert(this.ExistsVertex(value.Item), "Must already have the assigned vertex in the graph.");
                    this.root = value;
                }
            }
        }

        internal sealed class Vertex
        {
            public VertexColor Color { get; set; }
            public string EntitySet { get; set; }
            public bool IsCollection { get; set; }
            public bool IsComplex { get; set; }
            public object Item { get; set; }
            public Vertex Parent { get; set; }
            public string ParentProperty { get; set; }
            //[CompilerGenerated]
            //private VertexColor <Color>k__BackingField;
            //[CompilerGenerated]
            //private string <EntitySet>k__BackingField;
            //[CompilerGenerated]
            //private bool <IsCollection>k__BackingField;
            //[CompilerGenerated]
            //private bool <IsComplex>k__BackingField;
            //[CompilerGenerated]
            //private object <Item>k__BackingField;
            //[CompilerGenerated]
            //private BindingGraph.Vertex <Parent>k__BackingField;
            //[CompilerGenerated]
            //private string <ParentProperty>k__BackingField;
            private List<BindingGraph.Edge> incomingEdges;
            private List<BindingGraph.Edge> outgoingEdges;

            public Vertex(object item)
            {
                Debug.Assert(item != null, "item must be non-null");
                this.Item = item;
                this.Color = VertexColor.White;
            }

            public void GetEntityCollectionInfo(out object source, out string sourceProperty, out string sourceEntitySet, out string targetEntitySet)
            {
                Debug.Assert(this.IsCollection, "Must be a collection to be in this method");
                if (!this.IsRootCollection)
                {
                    Debug.Assert(this.Parent != null, "Parent must be non-null for child collection");
                    source = this.Parent.Item;
                    Debug.Assert(source != null, "Source object must be present for child collection");
                    sourceProperty = this.ParentProperty;
                    Debug.Assert(sourceProperty != null, "Source entity property associated with a child collection must be non-null");
                    Debug.Assert(source.GetType().GetProperty(sourceProperty) != null, "Unable to get information for the source entity property associated with a child collection");
                    sourceEntitySet = this.Parent.EntitySet;
                }
                else
                {
                    Debug.Assert(this.Parent == null, "Parent must be null for top level collection");
                    source = null;
                    sourceProperty = null;
                    sourceEntitySet = null;
                }
                targetEntitySet = this.EntitySet;
            }

            //public VertexColor Color
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<Color>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    set
            //    {
            //        this.<Color>k__BackingField = value;
            //    }
            //}

            //public string EntitySet
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<EntitySet>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    set
            //    {
            //        this.<EntitySet>k__BackingField = value;
            //    }
            //}

            public IList<BindingGraph.Edge> IncomingEdges
            {
                get
                {
                    if (this.incomingEdges == null)
                    {
                        this.incomingEdges = new List<BindingGraph.Edge>();
                    }
                    return this.incomingEdges;
                }
            }

            //public bool IsCollection
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<IsCollection>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    set
            //    {
            //        this.<IsCollection>k__BackingField = value;
            //    }
            //}

            //public bool IsComplex
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<IsComplex>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    set
            //    {
            //        this.<IsComplex>k__BackingField = value;
            //    }
            //}

            public bool IsRootCollection
            {
                get
                {
                    return (this.IsCollection && (this.Parent == null));
                }
            }

            //public object Item
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<Item>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    private set
            //    {
            //        this.<Item>k__BackingField = value;
            //    }
            //}

            public IList<BindingGraph.Edge> OutgoingEdges
            {
                get
                {
                    if (this.outgoingEdges == null)
                    {
                        this.outgoingEdges = new List<BindingGraph.Edge>();
                    }
                    return this.outgoingEdges;
                }
            }

            //public BindingGraph.Vertex Parent
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<Parent>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    set
            //    {
            //        this.<Parent>k__BackingField = value;
            //    }
            //}

            //public string ParentProperty
            //{
            //    [CompilerGenerated]
            //    get
            //    {
            //        return this.<ParentProperty>k__BackingField;
            //    }
            //    [CompilerGenerated]
            //    set
            //    {
            //        this.<ParentProperty>k__BackingField = value;
            //    }
            //}
        }
    }
}

