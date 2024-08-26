using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using TargetsMvc.Dto;

namespace TargetsMvc.Controllers
{
    public class AgentsController(IHttpClientFactory clientFactory, TokenDto token) : Controller
    {
        private readonly string baseUrl = "https://localhost:7118/agents";
        private readonly string MissionsBaseUrl = "https://localhost:7118/Missions";
        public async Task<IActionResult> Index()
        {
            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, baseUrl);
            var result = await httpClient.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                List<AgentDto>? agents = JsonSerializer.Deserialize<List<AgentDto>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true});
                return View(agents);
            }
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
            var result = await httpClient.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                AgentDto? agent = JsonSerializer.Deserialize<AgentDto>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(agent);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Missions(int id)
        {
            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/{id}/Missions");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth.Token);
            var result = await httpClient.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                MissionDto? mission = JsonSerializer.Deserialize<MissionDto>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(mission);
            }
            return RedirectToAction("Index");
        }
    }
}

