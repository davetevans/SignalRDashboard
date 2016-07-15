using Microsoft.Azure;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;

namespace SignalRDashboard.Data.Milliman.DataSources.Azure.Authentication
{
    public static class AzureResourceManagerConfigurationExtensions
    {
        public static TokenCloudCredentials GetTokenCloudCredentials(this AzureResourceManager application, string subscriptionId)
        {
            var token = GetAuthToken(application);
            var tokenCloudCredentials = new TokenCloudCredentials(subscriptionId, token);

            return tokenCloudCredentials;
        }

        public static TokenCredentials GetTokenCredentials(this AzureResourceManager application)
        {
            var token = GetAuthToken(application);
            var tokenCloudCredentials = new TokenCredentials(token);

            return tokenCloudCredentials;
        }

        static string GetAuthToken(AzureResourceManager application)
        {
            var authenticationContext = new AuthenticationContext(application.AuthenticationEndpoint);
            var clientCredential = new ClientCredential(application.ClientId, application.ClientSecret);
            //var token = authenticationContext.AcquireToken(application.Resource, clientCredential);

            //return token.AccessToken;
            return "todo";
        }
    }
}
