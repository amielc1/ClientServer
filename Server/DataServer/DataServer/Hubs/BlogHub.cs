using Microsoft.AspNetCore.SignalR;

namespace DataServer.Hubs;

public class BlogHub : Hub
{

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<BlogHub> _logger;
    public BlogHub(IHttpClientFactory httpClientFactory, ILogger<BlogHub> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }



    public async Task GetLatestPosts()
    {
        _logger.LogInformation("from hub, invoke GetLatestPosts() start");
        // Fetch latest posts from your data source
        var posts = await GetPosts();
        _logger.LogInformation($"from hub, invoke GetLatestPosts() received all posts {posts}");

        _logger.LogInformation("from hub, send posts to ReceivePosts");

        await Clients.Caller.SendAsync("ReceivePosts", posts);
    }


    private async Task<string> GetPosts()
    {
        try
        {
            string apiUrl = "https://jsonplaceholder.typicode.com/posts";

            var client = _httpClientFactory.CreateClient();
            _logger.LogInformation("before Get Posts from WEB");
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            _logger.LogInformation("after  Get Posts from WEB");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("read all string from response done");
                return content;
            }
            else
            {
                return "Failed to retrieve data. Status code: " + response.StatusCode;
            } 
        }
        catch (Exception e)
        {
            _logger.LogError(e, "error when perform GET REST");
            throw;
        }
    }
}