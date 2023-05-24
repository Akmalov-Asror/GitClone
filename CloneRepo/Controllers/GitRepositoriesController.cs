using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CloneRepo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitRepositoriesController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public GitRepositoriesController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> GetGitHubProjects()
        {
            try
            {
                // GitHub API ga so'rov yuborish
                string apiUrl = "https://api.github.com/users/{username}/repos";
                string username = "Akmalov-Asror";
                string token = "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";
                string requestUrl = apiUrl.Replace("{username}", username);

                var client = _clientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("User-Agent", "request");
                client.DefaultRequestHeaders.Add("Authorization", $"Token {token}");

                var response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    // JSON ma'lumotlarni olish
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    var responseString = await response.Content.ReadAsStringAsync();
                    if (responseString == null)
                    {
                        Console.WriteLine("string null");
                    }
                    Console.WriteLine(responseString);

                    var projects = await JsonSerializer.DeserializeAsync<List<GitHubProject>>(responseStream);

                    // Proyektlarni chiqarish
                    List<string> projectNames = projects.Select(p => p.name).ToList();
                    return Ok(projectNames);
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "GitHub API bilan xatolik yuz berdi.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

    public class GitHubProject
    {
        public string name { get; set; }
    }
}

