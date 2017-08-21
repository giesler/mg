namespace System.Data.Services.Http
{
    using System;
    using System.Data.Services.Client;
    using System.Diagnostics;

    internal static class ValidationHelper
    {
        private static readonly char[] HttpTrimCharacters = new char[] { '\t', '\n', '\v', '\f', '\r', ' ' };
        private static readonly char[] InvalidParamChars = new char[] { 
            '(', ')', '<', '>', '@', ',', ';', ':', '\\', '"', '\'', '/', '[', ']', '?', '=', 
            '{', '}', ' ', '\t', '\r', '\n'
         };

        internal static string CheckBadChars(string name, bool isHeaderValue)
        {
            if (string.IsNullOrEmpty(name))
            {
                if (!isHeaderValue)
                {
                    if (name == null)
                    {
                        throw new ArgumentNullException("name");
                    }
                    throw new InvalidOperationException(Strings.HttpWeb_InternalArgument("ValidationHelper.CheckBadChars.7", name));
                }
                return string.Empty;
            }
            if (isHeaderValue)
            {
                name = name.Trim(HttpTrimCharacters);
                int num = 0;
                for (int i = 0; i < name.Length; i++)
                {
                    char ch = (char) ('\x00ff' & name[i]);
                    switch (num)
                    {
                        case 0:
                            if (ch == '\r')
                            {
                                break;
                            }
                            goto Label_0100;

                        case 1:
                            if (ch != '\n')
                            {
                                throw new InvalidOperationException(Strings.HttpWeb_InternalArgument("ValidationHelper.CheckBadChars", name));
                            }
                            goto Label_00CF;

                        case 2:
                            if ((ch != ' ') && (ch != '\t'))
                            {
                                throw new InvalidOperationException(Strings.HttpWeb_InternalArgument("ValidationHelper.CheckBadChars.2", name));
                            }
                            goto Label_00F9;

                        default:
                        {
                            continue;
                        }
                    }
                    num = 1;
                    continue;
                Label_00CF:
                    num = 2;
                    continue;
                Label_00F9:
                    num = 0;
                    continue;
                Label_0100:
                    if (ch == '\n')
                    {
                        num = 2;
                    }
                    else if ((ch == '\x007f') || ((ch < ' ') && (ch != '\t')))
                    {
                        throw new InvalidOperationException(Strings.HttpWeb_InternalArgument("ValidationHelper.CheckBadChars.3", name));
                    }
                }
                if (num != 0)
                {
                    throw new InvalidOperationException(Strings.HttpWeb_InternalArgument("ValidationHelper.CheckBadChars.4", name));
                }
                return name;
            }
            if (name.IndexOfAny(InvalidParamChars) != -1)
            {
                throw new InvalidOperationException(Strings.HttpWeb_InternalArgument("ValidationHelper.CheckBadChars.5", name));
            }
            if (ContainsNonAsciiChars(name))
            {
                throw new InvalidOperationException(Strings.HttpWeb_InternalArgument("ValidationHelper.CheckBadChars.6", name));
            }
            return name;
        }

        private static bool ContainsNonAsciiChars(string token)
        {
            Debug.Assert(token != null, "token != null");
            for (int i = 0; i < token.Length; i++)
            {
                if ((token[i] < ' ') || (token[i] > '~'))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

