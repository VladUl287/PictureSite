using Microsoft.AspNetCore.Mvc;
using react_Api.ModelsDto;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Threading.Tasks;
using react_Api.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using react_Api.Database.Models;
using SixLabors.ImageSharp.Processing;

namespace react_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly DatabaseContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public PictureController(DatabaseContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("all")]
        public async Task<IEnumerable<PictureModel>> GetAll([FromQuery] int size = 0, [FromQuery] int page = 0)
        {
            var query =
               from e in dbContext.Pictures
               select new PictureModel
               {
                   Id = e.Id,
                   View = $"{Request.Scheme}://{Request.Host}" +
                   $"{Url.Action(nameof(GetPicture), "Picture", new { id = e.Id })}"
               };

            if (size > 0 && page > 0)
            {
                int skip = size * (page - 1);
                query = query
                    .Skip(skip)
                    .Take(size);
            }

            return await query.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<PhysicalFileResult> GetPicture([FromRoute] int id)
        {
            var path = await dbContext.Pictures
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => e.Path)
                .FirstOrDefaultAsync();

            return PhysicalFile(path, "image/jpeg");
        }

        [HttpGet("download/{id:int}")]
        public async Task<ActionResult> Download([FromRoute] int id, [FromQuery] int width, [FromQuery] int height)
        {
            if (width < 640 || height < 320)
            {
                return BadRequest(new { error = "Минимальные размеры изображения 640x320" });
            }

            var path = await dbContext.Pictures
                   .AsNoTracking()
                   .Where(e => e.Id == id)
                   .Select(e => e.Path)
                   .FirstOrDefaultAsync();

            if (System.IO.File.Exists(path))
            {
                return BadRequest(new { error = "Изображение не существует" });
            }

            using var stream = new MemoryStream();
            using var img = await Image.LoadAsync(path);
            img.Mutate(op => op.Resize(width, height));
            img.Save(stream, new JpegEncoder());

            return File(stream, "image/jpeg");
        }

        [HttpPost("create")]
        public async Task Post([FromForm] CreatePictureModel pictureModel)
        {
            if (pictureModel.Image.Length <= 0)
            {
                return;
            }

            var path = $@"{webHostEnvironment.ContentRootPath}\Files\pictures\";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += pictureModel.Image.FileName;

            using var img = await Image.LoadAsync(pictureModel.Image.OpenReadStream());
            img.Save(path, new JpegEncoder());

            var picture = new Picture
            {
                Path = path
            };

            using var transaction = await dbContext.Database.BeginTransactionAsync();

            await dbContext.Pictures.AddAsync(picture);
            //await dbContext.SaveChangesAsync();

            for (int i = 0; i < pictureModel.Tags.Length; i++)
            {
                //if (pictureModel.Tags[i].Id == 0)
                //{
                //    await dbContext.Tags.AddAsync(pictureModel.Tags[i]);
                //    await dbContext.SaveChangesAsync();
                //}

                await dbContext.PicturesTags.AddAsync(new PictureTags
                {
                    PictureId = picture.Id,
                    TagId = pictureModel.Tags[i].Id
                });
            }
            await dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }
    }
}