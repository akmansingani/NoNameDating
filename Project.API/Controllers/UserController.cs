using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Project.API.Data;
using Project.API.Dtos;
using Project.API.Helpers;
using Project.API.Models;

namespace Project.API.Controllers
{
    [Authorize]

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IDatingRepo _datingrepo;
        private readonly IAuthRepo _authrepo;
        private readonly IMapper _mapper;

        public UserController(IAuthRepo authrepo, IConfiguration config, IDatingRepo datingrepo,IMapper mapper)
        {
            _authrepo = authrepo;
            _config = config;
            _datingrepo = datingrepo;
            _mapper = mapper;
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParamsDto query)
        {
            int currentUID = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            query.UserID = currentUID;

            var users = await _datingrepo.GetUsers(query);
            
            var result = _mapper.Map<IEnumerable<UserListDto>>(users);

            Response.AddPaginationHeader(users.TotalCount, users.TotalPages, users.PageSize, users.CurrentPage);

            return Ok(result);
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _datingrepo.GetUser(id);
            var result = _mapper.Map<UserListDto>(user);

            return Ok(result);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id,UserUpdateDto userModel)
        {
            try
            {
                if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    return Unauthorized();

                var dbUser = await _datingrepo.GetUser(id);
                _mapper.Map(userModel, dbUser);

                if (await _datingrepo.SaveAll())
                {
                    return NoContent();
                }

                throw new Exception($"Error updating user {userModel.UserID} !");
            }
            catch(Exception ex)
            {
                throw new Exception($"Error updating user {userModel.UserID} !");
            }

        }

        [HttpPost("likeuser/{id}/{likeuserid}")]
        public async Task<IActionResult> LikeUser(int id,int likeuserid)
        {
            try
            {
                if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                    return Unauthorized();

                var likes = await _datingrepo.GetLikes(id,likeuserid);
               
                if(likes != null)
                {
                    return BadRequest("You have already liked the user!");
                }

                if(await _datingrepo.GetUser(likeuserid) == null)
                {
                    return NotFound();
                }

                var like = new Likes
                {
                    LikeByUserID = id,
                    LikedUserID = likeuserid
                };

                _datingrepo.Add(like);

                if(await _datingrepo.SaveAll())
                {
                    return Ok();
                }
             
                throw new Exception($"Error adding user likeuserid !");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding like user likeuserid !");
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
