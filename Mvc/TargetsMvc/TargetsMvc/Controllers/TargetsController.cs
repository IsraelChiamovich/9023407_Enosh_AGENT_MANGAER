using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TargetsMvc.Dto;

namespace TargetsMvc.Controllers
{
    public class TargetsController(IHttpClientFactory clientFactory) : Controller
    {
        private readonly string BaseUrl = "https://localhost:7118/targets";
        public async Task<IActionResult> Index()
        {
            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl);
            var result = await httpClient.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                List<TargetDto>? targets = JsonSerializer.Deserialize<List<TargetDto>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(targets);
            }
            return View();
        }
        
    }
}
