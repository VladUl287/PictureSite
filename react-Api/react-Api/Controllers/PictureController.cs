using Microsoft.AspNetCore.Mvc;
using react_Api.Models;
using react_Api.ModelsDto;
using react_Api.Services;
using react_Api.ViewModels;
using SixLabors.ImageSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace react_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly PictureService pictureService;

        public PictureController(PictureService pictureService)
        {
            this.pictureService = pictureService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] int size = 0, [FromQuery] int page = 0)
        {
            var result = await pictureService.GetAll(size, page);

            for (int i = 0; i < result.Count; i++)
            {
                result[i].View = $"{Request.Scheme}://{Request.Host}" +
                   $"{Url.Action(nameof(GetPicture), "Picture", new { id = result[i].Id })}";
            }

            return Ok(result);
        }

        [HttpGet("main/{id:int}")]
        public async Task<PictureModel> GetMain([FromRoute] int id)
        {
            var picture = await pictureService.Get(id);

            picture.View = $"{Request.Scheme}://{Request.Host}" +
                    $"{Url.Action(nameof(GetPicture), "Picture", new { id = picture.Id })}";

            return picture;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPicture([FromRoute] int id)
        {
            var path = await pictureService.GetFilePath(id);

            return PhysicalFile(path, "image/jpeg");
        }

        [HttpGet("download/{id:int}")]
        public async Task<IActionResult> Download([FromRoute] int id, [FromQuery] int width, [FromQuery] int height)
        {
            var result = await pictureService.Download(id, new Size(width, height));

            return result.Match<IActionResult>(
                stream => File(stream, "image/jpeg"),
                faildSize => BadRequest(Errors.NotCorrectImage),
                notExists => BadRequest(Errors.FileNotExists)
            );
        }

        [HttpGet("tag/{id:int}")]
        public async Task<IEnumerable<PictureModel>> GetByTag([FromRoute] int id, [FromQuery] int size = 0, [FromQuery] int page = 0)
        {
            return await pictureService.GetByTag(id, size, page);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] CreatePictureModel createModel)
        {
            var result = await pictureService.Create(createModel);

            return result.Match<IActionResult>(
                picture => CreatedAtAction("Create", new { picture.Id }),
                faild => BadRequest(Errors.NotCorrectImage)
            );
        }
    }
}