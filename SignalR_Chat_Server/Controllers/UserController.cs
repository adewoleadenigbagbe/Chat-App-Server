using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SignalR_Chat_Server.DAO;
using SignalR_Chat_Server.Model;
using NLog;

namespace SignalR_Chat_Server.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly UserRepository _userRepository; 
        public UserController(IOptions<DataBaseSettings> options)
        {
            _userRepository = new UserRepository(options);
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            _logger.Debug("Started");
            var loginUser = await _userRepository.GetUser(user.UserName.ToLower().Trim(), user.Password.ToLower().Trim());
            if (loginUser == null)
            {
                var respons = new
                {
                    ResponseMessage = "Username and Password not found",
                    ResponseCode = "10",
                    UserName = "",
                    Password = ""
                };
                return Ok(respons);

            }


            if (!string.IsNullOrEmpty(loginUser.UserName))
            {
                var response = new
                {
                    ResponseMessage = "Success",
                    ResponseCode = "00",
                    user.UserName,
                    user.Password
                };

                return Ok(response);
            }

            var responseErr = new
            {
                ResponseMessage = "Username and Password not found",
                ResponseCode = "10",
                UserName = "",
                Password = ""
            };
            return Ok(responseErr);
        }

        [Route("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User user)
        {
            var result = await _userRepository.CreateUser(user);
            var response = new
            {
                ResponseMessage = result.Item2,
                ResponseCode = result.Item1,
                user.UserName,
                user.Password
            };
            return Ok(response);
        }
    }
}