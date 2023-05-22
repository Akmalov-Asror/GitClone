using CloneRepo.Entities;

namespace CloneRepo.Services;


public class GitHubService
{
    private readonly HttpClient _httpClient;

    public GitHubService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Repository>> GetRepositories(string owner, int count)
    {
        var apiUrl = $"https://api.github.com/users/{owner}/repos?per_page={count}";
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "YourAppName");

        var response = await _httpClient.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode();

        var repositories = await response.Content.ReadFromJsonAsync<List<Repository>>();
        return repositories;
    }
}
