using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Text.Json;
using TargetsMvc.Dto;

namespace TargetsMvc.Controllers
{
    public class TargetsController(IHttpClientFactory clientFactory, TokenDto token) : Controller
    {
        private readonly string baseUrl = "https://localhost:7118/targets";
        public async Task<IActionResult> Index()
        {
            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, baseUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
            var result = await httpClient.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                List<TargetDto>? targets = JsonSerializer.Deserialize<List<TargetDto>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(targets);
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
                TargetDto? target = JsonSerializer.Deserialize<TargetDto>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(target);
            }
            return RedirectToAction("Index");
        }

    }
}
