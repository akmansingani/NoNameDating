using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Project.API.Data;
using Project.API.Helpers;
using CloudinaryDotNet;
using Project.API.Dtos;
using System.Security.Claims;
using CloudinaryDotNet.Actions;
using Project.API.Models;

namespace Project.API.Controllers
{
    [Authorize]

    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepo _datingrepo;

        private readonly IMapper _mapper;

        private readonly IOptions<CloudinarySettings> _config;

        private Cloudinary _cloudinary;

        public PhotosController(IDatingRepo datingrepo, IMapper mapper, IOptions<CloudinarySettings> config)
        {
            _datingrepo = datingrepo;
            _mapper = mapper;
            _config = config;

            Account acc = new Account(
            _config.Value.name,
            _config.Value.key,
            _config.Value.secret);

            _cloudinary = new Cloudinary(acc);

        }

        [HttpGet("getphoto/{photoid}", Name = "getphoto")]

        public async Task<IActionResult> GetPhoto(int photoid)
        {
            var photoFromDB = await _datingrepo.GetPhoto(photoid);

            var photo = _mapper.Map<PhotoDto>(photoFromDB);

            return Ok(photo);
        }

        [HttpPost("addphoto/{userid}")]

        public async Task<IActionResult> AddPhoto(int userid,[FromForm] PhotoDto photoReceived)
        {
            if (userid != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var dbUser = await _datingrepo.GetUser(userid);

            var file = photoReceived.file;

            var objUpload = new ImageUploadResult();

            if(file.Length > 0)
            {
                using(var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.FileName, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    objUpload = _cloudinary.Upload(uploadParams);
                }
            }

            photoReceived.CreatedDate = DateTime.Now;
            photoReceived.Url = objUpload.Uri.ToString();
            photoReceived.PublicID = objUpload.PublicId;

            var photo = _mapper.Map<Photos>(photoReceived);

            if(dbUser.Photos.Count <= 0)
            {
                photo.IsMain = true;
            }

            dbUser.Photos.Add(photo);

            if (await _datingrepo.SaveAll())
            {
                var result = _mapper.Map<PhotoDto>(photo);
                return CreatedAtRoute("getphoto", new { photoid = photo.Id }, result);
            }

            return BadRequest("Error uploading photo");
        }

       [HttpPost("setmainphoto/{userid}/{photoid}")]

       public async Task<IActionResult> SetMainPhoto(int userid,int photoid)
        {
            if (userid != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var dbUser = await _datingrepo.GetUser(userid);

            var newMainPhoto = dbUser.Photos.Where(x => x.Id == photoid).FirstOrDefault();

            if (newMainPhoto == null)
            {
                return Unauthorized();
            }

            var currentMainPhoto = dbUser.Photos.Where(x => x.IsMain == true).FirstOrDefault();
            currentMainPhoto.IsMain = false;

            newMainPhoto.IsMain = true;

            if (await _datingrepo.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Error setting main photo");
        }

        [HttpDelete("delete/{userid}/{photoid}")]

        public async Task<IActionResult> DeletePhoto(int userid, int photoid)
        {
            if (userid != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var dbUser = await _datingrepo.GetUser(userid);

            var photo = dbUser.Photos.Where(x => x.Id == photoid).FirstOrDefault();

            if (photo == null)
            {
                return Unauthorized();
            }

            if(photo.IsMain)
            {
                return BadRequest("You cannot delete main photo");
            }

            if(string.IsNullOrEmpty(photo.PublicID))
            {
                _datingrepo.Delete(photo);
            }
            else
            {
                var deleteParams = new DeletionParams(photo.PublicID);

                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                {
                    _datingrepo.Delete(photo);
                }
            }

            if (await _datingrepo.SaveAll())
            {
                return Ok();
            }

            return BadRequest("Error setting main photo");
        }

    }
}
