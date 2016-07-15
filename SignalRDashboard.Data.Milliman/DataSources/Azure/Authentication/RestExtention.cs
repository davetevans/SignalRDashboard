using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;

namespace SignalRDashboard.Data.Milliman.DataSources.Azure.Authentication
{
    public static class RestExtention
    {
        public static AuthenticationResult Authenticate(this AzureActiveDirectory application)
        {
            var authenticationContext = new AuthenticationContext(application.AuthenticationEndpoint);
            var clientCredential = new ClientCredential(application.ClientId, application.Password);
            var token = authenticationContext.AcquireTokenAsync(application.Resource, clientCredential).Result;

            return token;
        }
        public static ServiceClientCredentials GetAzureServiceCredentials(this AzureActiveDirectory account)
        {
            var token = account.Authenticate();
            var tokenCloudCredentials = new TokenCredentials(token.AccessToken);

            return tokenCloudCredentials;
        }
    }
}
