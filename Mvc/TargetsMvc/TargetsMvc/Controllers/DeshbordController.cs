using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TargetsMvc.Dto;
using TargetsMvc.ViewModels;

namespace TargetsMvc.Controllers
{
    public class DeshbordController(IHttpClientFactory clientFactory) : Controller
    {
        private readonly string baseUrl = "https://localhost:7118/Deshbord";
        public async Task<IActionResult> Index()
        {
            var httpClient = clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, baseUrl);
            var result = await httpClient.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                DeshbordVM? deshbordVM = JsonSerializer.Deserialize<DeshbordVM>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                return View(deshbordVM);
            }
            return View();
        }
    }
}
