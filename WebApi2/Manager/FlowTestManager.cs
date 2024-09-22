using System.Net.Http.Headers;
using System.Net.Http;
using WebApi2.Manager.Interface;
using Microsoft.IdentityModel.Clients.ActiveDirectory;





namespace WebApi2.Manager
{
    public class FlowTestManager:IFlowTestManager
    {
        private readonly IConfiguration configuration;
        private readonly HttpClient _httpClient;
        public FlowTestManager(IConfiguration configuration, HttpClient _httpClient) 
        { 
            this.configuration = configuration;
            this._httpClient = _httpClient;
        }


        public async Task<bool> CalliningAuthorizedFlow()
        {
            try
            {
                var flowTriggerUrl = "https://prod-18.centralindia.logic.azure.com:443/workflows/4ef8bbd21d354a3a9b55e14623ecdf94/triggers/manual/paths/invoke?api-version=2016-06-01"; // Replace with your actual URL

                // Obtain an access token for the flow's tenant
                var authenticationContext = new AuthenticationContext(configuration["AzureAuth:Authority"]);
                var clientCredential = new ClientCredential(configuration["AzureAuth:ClientId"], configuration["AzureAuth:ClientSecret"]);
                var authenticationResult = await authenticationContext.AcquireTokenAsync(flowTriggerUrl, clientCredential);

                // Add the access token to the request headers
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);

                // Make the request to trigger the flow
                var response = await _httpClient.PostAsync(flowTriggerUrl, null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
