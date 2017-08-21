namespace Microsoft.Live
{
    using System;
    using System.Data.Services.Client;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Xml;
    using System.Xml.Linq;

    public static class ExceptionExtension
    {
        private const string CodeElement = "code";
        private const string ErrorElement = "error";
        private const string MessageElement = "message";

        public static int GetErrorCode(this DataServiceClientException dcExp)
        {
            return InternalGetErrorCode(dcExp, dcExp.StatusCode);
        }

        public static int GetErrorCode(this DataServiceQueryException dqExp)
        {
            return InternalGetErrorCode(dqExp, 0);
        }

        public static int GetErrorCode(this DataServiceRequestException drExp)
        {
            return InternalGetErrorCode(drExp, 0);
        }

        private static bool GetErrorCodeFromInnerException(Exception exp, out int code)
        {
            code = 0;
            bool flag = false;
            try
            {
                StringReader input = new StringReader(exp.Message);
                XmlReader reader = XmlReader.Create(input);
                reader.Read();
                XElement element = XElement.Load(reader);
                if ((element.Name.LocalName == "error") && (element.Name.NamespaceName == "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata"))
                {
                    XElement element2 = element.Element(XName.Get("code", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata"));
                    if ((element2 != null) && int.TryParse(element2.Value, out code))
                    {
                        flag = true;
                    }
                }
            }
            catch
            {
            }
            return flag;
        }

        public static string GetErrorMessage(this DataServiceClientException dcExp)
        {
            return InternalGetErrorMessage(dcExp);
        }

        public static string GetErrorMessage(this DataServiceQueryException dqExp)
        {
            return InternalGetErrorMessage(dqExp);
        }

        public static string GetErrorMessage(this DataServiceRequestException drExp)
        {
            return InternalGetErrorMessage(drExp);
        }

        private static bool GetErrorMessageFromInnerException(Exception exp, out string message)
        {
            message = null;
            bool flag = false;
            try
            {
                StringReader input = new StringReader(exp.Message);
                XmlReader reader = XmlReader.Create(input);
                reader.Read();
                XElement element = XElement.Load(reader);
                if ((element.Name.LocalName == "error") && (element.Name.NamespaceName == "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata"))
                {
                    XElement element2 = element.Element(XName.Get("message", "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata"));
                    if (element2 != null)
                    {
                        message = element2.Value;
                        flag = true;
                    }
                }
            }
            catch
            {
            }
            return flag;
        }

        public static HttpStatusCode GetStatusCode(this DataServiceClientException dcExp)
        {
            return (HttpStatusCode) dcExp.StatusCode;
        }

        public static HttpStatusCode GetStatusCode(this DataServiceQueryException dqExp)
        {
            HttpStatusCode statusCode = (HttpStatusCode) 0;
            if (dqExp.Response != null)
            {
                statusCode = (HttpStatusCode) dqExp.Response.StatusCode;
            }
            return statusCode;
        }

        public static HttpStatusCode GetStatusCode(this DataServiceRequestException drExp)
        {
            HttpStatusCode statusCode = (HttpStatusCode) 0;
            if (drExp.Response != null)
            {
                OperationResponse response = drExp.Response.FirstOrDefault<OperationResponse>();
                if (response != null)
                {
                    statusCode = (HttpStatusCode) response.StatusCode;
                }
            }
            return statusCode;
        }

        private static int InternalGetErrorCode(Exception exp, int statusCode)
        {
            int num2;
            int num = statusCode;
            if ((exp.InnerException != null) && GetErrorCodeFromInnerException(exp.InnerException, out num2))
            {
                num = num2;
            }
            return num;
        }

        private static string InternalGetErrorMessage(Exception exp)
        {
            string str2;
            string message = exp.Message;
            if ((exp.InnerException != null) && GetErrorMessageFromInnerException(exp.InnerException, out str2))
            {
                message = str2;
            }
            return message;
        }
    }
}

