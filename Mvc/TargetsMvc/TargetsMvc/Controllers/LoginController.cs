using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using TargetsMvc.Dto;

namespace TargetsMvc.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly TokenDto _token;
        private readonly string baseUrl = "https://localhost:7118/Login";

        public LoginController(IHttpClientFactory clientFactory, TokenDto token)
        {
            _clientFactory = clientFactory;
            _token = token;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var httpClient = _clientFactory.CreateClient();

            var httpContent = new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "application/json");

            var result = await httpClient.PostAsync($"{baseUrl}", httpContent);
            if (result.IsSuccessStatusCode)
            {
                TokenDto token = new() { Token = await result.Content.ReadAsStringAsync() };
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
