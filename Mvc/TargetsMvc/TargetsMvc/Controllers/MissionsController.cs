using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TargetsMvc.Dto;
using TargetsMvc.Models;

namespace TargetsMvc.Controllers
{
    public class MissionsController(IHttpClientFactory clientFactory, TokenDto token) : Controller
    {
        private readonly string baseUrl = "https://localhost:7118/Missions";
        public async Task<IActionResult> Index()
        {
            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, baseUrl);
            var result = await httpClient.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                List<MissionDto>? missions = JsonSerializer.Deserialize<List<MissionDto>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(missions);
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
                MissionDto? mission = JsonSerializer.Deserialize<MissionDto>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(mission);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Assigned(int id, MissionModel missionModel)
        {
            int missionId = id;
            missionModel.status = "assigned";
            var httpClient = clientFactory.CreateClient();
            var httpContent = new StringContent(JsonSerializer.Serialize(missionModel), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"{baseUrl}/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
            request.Content = httpContent;
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
