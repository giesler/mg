namespace System.Data.Services.Client
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    internal static class AtomMaterializerInvoker
    {
        internal static object DirectMaterializePlan(object materializer, object entry, Type expectedEntryType)
        {
            Debug.Assert(materializer.GetType() == typeof(AtomMaterializer), "materializer.GetType() == typeof(AtomMaterializer)");
            Debug.Assert(entry.GetType() == typeof(AtomEntry), "entry.GetType() == typeof(AtomEntry)");
            return AtomMaterializer.DirectMaterializePlan((AtomMaterializer) materializer, (AtomEntry) entry, expectedEntryType);
        }

        internal static IEnumerable<T> EnumerateAsElementType<T>(IEnumerable source)
        {
            return AtomMaterializer.EnumerateAsElementType<T>(source);
        }

        internal static List<TTarget> ListAsElementType<T, TTarget>(object materializer, IEnumerable<T> source) where T: TTarget
        {
            Debug.Assert(materializer.GetType() == typeof(AtomMaterializer), "materializer.GetType() == typeof(AtomMaterializer)");
            return AtomMaterializer.ListAsElementType<T, TTarget>((AtomMaterializer) materializer, source);
        }

        internal static bool ProjectionCheckValueForPathIsNull(object entry, Type expectedType, object path)
        {
            Debug.Assert(entry.GetType() == typeof(AtomEntry), "entry.GetType() == typeof(AtomEntry)");
            Debug.Assert(path.GetType() == typeof(ProjectionPath), "path.GetType() == typeof(ProjectionPath)");
            return AtomMaterializer.ProjectionCheckValueForPathIsNull((AtomEntry) entry, expectedType, (ProjectionPath) path);
        }

        internal static AtomEntry ProjectionGetEntry(object entry, string name)
        {
            Debug.Assert(entry.GetType() == typeof(AtomEntry), "entry.GetType() == typeof(AtomEntry)");
            return AtomMaterializer.ProjectionGetEntry((AtomEntry) entry, name);
        }

        internal static object ProjectionInitializeEntity(object materializer, object entry, Type expectedType, Type resultType, string[] properties, Func<object, object, Type, object>[] propertyValues)
        {
            Debug.Assert(materializer.GetType() == typeof(AtomMaterializer), "materializer.GetType() == typeof(AtomMaterializer)");
            Debug.Assert(entry.GetType() == typeof(AtomEntry), "entry.GetType() == typeof(AtomEntry)");
            return AtomMaterializer.ProjectionInitializeEntity((AtomMaterializer) materializer, (AtomEntry) entry, expectedType, resultType, properties, propertyValues);
        }

        internal static IEnumerable ProjectionSelect(object materializer, object entry, Type expectedType, Type resultType, object path, Func<object, object, Type, object> selector)
        {
            Debug.Assert(materializer.GetType() == typeof(AtomMaterializer), "materializer.GetType() == typeof(AtomMaterializer)");
            Debug.Assert(entry.GetType() == typeof(AtomEntry), "entry.GetType() == typeof(AtomEntry)");
            Debug.Assert(path.GetType() == typeof(ProjectionPath), "path.GetType() == typeof(ProjectionPath)");
            return AtomMaterializer.ProjectionSelect((AtomMaterializer) materializer, (AtomEntry) entry, expectedType, resultType, (ProjectionPath) path, selector);
        }

        internal static object ProjectionValueForPath(object materializer, object entry, Type expectedType, object path)
        {
            Debug.Assert(materializer.GetType() == typeof(AtomMaterializer), "materializer.GetType() == typeof(AtomMaterializer)");
            Debug.Assert(entry.GetType() == typeof(AtomEntry), "entry.GetType() == typeof(AtomEntry)");
            Debug.Assert(path.GetType() == typeof(ProjectionPath), "path.GetType() == typeof(ProjectionPath)");
            return AtomMaterializer.ProjectionValueForPath((AtomMaterializer) materializer, (AtomEntry) entry, expectedType, (ProjectionPath) path);
        }

        internal static object ShallowMaterializePlan(object materializer, object entry, Type expectedEntryType)
        {
            Debug.Assert(materializer.GetType() == typeof(AtomMaterializer), "materializer.GetType() == typeof(AtomMaterializer)");
            Debug.Assert(entry.GetType() == typeof(AtomEntry), "entry.GetType() == typeof(AtomEntry)");
            return AtomMaterializer.ShallowMaterializePlan((AtomMaterializer) materializer, (AtomEntry) entry, expectedEntryType);
        }
    }
}

