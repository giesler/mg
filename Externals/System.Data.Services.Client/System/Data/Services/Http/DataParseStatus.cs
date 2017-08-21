namespace System.Data.Services.Http
{
    using System;

    internal enum DataParseStatus
    {
        NeedMoreData,
        ContinueParsing,
        Done,
        Invalid,
        DataTooBig
    }
}

