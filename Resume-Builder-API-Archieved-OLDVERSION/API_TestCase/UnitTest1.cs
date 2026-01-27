using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace API_TestCase
{
    public class UserTestsAuth
    {
        string _publicKey = "";
        string _baseUrl = "https://localhost:7194";
        string _authUrl;
        string _deleteUrl;

        public UserTestsAuth()
        {
            _authUrl = _baseUrl + "/api/Users/Auth";
            _deleteUrl = _baseUrl + "/api/Users/Delete";
            Trace.Listeners.Add(new ConsoleTraceListener());
        }

        [Fact]
        public async Task TestAuthEndpoint_InvalidApiKey_ReturnsUnauthorized()
        {
            using (var client = new HttpClient())
            {
                var payload = new
                {
                    apiKey = "invalid_key",
                    FirstName = "Test",
                    LastName = "User",
                    Email = "testuser@example.com",
                    Mobile = "1234567890"
                };

                var response = await client.PostAsJsonAsync(_authUrl, payload);

                Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }

        [Fact]
        public async Task TestAuthEndpoint_ValidApi_ReturnsUnauthorized()
        {
            using (var client = new HttpClient())
            {
                var payload = new
                {
                    apiKey = "NARESH_IS_AKAMI",
                    FirstName = "Test",
                    LastName = "User",
                    Email = "testuser@example.com",
                    Mobile = "1234567890"
                }; 
                
                var response = await client.PostAsJsonAsync(_authUrl, payload);

                // Deserialize the response content to access userTemp.publicId
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<dynamic>(jsonResponse);

                _publicKey = responseObject.userTemp.publicId.ToString();

                Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task Test_DeletePublicKey_User()
        {
            using (var client = new HttpClient())
            {
                string url = _deleteUrl + $"/{_publicKey}";

                var response = await client.DeleteAsync(url);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<dynamic>(jsonResponse);


                Assert.Equal("User deleted successfully", responseObject.message);
            }
        }

    }
}
