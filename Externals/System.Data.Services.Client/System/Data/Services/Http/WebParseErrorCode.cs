namespace System.Data.Services.Http
{
    using System;

    internal enum WebParseErrorCode
    {
        Generic,
        InvalidHeaderName,
        InvalidContentLength,
        IncompleteHeaderLine,
        CrLfError,
        InvalidChunkFormat,
        UnexpectedServerResponse
    }
}

