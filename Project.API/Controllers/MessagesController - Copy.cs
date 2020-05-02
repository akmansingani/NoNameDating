using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.API.Data;
using Project.API.Dtos;
using Project.API.Helpers;
using Project.API.Models;

namespace Project.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController1 : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IDatingRepo _datingrepo;
        private readonly IMapper _mapper;

        public MessagesController1(IAuthRepo authrepo, IConfiguration config, IDatingRepo datingrepo, IMapper mapper)
        {
            _config = config;
            _datingrepo = datingrepo;
            _mapper = mapper;
        }

        // GET: api/Messages
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Messages/5
        [HttpGet("getmessage/{userid}/{messageid}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage1(int userid,int messageid)
        {
            try
            {
                if (userid != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    return Unauthorized();

                var message = await _datingrepo.GetMessage(messageid);

                if(message == null)
                {
                    return NotFound();
                }

                return Ok(message);

            }
            catch (Exception ex)
            {
                return BadRequest("Error while fetching message");
            }

           
        }

        [HttpGet("getmessageuser/{userid}/")]
        public async Task<IActionResult> GetMessageUser1(int userid, [FromQuery] MessageParamsDto query)
        {
            try
            {
                if (userid != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    return Unauthorized();

                query.UserID = userid;

                var messages = await _datingrepo.GetMessagesForUser(query);

                var result = _mapper.Map<IEnumerable<MessageReturnDto>>(messages);

                Response.AddPaginationHeader(messages.TotalCount, messages.TotalPages,
                    messages.PageSize, messages.CurrentPage);

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest("Error while fetching user messages");
            }


        }


        [HttpGet("getmessagethread/{userid}/{receiverid}")]
        public async Task<IActionResult> GetMessageThread(int userid, int receiverid)
        {
            try
            {
                if (userid != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    return Unauthorized();

                var message = await _datingrepo.GetMessageThread(userid,receiverid);

                var result = _mapper.Map<IEnumerable<MessageReturnDto>>(message);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error while fetching message thread");
            }


        }

        // POST: api/Messages
        [HttpPost("createmessage/{userid}")]
        public async Task<IActionResult> CreateMessage(int userid, MessageCreateDto messageParams)
        {
            try
            {
                if (userid != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    return Unauthorized();

                messageParams.SenderID = userid;

                var receiver = await _datingrepo.GetUser(messageParams.ReceiverID);
                if (receiver == null)
                {
                    return BadRequest("Receiving user does not exists");
                }

                var message = _mapper.Map<Messages>(messageParams);

                _datingrepo.Add(message);

                var returnMessage = _mapper.Map<MessageCreateDto>(message);

                if (await _datingrepo.SaveAll())
                {
                    return CreatedAtRoute("GetMessage", new { id = message.MessageID }, returnMessage);
                }

                return BadRequest("Error while creating message");

                

            }
            catch (Exception ex)
            {
                return BadRequest("Error while creating message");
            }
        }

        // PUT: api/Messages/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
