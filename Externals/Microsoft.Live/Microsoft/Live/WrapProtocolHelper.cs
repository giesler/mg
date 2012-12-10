namespace Microsoft.Live
{
    using System;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Net;
    //using System.Windows.Browser;

    internal static class WrapProtocolHelper
    {
        private static string BuildAccessTokenRequestBody(AppInformation appInfo)
        {
            string format = "wrap_client_id={0}&wrap_client_secret={1}&wrap_callback={2}&wrap_verification_code={3}";
            string callbackUrl = appInfo.CallbackUrl;
            if (!string.IsNullOrEmpty(appInfo.SessionId))
            {
                Uri uri = new Uri(callbackUrl);
                callbackUrl = uri.GetComponents(UriComponents.Path | UriComponents.SchemeAndServer | UriComponents.UserInfo, UriFormat.Unescaped).TrimEnd(new char[] { '/' });
                if (string.IsNullOrEmpty(uri.Query))
                {
                    callbackUrl = new Uri(callbackUrl + "?wl_session_id=" + appInfo.SessionId).OriginalString;
                }
                else
                {
                    callbackUrl = new Uri(callbackUrl + "?" + uri.Query + "&wl_session_id=" + appInfo.SessionId).OriginalString;
                }
            }
            return string.Format(CultureInfo.InvariantCulture, format, new object[] { HttpUtility.UrlEncode(appInfo.ClientId), HttpUtility.UrlEncode(appInfo.ClientSecret), HttpUtility.UrlEncode(callbackUrl), appInfo.AuthInfo.ClientVerifier });
        }

        internal static string BuildConsentUrl(AppInformation appInfo)
        {
            string format = "{0}?wrap_client_id={1}&wrap_callback={2}&wrap_client_state={3}&wrap_scope={4}&mkt=&vmode=auto&pl={5}&tou={6}";
            if (appInfo.RequestedOffers.Count == 0)
            {
                throw new InvalidOperationException("Atleast one offer should be requested");
            }
            string str2 = "";
            int num = 0;
            foreach (Offer offer in appInfo.RequestedOffers)
            {
                num++;
                str2 = str2 + offer.ToString();
                if (num < appInfo.RequestedOffers.Count)
                {
                    str2 = str2 + ",";
                }
            }
            return string.Format(CultureInfo.InvariantCulture, format, new object[] { appInfo.ConsentUrl, appInfo.ClientId, appInfo.CallbackUrl, appInfo.ClientState, str2, appInfo.PolicyUrl, appInfo.TouUrl });
        }

        private static string BuildRefreshTokenRequestBody(AppInformation appInfo)
        {
            string format = "wrap_refresh_token={0}&wrap_client_id={1}&wrap_client_secret={2}";
            return string.Format(CultureInfo.InvariantCulture, format, new object[] { appInfo.AuthInfo.RefreshToken, appInfo.ClientId, appInfo.ClientSecret });
        }

        internal static bool GetVerifierFromTitle(string titleValue, AppInformation appInfo)
        {
            int index = titleValue.IndexOf("code=");
            if (index == -1)
            {
                return false;
            }
            string strA = titleValue.Substring(index + "code=".Length).Trim();
            if (string.Compare(strA, "user_denied", StringComparison.OrdinalIgnoreCase) == 0)
            {
                appInfo.AuthInfo.Error = new AuthorizationException(StringResource.FailToAuthenticate);
                return false;
            }
            string[] strArray = strA.Split(AuthConstants.Wrap.VerifierTitleSeparator2);
            appInfo.AuthInfo.ClientVerifier = strArray[0];
            appInfo.AuthInfo.Scope = strArray[1].Substring("exp=".Length);
            appInfo.AuthInfo.GrantedOffers = ParseOffers(appInfo.AuthInfo.Scope);
            return true;
        }

        internal static bool ParseAccessToken(string token, AppAuthentication authInfo, DateTime requestTime)
        {
            string[] strArray = token.Split(AuthConstants.Wrap.ParameterSeparator);
            if ((strArray != null) && (strArray.Length > 0))
            {
                foreach (string str in strArray)
                {
                    if (str.StartsWith("wrap_error_reason=", StringComparison.Ordinal))
                    {
                        authInfo.Error = new AuthorizationException(HttpUtility.UrlDecode(str.Substring("wrap_error_reason=".Length)));
                        break;
                    }
                    if (str.StartsWith("wrap_access_token=", StringComparison.Ordinal))
                    {
                        authInfo.AccessToken = HttpUtility.UrlDecode(str.Substring("wrap_access_token=".Length));
                        if (!string.IsNullOrEmpty(authInfo.AccessToken))
                        {
                            goto Label_017C;
                        }
                        authInfo.Error = new AuthorizationException(StringResource.NoAccessTokenReturned);
                        break;
                    }
                    if (str.StartsWith("wrap_refresh_token=", StringComparison.Ordinal))
                    {
                        authInfo.RefreshToken = str.Substring("wrap_refresh_token=".Length);
                        if (!string.IsNullOrEmpty(authInfo.RefreshToken))
                        {
                            goto Label_017C;
                        }
                        authInfo.Error = new AuthorizationException(StringResource.NoRefreshTokenReturned);
                        break;
                    }
                    if (str.StartsWith("wrap_access_token_expires_in=", StringComparison.Ordinal))
                    {
                        long num;
                        authInfo.Expiry = string.Empty;
                        if (long.TryParse(str.Substring("wrap_access_token_expires_in=".Length), out num))
                        {
                            TimeSpan span = (TimeSpan) (requestTime.AddSeconds((double) num) - AuthConstants.Wrap.BaseDateTime);
                            authInfo.Expiry = ((long) span.TotalSeconds).ToString();
                        }
                    }
                    else if (str.StartsWith("uid=", StringComparison.Ordinal))
                    {
                        authInfo.UserId = str.Substring("uid=".Length);
                    }
                Label_017C:;
                }
            }
            return (!string.IsNullOrEmpty(authInfo.AccessToken) && !string.IsNullOrEmpty(authInfo.RefreshToken));
        }

        internal static bool ParseConsentResponseHTML(string responseHtml, AppInformation appInfo)
        {
            bool verifierFromTitle = false;
            int startIndex = responseHtml.IndexOf("<title>", 0, StringComparison.InvariantCultureIgnoreCase) + "<title>".Length;
            int num2 = responseHtml.IndexOf("</title>", 0, StringComparison.InvariantCultureIgnoreCase);
            if ((startIndex != -1) && (num2 != -1))
            {
                verifierFromTitle = GetVerifierFromTitle(responseHtml.Substring(startIndex, num2 - startIndex).Trim(), appInfo);
            }
            return verifierFromTitle;
        }

        internal static string ParseErrorResponseBody(string body)
        {
            string str = string.Empty;
            string str2 = string.Empty;
            string str3 = string.Empty;
            string str4 = string.Empty;
            string[] strArray = body.Split(new char[] { '&' });
            if (strArray.Length > 0)
            {
                foreach (string str5 in strArray)
                {
                    if (str5.StartsWith("error_code="))
                    {
                        str2 = HttpUtility.UrlDecode(str5.Substring("error_code=".Length));
                    }
                    else if (str5.StartsWith("wrap_error_reason="))
                    {
                        str3 = HttpUtility.UrlDecode(str5.Substring("wrap_error_reason=".Length));
                    }
                }
            }
            if (string.IsNullOrEmpty(str2) && string.IsNullOrEmpty(str3))
            {
                return str;
            }
            return (str2 + " " + str3 + " " + str4);
        }

        internal static Collection<Offer> ParseOffers(string grantedOffers)
        {
            Collection<Offer> collection = new Collection<Offer>();
            if (!string.IsNullOrEmpty(grantedOffers))
            {
                foreach (string str in grantedOffers.Split(new char[] { AuthConstants.Wrap.OfferSeparator[0] }))
                {
                    collection.Add(new Offer(str));
                }
            }
            return collection;
        }

        private static bool ProcessAccessTokenResponse(HttpWebRequest request, HttpWebResponse response, AppAuthentication authInfo, DateTime requestTime)
        {
            if (response != null)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return ParseAccessToken(reader.ReadToEnd(), authInfo, requestTime);
                }
            }
            authInfo.Error = new AuthorizationException(string.Format(CultureInfo.CurrentCulture, StringResource.NullResponseReceived, new object[] { request.RequestUri.ToString() }));
            return false;
        }

        internal static Exception ProcessGetTokenWebException(WebException webExp)
        {
            string message = webExp.Message;
            HttpWebResponse response = webExp.Response as HttpWebResponse;
            if (response != null)
            {
                Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        string str2 = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(str2))
                        {
                            message = ParseErrorResponseBody(str2);
                        }
                    }
                }
            }
            return new AuthorizationException(message, webExp);
        }

        internal static void RefreshAccessTokenAsync(AppInformation appInfo, Action<bool> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            if (appInfo == null)
            {
                throw new ArgumentNullException("appInfo");
            }
            if (string.IsNullOrEmpty(appInfo.AuthInfo.RefreshToken))
            {
                throw new ArgumentException(StringResource.RefreshTokenInvalid, "appInfo.AuthInfo.RefreshToken");
            }
            appInfo.AuthInfo.AccessToken = null;
            appInfo.AuthInfo.Expiry = null;
            Uri requestUri = new Uri(appInfo.ConsentRoot, "RefreshToken.aspx");
            HttpWebRequest request = WebRequest.Create(requestUri) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.BeginGetRequestStream(delegate (IAsyncResult result) {
                if (result.IsCompleted)
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(request.EndGetRequestStream(result)))
                        {
                            writer.Write(BuildRefreshTokenRequestBody(appInfo));
                            writer.Close();
                            DateTime timeOfTokenRequest = DateTime.Now;
                            request.BeginGetResponse(delegate (IAsyncResult getResponseResult) {
                                if (getResponseResult.IsCompleted)
                                {
                                    try
                                    {
                                        HttpWebResponse response = request.EndGetResponse(getResponseResult) as HttpWebResponse;
                                        callback(ProcessAccessTokenResponse(request, response, appInfo.AuthInfo, timeOfTokenRequest));
                                    }
                                    catch (WebException exception)
                                    {
                                        appInfo.AuthInfo.Error = ProcessGetTokenWebException(exception);
                                        callback(false);
                                    }
                                    catch (Exception exception2)
                                    {
                                        appInfo.AuthInfo.Error = exception2;
                                        callback(false);
                                    }
                                }
                            }, null);

                           
                            //<>c__DisplayClass8 CS$<>8__locals9 = (<>c__DisplayClass8) this;
                            //writer.Write(BuildRefreshTokenRequestBody(appInfo));
                            //writer.Close();
                            //DateTime timeOfTokenRequest = DateTime.Now;
                            //request.BeginGetResponse(delegate (IAsyncResult getResponseResult) {
                            //    if (getResponseResult.IsCompleted)
                            //    {
                            //        try
                            //        {
                            //            HttpWebResponse response = CS$<>8__locals9.request.EndGetResponse(getResponseResult) as HttpWebResponse;
                            //            CS$<>8__locals9.callback(ProcessAccessTokenResponse(CS$<>8__locals9.request, response, CS$<>8__locals9.appInfo.AuthInfo, timeOfTokenRequest));
                            //        }
                            //        catch (WebException exception)
                            //        {
                            //            CS$<>8__locals9.appInfo.AuthInfo.Error = ProcessGetTokenWebException(exception);
                            //            CS$<>8__locals9.callback(false);
                            //        }
                            //        catch (Exception exception2)
                            //        {
                            //            CS$<>8__locals9.appInfo.AuthInfo.Error = exception2;
                            //            CS$<>8__locals9.callback(false);
                            //        }
                            //    }
                            //}, null);
                        }
                    }
                    catch (Exception exception)
                    {
                        appInfo.AuthInfo.Error = exception;
                        callback(false);
                    }
                }
            }, null);
        }

        internal static void RequestAccessTokenAsync(AppInformation appInfo, Action<bool> callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            if (appInfo == null)
            {
                throw new ArgumentNullException("appInfo");
            }
            Uri requestUri = new Uri(appInfo.ConsentRoot, "AccessToken.aspx");
            HttpWebRequest request = WebRequest.Create(requestUri) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.BeginGetRequestStream(delegate (IAsyncResult result) {
                if (result.IsCompleted)
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(request.EndGetRequestStream(result)))
                        {
                            //<>c__DisplayClass2 CS$<>8__locals3 = (<>c__DisplayClass2) this;
                            writer.Write(BuildAccessTokenRequestBody(appInfo));
                            writer.Close();
                            DateTime timeOfTokenRequest = DateTime.Now;
                            request.BeginGetResponse(delegate (IAsyncResult getResponseResult) {
                                if (getResponseResult.IsCompleted)
                                {
                                    try
                                    {
                                        HttpWebResponse response = request.EndGetResponse(getResponseResult) as HttpWebResponse;
                                        callback(ProcessAccessTokenResponse(request, response, appInfo.AuthInfo, timeOfTokenRequest));
                                        //HttpWebResponse response = CS$<>8__locals3.request.EndGetResponse(getResponseResult) as HttpWebResponse;
                                        //CS$<>8__locals3.callback(ProcessAccessTokenResponse(CS$<>8__locals3.request, response, CS$<>8__locals3.appInfo.AuthInfo, timeOfTokenRequest));
                                    }
                                    catch (WebException exception)
                                    {
                                        appInfo.AuthInfo.Error = ProcessGetTokenWebException(exception);
                                        callback(false);
                                        //CS$<>8__locals3.appInfo.AuthInfo.Error = ProcessGetTokenWebException(exception);
                                        //CS$<>8__locals3.callback(false);
                                    }
                                    catch (Exception exception2)
                                    {
                                        appInfo.AuthInfo.Error = exception2;
                                        callback(false);
                                        //CS$<>8__locals3.appInfo.AuthInfo.Error = exception2;
                                        //CS$<>8__locals3.callback(false);
                                    }
                                }
                            }, null);
                        }
                    }
                    catch (Exception exception)
                    {
                        appInfo.AuthInfo.Error = exception;
                        callback(false);
                    }
                }
            }, null);
        }
    }
}

