using APIWeb_demo.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIWeb_demo.Models;

namespace APIWeb_demo.Controllers
{
    [ApiController]
    [Route("api/Sonnette")]
    public class SonnetteController : Controller
    {
        private readonly IHubContext<NotifHub> _notificationHubContext;
        public SonnetteController(IHubContext<NotifHub> notificationHubContext)
        {
            _notificationHubContext = notificationHubContext;
        }

        [HttpPost]
        public async void Index(Sonnette model)
        {
            await _notificationHubContext.Clients.All.SendAsync("ReceiveNotif",  model.state);
        }
    }
}
