using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TargetsMvc.Dto;

namespace TargetsMvc.Controllers
{
    public class AgentsController(IHttpClientFactory clientFactory) : Controller
    {
        private readonly string BaseUrl = "https://localhost:7118/agents";
        public async Task<IActionResult> Index()
        {
            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl);
            var result = await httpClient.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                List<AgentDto>? agents = JsonSerializer.Deserialize<List<AgentDto>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true});
                return View(agents);
            }
            return View();
        }
    }
}

