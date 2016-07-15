using Microsoft.Azure;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace SignalRDashboard.Data.Milliman.DataSources.Azure.Authentication
{

    public static class AzureActiveDirectoryExtensions
    {
        public static AuthenticationResult Authenticate(this AzureActiveDirectory application, string username, string password)
        {
            var authenticationContext = new AuthenticationContext(application.AuthenticationEndpoint);
            //var userCredential = new UserCredential(username, password);
            var userCredential = new UserCredential(username);
            var token = authenticationContext.AcquireTokenAsync(application.Resource, application.ClientId, userCredential);

            return token.Result;
        }

        public static TokenCloudCredentials GetAzureCredentials(this AzureActiveDirectory application, string subscriptionId)
        {
            var token = application.Authenticate(application.Username, application.Password);
            var tokenCloudCredentials = new TokenCloudCredentials(subscriptionId, token.AccessToken);

            return tokenCloudCredentials;
        }

        //public static TokenCloudCredentials GetTokenCloudCredentials(this AzureActiveDirectory application, string subscriptionId)
        //{
        //    var token = GetAuthToken(application, application.Username, application.Password);
        //    var tokenCloudCredentials = new TokenCloudCredentials(subscriptionId, token);

        //    return tokenCloudCredentials;
        //}

        //static string GetAuthToken(AzureActiveDirectory application, string username, string password)
        //{
        //    var authenticationContext = new AuthenticationContext(application.AuthenticationEndpoint);
        //    var clientCredential = new UserCredential(username, password);
        //    var token = authenticationContext.AcquireToken(application.Resource, application.ClientId, clientCredential);

        //    return token.AccessToken;
        //}
    }
}
