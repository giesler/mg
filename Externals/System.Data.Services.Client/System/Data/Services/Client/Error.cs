namespace System.Data.Services.Client
{
    using System;
    using System.Linq.Expressions;

    internal static class Error
    {
        internal static ArgumentException Argument(string message, string parameterName)
        {
            return Trace<ArgumentException>(new ArgumentException(message, parameterName));
        }

        internal static Exception ArgumentNull(string paramName)
        {
            return new ArgumentNullException(paramName);
        }

        internal static Exception ArgumentOutOfRange(string paramName)
        {
            return new ArgumentOutOfRangeException(paramName);
        }

        internal static InvalidOperationException BatchStreamContentExpected(BatchStreamState state)
        {
            return InvalidOperation(Strings.BatchStream_ContentExpected(state.ToString()));
        }

        internal static InvalidOperationException BatchStreamContentUnexpected(BatchStreamState state)
        {
            return InvalidOperation(Strings.BatchStream_ContentUnexpected(state.ToString()));
        }

        internal static InvalidOperationException BatchStreamGetMethodNotSupportInChangeset()
        {
            return InvalidOperation(Strings.BatchStream_GetMethodNotSupportedInChangeset);
        }

        internal static InvalidOperationException BatchStreamInternalBufferRequestTooSmall()
        {
            return InvalidOperation(Strings.BatchStream_InternalBufferRequestTooSmall);
        }

        internal static InvalidOperationException BatchStreamInvalidBatchFormat()
        {
            return InvalidOperation(Strings.BatchStream_InvalidBatchFormat);
        }

        internal static InvalidOperationException BatchStreamInvalidContentLengthSpecified(string contentLength)
        {
            return InvalidOperation(Strings.BatchStream_InvalidContentLengthSpecified(contentLength));
        }

        internal static InvalidOperationException BatchStreamInvalidContentTypeSpecified(string headerName, string headerValue, string mime1, string mime2)
        {
            return InvalidOperation(Strings.BatchStream_InvalidContentTypeSpecified(headerName, headerValue, mime1, mime2));
        }

        internal static InvalidOperationException BatchStreamInvalidDelimiter(string delimiter)
        {
            return InvalidOperation(Strings.BatchStream_InvalidDelimiter(delimiter));
        }

        internal static InvalidOperationException BatchStreamInvalidHeaderValueSpecified(string headerValue)
        {
            return InvalidOperation(Strings.BatchStream_InvalidHeaderValueSpecified(headerValue));
        }

        internal static InvalidOperationException BatchStreamInvalidHttpMethodName(string methodName)
        {
            return InvalidOperation(Strings.BatchStream_InvalidHttpMethodName(methodName));
        }

        internal static InvalidOperationException BatchStreamInvalidHttpVersionSpecified(string actualVersion, string expectedVersion)
        {
            return InvalidOperation(Strings.BatchStream_InvalidHttpVersionSpecified(actualVersion, expectedVersion));
        }

        internal static InvalidOperationException BatchStreamInvalidMethodHeaderSpecified(string header)
        {
            return InvalidOperation(Strings.BatchStream_InvalidMethodHeaderSpecified(header));
        }

        internal static InvalidOperationException BatchStreamInvalidNumberOfHeadersAtChangeSetStart(string header1, string header2)
        {
            return InvalidOperation(Strings.BatchStream_InvalidNumberOfHeadersAtChangeSetStart(header1, header2));
        }

        internal static InvalidOperationException BatchStreamInvalidNumberOfHeadersAtOperationStart(string header1, string header2)
        {
            return InvalidOperation(Strings.BatchStream_InvalidNumberOfHeadersAtOperationStart(header1, header2));
        }

        internal static InvalidOperationException BatchStreamInvalidOperationHeaderSpecified()
        {
            return InvalidOperation(Strings.BatchStream_InvalidOperationHeaderSpecified);
        }

        internal static InvalidOperationException BatchStreamMissingBoundary()
        {
            return InvalidOperation(Strings.BatchStream_MissingBoundary);
        }

        internal static InvalidOperationException BatchStreamMissingContentTypeHeader(string headerName)
        {
            return InvalidOperation(Strings.BatchStream_MissingContentTypeHeader(headerName));
        }

        internal static InvalidOperationException BatchStreamMissingEndChangesetDelimiter()
        {
            return InvalidOperation(Strings.BatchStream_MissingEndChangesetDelimiter);
        }

        internal static InvalidOperationException BatchStreamMissingOrInvalidContentEncodingHeader(string headerName, string headerValue)
        {
            return InvalidOperation(Strings.BatchStream_MissingOrInvalidContentEncodingHeader(headerName, headerValue));
        }

        internal static InvalidOperationException BatchStreamMoreDataAfterEndOfBatch()
        {
            return InvalidOperation(Strings.BatchStream_MoreDataAfterEndOfBatch);
        }

        internal static InvalidOperationException BatchStreamOnlyGETOperationsCanBeSpecifiedInBatch()
        {
            return InvalidOperation(Strings.BatchStream_OnlyGETOperationsCanBeSpecifiedInBatch);
        }

        internal static InvalidOperationException HttpHeaderFailure(int errorCode, string message)
        {
            return Trace<InvalidOperationException>(new InvalidOperationException(message));
        }

        internal static InvalidOperationException InternalError(System.Data.Services.Client.InternalError value)
        {
            return InvalidOperation(Strings.Context_InternalError((int) value));
        }

        internal static InvalidOperationException InvalidOperation(string message)
        {
            return Trace<InvalidOperationException>(new InvalidOperationException(message));
        }

        internal static InvalidOperationException InvalidOperation(string message, Exception innerException)
        {
            return Trace<InvalidOperationException>(new InvalidOperationException(message, innerException));
        }

        internal static NotSupportedException MethodNotSupported(MethodCallExpression m)
        {
            return NotSupported(Strings.ALinq_MethodNotSupported(m.Method.Name));
        }

        internal static Exception NotImplemented()
        {
            return new NotImplementedException();
        }

        internal static Exception NotSupported()
        {
            return new NotSupportedException();
        }

        internal static NotSupportedException NotSupported(string message)
        {
            return Trace<NotSupportedException>(new NotSupportedException(message));
        }

        internal static void ThrowBatchExpectedResponse(System.Data.Services.Client.InternalError value)
        {
            throw InvalidOperation(Strings.Batch_ExpectedResponse((int) value));
        }

        internal static void ThrowBatchUnexpectedContent(System.Data.Services.Client.InternalError value)
        {
            throw InvalidOperation(Strings.Batch_UnexpectedContent((int) value));
        }

        internal static void ThrowInternalError(System.Data.Services.Client.InternalError value)
        {
            throw InternalError(value);
        }

        internal static void ThrowObjectDisposed(Type type)
        {
            throw Trace<ObjectDisposedException>(new ObjectDisposedException(type.ToString()));
        }

        private static T Trace<T>(T exception) where T: Exception
        {
            return exception;
        }
    }
}

