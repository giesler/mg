using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Services.Client;
using System.Collections.Generic;
using Microsoft.Live;
using System.Reflection;
using System.Linq;

namespace Microsoft.Live
{
    public static class DataContextHelper
    {
        public static DataServiceQuery<T> CreateRelatedQuery<T>(this LiveDataContext context, object entity, ICollection<T> relationshipCollection)
        {
            Type targetType = relationshipCollection.GetType();
            string propertyName = null;
            foreach (PropertyInfo info in entity.GetType().GetProperties().Where<PropertyInfo>(delegate(PropertyInfo pi)
            {
                return targetType == pi.PropertyType;
            }))
            {
                if (info.GetValue(entity, null) == relationshipCollection)
                {
                    propertyName = info.Name;
                    break;
                }
            }
            if (null == propertyName)
            {
                throw new InvalidOperationException("Property is not a relationship on the specified instance");
            }
            return context.CreateRelatedQuery<T>(entity, propertyName);
        }

        public static DataServiceQuery<T> CreateRelatedQuery<T>(this LiveDataContext context, object entity, string propertyName)
        {
            Uri uri;
            EntityDescriptor descriptor = context.EnsureContained(entity, "entity");
            Util.CheckArgumentNotEmpty(propertyName, "propertyName");
            //ClientType type = ClientType.Create(entity.GetType());
            //Debug.Assert(type.IsEntityType, "must be entity type to be contained");
            if (EntityStates.Added == descriptor.State)
            {
                throw Error.InvalidOperation("Context_NoLoadWithInsertEnd");
            }
            //ClientType.ClientProperty property = type.GetProperty(propertyName, false);
            //Debug.Assert(null != property, "should have thrown if propertyName didn't exist");
            //if (null == (uri = context.GetEntityDescriptor(entity).GetRelationshipLink(propertyName)))
            if (null == (uri = entity.GetRelationshipLink(propertyName)))
            {
                // TODO: Need to fix create query
                //return context.CreateQuery<T>(context.GetEntityDescriptor(entity).GetResourceUri(context.BaseUri, true).OriginalString + "/" + propertyName);
            }
            return context.CreateQuery<T>(uri.OriginalString);
        }

        private static Uri GetRelationshipLink(this object entity, string propertyName)
        {
            var attributes = entity.GetType().GetCustomAttributes(typeof(EntityPropertyMappingExAttribute), true);
            foreach (EntityPropertyMappingExAttribute attribute in attributes.OfType<EntityPropertyMappingExAttribute>())
            {
                if (attribute.TargetSyndicationItem == SyndicationItemPropertyEx.LinkHref &&
                    attribute.SourcePath == propertyName &&
                    attribute.Criteria.Key == propertyName)
                    return new Uri(attribute.Criteria.Value);
            }
            return null;
        }

        // TODO: Complete GetResourceUri
        //private Uri GetResourceUri(this object entity, Uri baseUriWithSlash, bool queryLink)
        //{
        //    if (this.parentDescriptor != null)
        //    {
        //        if (this.parentDescriptor.Identity == null)
        //        {
        //            return Util.CreateUri(Util.CreateUri(baseUriWithSlash, new Uri("$" + this.parentDescriptor.ChangeOrder.ToString(CultureInfo.InvariantCulture), UriKind.Relative)), Util.CreateUri(this.parentProperty, UriKind.Relative));
        //        }
        //        if ((this.parentDescriptor.RelationshipLinks != null) && this.parentDescriptor.RelationshipLinks.ContainsKey(this.parentProperty))
        //        {
        //            return this.parentDescriptor.RelationshipLinks[this.parentProperty];
        //        }
        //        return Util.CreateUri(Util.CreateUri(baseUriWithSlash, this.parentDescriptor.GetLink(queryLink)), this.GetLink(queryLink));
        //    }
        //    return Util.CreateUri(baseUriWithSlash, this.GetLink(queryLink));
        //}




        private static EntityDescriptor EnsureContained(this LiveDataContext context, object resource, string parameterName)
        {
            Util.CheckArgumentNull<object>(resource, parameterName);
            EntityDescriptor descriptor = context.GetEntityDescriptor(resource);
            //if (!this.entityDescriptors.TryGetValue(resource, out descriptor))
            //{
            //    throw Error.InvalidOperation("Context_EntityNotContained");
            //}
            if (descriptor == null)
            {
                throw Error.InvalidOperation("Context_EntityNotContained");
            }
            return descriptor;
        }



 

    }


    internal static class Util
    {
        internal static T CheckArgumentNull<T>(T value, string parameterName) where T : class
        {
            if (null == value)
            {
                throw Error.ArgumentNull(parameterName);
            }
            return value;
        }

        internal static void CheckArgumentNotEmpty(string value, string parameterName)
        {
            CheckArgumentNull<string>(value, parameterName);
            if (0 == value.Length)
            {
                throw Error.Argument("Util_EmptyString", parameterName);
            }
        }

 

    }

    internal static class Error
    {
        internal static Exception ArgumentNull(string paramName)
        {
            return new ArgumentNullException(paramName);
        }

        internal static ArgumentException Argument(string message, string parameterName)
        {
            return Trace<ArgumentException>(new ArgumentException(message, parameterName));
        }

        internal static InvalidOperationException InvalidOperation(string message)
        {
            return Trace<InvalidOperationException>(new InvalidOperationException(message));
        }

 

 

        private static T Trace<T>(T exception) where T : Exception
        {
            return exception;
        }
    }
}
