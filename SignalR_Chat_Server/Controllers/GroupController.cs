using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SignalR_Chat_Server.DAO;
using SignalR_Chat_Server.Model;

namespace SignalR_Chat_Server.Controllers
{
    [Produces("application/json")]
    [Route("api/Group")]
    public class GroupController : Controller
    {
        private readonly GroupRepository _groupRepository;
        public GroupController(IOptions<DataBaseSettings> options)
        {
            _groupRepository = new GroupRepository(options);
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Groups()
        {
            var list = await _groupRepository.Groups();
            if(list.Count() > 0)
            {
                var response = new
                {
                    ResponseCode = "00",
                    list
                };
                return Ok(response);
            }

            var responseR = new
            {
                ResponseCode = "10",
                list
            };
            return Ok(responseR);

        }
    }
}