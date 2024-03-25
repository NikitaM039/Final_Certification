using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using WebApiLibr.Models.DTO;
using Microsoft.AspNetCore.Authentication;
using ApiMessages.Abstraction;

namespace ApiMessages.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : ControllerBase
    {
        
        public class UserController : ControllerBase
        {
            private readonly IMessageRepository _messageRepository;

            public UserController(IMessageRepository messageRepository)
            {
                _messageRepository = messageRepository;
            }

            [HttpGet]
            [Route("get_message")]
            [Authorize(Roles = "Admin, User")]
            public IActionResult GetNewMessage()
            {
                var senderEmail = GetUserEmailFromToken().GetAwaiter().GetResult();
                var result = _messageRepository.GetUnreadMessages;
                
                return Ok(result);
            }


            [HttpPost]
            [Route("send_message")]
            [Authorize(Roles = "Admin, User")]
            public IActionResult SendMessage(string recipientEmail, string text)
            {
                var senderEmail = GetUserEmailFromToken().GetAwaiter().GetResult();

                var model = new MessageDTO { SenderEmail = senderEmail,RecipientEmail = recipientEmail, Text = text };

                var result = _messageRepository.SendMessage(model);
                return Ok(result);
            }

            private async Task<string> GetUserEmailFromToken()
            {
                var token = await HttpContext.GetTokenAsync("access_token");

                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
                var claim = jwtToken!.Claims.Single(x => x.Type.Contains("emailaddress"));

                return claim.Value;

            
            }
        }
    }
}
