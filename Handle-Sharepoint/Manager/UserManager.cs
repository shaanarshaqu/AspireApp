using Handle_Sharepoint.Data.Models;
using Handle_Sharepoint.Manager.Inteface;
using Handle_Sharepoint.Secure;
using Npgsql;
using System.Net.Http.Headers;
using System.Text;

namespace Handle_Sharepoint.Manager
{
    public class UserManager:IUserManager
    {
        private readonly NpgsqlConnection npgsqlConnection;
        private readonly ILogger<UserManager> log;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;
        private readonly AzureAuth azureAuth;
        public UserManager(IConfiguration configuration, ILogger<UserManager> log,IHttpClientFactory httpClientFactory, AzureAuth azureAuth)
        {
            npgsqlConnection = new NpgsqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
            this.log = log;
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
            this.azureAuth = azureAuth;
        }




        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                List<User> user_list = new List<User>();
                using (NpgsqlCommand cmd = new NpgsqlCommand("select * from Users", npgsqlConnection))
                {
                    npgsqlConnection.Open();
                    NpgsqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        user_list.Add(new User
                        {
                            Id = (int)reader["id"],
                            Name = (string)reader["name"]
                        });
                    }
                }
                return user_list;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    public async Task<bool> AddUser(User user)
    {
        try
        {
            using (var cmd = new NpgsqlCommand("SELECT AddToUsers(@name)", npgsqlConnection))
            {
                npgsqlConnection.Open();
                cmd.Parameters.AddWithValue("name", user.Name);
                int res = (int)await cmd.ExecuteScalarAsync();
                log.LogInformation($"{res}");

                if (res > 0)
                {
                    var client = httpClientFactory.CreateClient();
                    string accessToken = await azureAuth.GenerateToken();
                    var body = new
                    {
                        email = "shaanarshaqup@gmail.com"
                    };

                    var jsonBody = System.Text.Json.JsonSerializer.Serialize(body);
                    log.LogInformation(jsonBody);
                    var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    string mailflow = "https://prod-18.centralindia.logic.azure.com:443/workflows/4ef8bbd21d354a3a9b55e14623ecdf94/triggers/manual/paths/invoke?api-version=2016-06-01";
                    var response = await client.PostAsync(mailflow, content);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return response.IsSuccessStatusCode;
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            log.LogError($"Exception: {ex.Message}");
            throw;
        }
    } 


        /*public async Task<bool> AddUser(User user)
        {
            try
            {
                using (var cmd = new NpgsqlCommand("SELECT AddToUsers(@name)", npgsqlConnection))
                {
                    npgsqlConnection.Open();
                    cmd.Parameters.AddWithValue("name", user.Name);
                    //cmd.CommanType = CommandType.StoredProcedure
                    int res = (int)await cmd.ExecuteScalarAsync();
                    if (res > 0)
                    {
                        var client = httpClientFactory.CreateClient();
                        var body = new
                        {
                            email = "shaanarshaqup@gmail.com"
                        };

                        var jsonBody = System.Text.Json.JsonSerializer.Serialize(body);
                        log.LogInformation(jsonBody);
                        var content = new StringContent(jsonBody, null, "application/json");
                        string mailflow = "https://prod-16.centralindia.logic.azure.com:443/workflows/fb7c854a56b1488096f266bb7950e9d7/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=r76fsB2DcUaGzXjbhbC4MJlByBklSWPqZd7QUIFIm_w";         
                        var response = await client.PostAsync(mailflow, content);
                        var responseBody = await response.Content.ReadAsStringAsync();
                        return response.IsSuccessStatusCode;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }*/
    }
}
