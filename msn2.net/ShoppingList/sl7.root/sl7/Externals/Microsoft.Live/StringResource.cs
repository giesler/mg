using System;

internal static class StringResource
{
    internal static string CantCallSyncMethodsInSL
    {
        get
        {
            return "Sync methods cannot be called in Silverlight.";
        }
    }

    internal static string CantFindBaseUri
    {
        get
        {
            return "Could not find the base uri for collection '{0}'.  Please make sure you have given the application consent to access the requested data and the service you're trying to reach is available.";
        }
    }

    internal static string CantSetCollectionBaseUris
    {
        get
        {
            return "CollectionBaseUris cannot be set during SignIn or after the user has already signed in.";
        }
    }

    internal static string FailToAuthenticate
    {
        get
        {
            return "Failed to grant consent to the application to permit access to user's data in Windows Live.";
        }
    }

    internal static string FailToReadServiceDoc
    {
        get
        {
            return "Failed to read service document.";
        }
    }

    internal static string FailToRefresh
    {
        get
        {
            return "Failed to refresh the access token.";
        }
    }

    internal static string InvalidAuthenticationInfoString
    {
        get
        {
            return "The authentication information passed in is invalid.";
        }
    }

    internal static string LoadInProgress
    {
        get
        {
            return "The collection is being loaded.  Please wait till load finishes before issuing another load call.";
        }
    }

    internal static string MustBeOOBFullTrust
    {
        get
        {
            return "'{0}' can only be called by applications running Out-Of-Browser and have elevated permissions.";
        }
    }

    internal static string MustBeWindowsApp
    {
        get
        {
            return "Application must be a desktop Windows applications.";
        }
    }

    internal static string NoAccessToken
    {
        get
        {
            return "Could not find access token.  Authentication information must be provided.";
        }
    }

    internal static string NoAccessTokenReturned
    {
        get
        {
            return "No access token was returned by the authentication server.";
        }
    }

    internal static string NoCallbackUrl
    {
        get
        {
            return "CallbackUrl must be specified.";
        }
    }

    internal static string NoClientId
    {
        get
        {
            return "ClientId must be specified.";
        }
    }

    internal static string NoClientSecret
    {
        get
        {
            return "ClientSecret must be specified.";
        }
    }

    internal static string NoOffer
    {
        get
        {
            return "At least one offer must be specified.";
        }
    }

    internal static string NoOfferGranted
    {
        get
        {
            return "No offers are granted.";
        }
    }

    internal static string NoQueryParams
    {
        get
        {
            return "The consent site did not return any authentication parameters.";
        }
    }

    internal static string NoRefreshTokenReturned
    {
        get
        {
            return "No refresh token is returned by the authentication server.";
        }
    }

    internal static string NoTokenForAsync
    {
        get
        {
            return "A valid access token or refresh token must be specified to call SignInAsync.";
        }
    }

    internal static string NullResponseReceived
    {
        get
        {
            return "WebRequest returned null response.";
        }
    }

    internal static string RefreshTokenInvalid
    {
        get
        {
            return "The refresh token is invalid.  The refresh token may be expired.";
        }
    }

    internal static string SignInDialogTitle
    {
        get
        {
            return "Sign in to Windows Live";
        }
    }

    internal static string SignInInProgress
    {
        get
        {
            return "Another SignIn call is in progress.";
        }
    }

    internal static string UserAlreadySignedIn
    {
        get
        {
            return "User already signed in.";
        }
    }

    internal static string UserNotSignedIn
    {
        get
        {
            return "User is not signed in.  Please call SignIn before performing any operations on the data conext.";
        }
    }

    internal static string VerifierInvalid
    {
        get
        {
            return "The verification code is invalid.";
        }
    }
}

