namespace SignalRDashboard.Data.Milliman.DataSources.Azure.Authentication
{
    public static class Config
    {
        public static AzureActiveDirectory StorageAccount = new AzureActiveDirectory
        {
            AuthenticationEndpoint = "https://login.windows.net/mgalfadevlive.onmicrosoft.com",
            Username = "storage@mgalfadevlive.onmicrosoft.com",
            Password = "N....",
            Resource = "https://management.core.windows.net/",
            ClientId = "dfd6930d-31eb-494d-b27c-03a5c662909d",

        };

        public static AzureActiveDirectory RmAccount = new AzureActiveDirectory
        {
            AuthenticationEndpoint = "https://login.windows.net/mgalfadevlive.onmicrosoft.com",
            Password = "P...",
            Resource = "https://management.core.windows.net/",
            ClientId = "61d42bef-cae1-43d8-9b54-502f1fa9827a",

        };
    }
}