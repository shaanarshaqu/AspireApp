using Microsoft.Identity.Client;

namespace Handle_Sharepoint.Secure
{
    public class AzureAuth
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<AzureAuth> logger;
        public AzureAuth(IConfiguration configuration, ILogger<AzureAuth> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }


        public async Task<string> GenerateToken()
        {
            logger.LogInformation("Generating token...");
            logger.LogInformation(@$"
            client-id : {configuration["AzureAuth:ClientId"]}       
            ClientSecret : {configuration["AzureAuth:ClientSecret"]}
            Authority : {configuration["AzureAuth:Authority"]}
            Scope : {configuration["AzureAuth:Scope"]}     
            ");

            var app = ConfidentialClientApplicationBuilder.Create(configuration["AzureAuth:ClientId"])
                .WithClientSecret(configuration["AzureAuth:ClientSecret"])
                .WithAuthority(new Uri(configuration["AzureAuth:Authority"]))
                .Build();

            var result = await app.AcquireTokenForClient(new[] { configuration["AzureAuth:Scope"] }).ExecuteAsync();
            return result.AccessToken;
        }

    }
}
