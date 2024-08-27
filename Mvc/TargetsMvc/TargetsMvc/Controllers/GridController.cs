using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TargetsMvc.Dto;
using TargetsMvc.ViewModels;

namespace TargetsMvc.Controllers
{
    public class GridController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _agentsBaseUrl = "https://localhost:7118/agents";
        private readonly string _targetsBaseUrl = "https://localhost:7118/targets";

        public GridController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public IActionResult Index()
        {
            var model = new GridVM(1000); 

            return View(model);
        }

        public async Task<IActionResult> Update()
        {
            var model = new GridVM(1000);

            var httpClient = _clientFactory.CreateClient();
            var agentsRequest = new HttpRequestMessage(HttpMethod.Get, _agentsBaseUrl);
            var agentsResult = await httpClient.SendAsync(agentsRequest);

            if (agentsResult.IsSuccessStatusCode)
            {
                var content = await agentsResult.Content.ReadAsStringAsync();
                List<AgentDto>? agents = JsonSerializer.Deserialize<List<AgentDto>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                if (agents != null)
                {
                    foreach (var agent in agents)
                    {
                        if (agent.x < model.Size && agent.y < model.Size)
                        {
                            model.Grid[agent.x, agent.y] = agent.ImageLink; 
                        }
                    }
                }
            }

            var targetsRequest = new HttpRequestMessage(HttpMethod.Get, _targetsBaseUrl);
            var targetsResult = await httpClient.SendAsync(targetsRequest);

            if (targetsResult.IsSuccessStatusCode)
            {
                var content = await targetsResult.Content.ReadAsStringAsync();
                List<TargetDto>? targets = JsonSerializer.Deserialize<List<TargetDto>>(content, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                if (targets != null)
                {
                    foreach (var target in targets)
                    {
                        if (target.x < model.Size && target.y < model.Size)
                        {
                            model.Grid[target.x, target.y] = target.ImageLink; 
                        }
                    }
                }
            }

            return View("Index", model); 
        }
    }
}
