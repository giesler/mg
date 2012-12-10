namespace Microsoft.Live
{
    using System;

    internal class AuthConstants
    {
        internal static class AppSettings
        {
            public const string AccessTokenPage = "AccessToken.aspx";
            public const string AuthProvider = "wl_wrap_store_provider_type";
            public const string ClientCallback = "wl_wrap_client_callback";
            public const string ClientId = "wl_wrap_client_id";
            public const string ClientSecret = "wl_wrap_client_secret";
            public const string ConnectPage = "connect.aspx";
            public const string ConsentRoot = "https://consent.live.com/";
            public const string ConsentRootSetting = "wl_wrap_consent_root";
            public const string DefaultClientOffer = "WL_Profiles.View";
            public const string ErrorPage = "Error.aspx";
            public const string LiveFxDefaultEndpoint = "http://apis.live.net";
            public const string RefreshTokenPage = "RefreshToken.aspx";
            public const string SessionIdProvider = "wl_wrap_sessionId_provider_type";
        }

        internal static class AuthCookies
        {
            public const string AccessToken = "c_accessToken";
            public const string ClientID = "c_clientId";
            public const string ClientState = "c_clientState";
            public const string Error = "c_error";
            public const string Expiry = "c_expiry";
            public const string LCA = "lca";
            public const string Scope = "c_scope";
            public const string UID = "c_uid";
        }

        internal static class InitParams
        {
            public const string AccessToken = "AccessToken";
            public const string ClientID = "ClientID";
            public const string ClientState = "ClientState";
            public const string Error = "Error";
            public const string Expiry = "Expiry";
            public const string RefreshToken = "RefreshToken";
            public const string Scope = "Scope";
            public const string UID = "UID";
        }

        internal static class JavaScript
        {
            public const string JSCloseWindow = "js_close_window";
        }

        internal static class WebReturnStatus
        {
            public const string BadRequest = "400 BadRequest";
            public const string MethodNotAllowed = "405 MethodNotAllowed";
            public const string OK = "200 OK";
            public const string Unauthorized = "401 Unauthorized";
        }

        internal static class Wrap
        {
            public const string AccessTokenParam = "wrap_access_token=";
            public static DateTime BaseDateTime = new DateTime(0x7b2, 1, 1);
            public const string ClientStateParam = "wrap_client_state=";
            public const string CodeParam = "code=";
            public const string ErrorCode = "error_code";
            public const string ErrorCodeParam = "error_code=";
            public const string ErrorInternalInfo = "internal_info";
            public const string ErrorInternalInfoParam = "internal_info=";
            public const string ErrorReason = "wrap_error_reason";
            public const string ErrorReasonParam = "wrap_error_reason=";
            public const string ExpiryParam = "wrap_access_token_expires_in=";
            public const string ExpParam = "exp=";
            public const string IdTypeParam = "uid=";
            public static char[] OfferSeparator = new char[] { ',', '.', ':' };
            public static char[] ParameterSeparator = new char[] { '&' };
            public const string PostContentType = "application/x-www-form-urlencoded";
            public static char[] QuerySeparator = new char[] { '?' };
            public const string RefreshTokenParam = "wrap_refresh_token=";
            public const string SessionId = "wl_session_id";
            public const string SessionIdParam = "wl_session_id=";
            public static char[] TokenSeparator = new char[] { ';' };
            public const string UserDenied = "user_denied";
            public const string Verifier = "wrap_verification_code";
            public const string VerifierParam = "wrap_verification_code=";
            public static char[] VerifierTitleSeparator1 = new char[] { ',' };
            public static char[] VerifierTitleSeparator2 = new char[] { ' ' };
        }
    }
}

