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
using Microsoft.AspNetCore.Http;
using System;

namespace react_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private readonly string path;
        private readonly DatabaseContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public PictureController(DatabaseContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;

            path = $@"{webHostEnvironment.ContentRootPath}\Files\pictures\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
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

        [HttpGet("main/{id:int}")]
        public async Task<PictureModel> GetMain([FromRoute] int id)
        {
            return await dbContext.Pictures
                .Where(e => e.Id == id)
                .Select(e => new PictureModel
                {
                    Id = e.Id,
                    View = $"{Request.Scheme}://{Request.Host}" +
                    $"{Url.Action(nameof(GetPicture), "Picture", new { id = e.Id })}",
                    OriginalWidth = e.OriginalWidth,
                    OriginalHeight = e.OriginalHeight,
                    Tags = e.PicturesTags
                        .Select(e => e.Tag)
                        .ToList()
                })
                .FirstOrDefaultAsync(); ;
        }

        [HttpGet("{id:int}")]
        public async Task<PhysicalFileResult> GetPicture([FromRoute] int id)
        {
            var fileName = await dbContext.Pictures
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => e.Path)
                .FirstOrDefaultAsync();

            return PhysicalFile(path + fileName, "image/jpeg");
        }

        [HttpGet("download/{id:int}")]
        public async Task<ActionResult> Download([FromRoute] int id, [FromQuery] int width, [FromQuery] int height)
        {
            if (width < 0 || height < 0)
            {
                return BadRequest(new ErrorModel("Некорректные размеры изображения."));
            }

            var name = await dbContext.Pictures
                   .AsNoTracking()
                   .Where(e => e.Id == id)
                   .Select(e => e.Path)
                   .FirstOrDefaultAsync();

            if (!System.IO.File.Exists(name))
            {
                if (name is not null)
                {
                    await dbContext.Database.ExecuteSqlInterpolatedAsync($"DELETE FROM [Pictures] WHERE [Id] = {id}");
                }

                return BadRequest(new ErrorModel("Изображение не существует."));
            }

            using var img = await Image.LoadAsync(path + name);
            if (img.Width != width)
            {
                img.Mutate(op => op.Resize(width, height));
            }
            var stream = new MemoryStream();
            img.Save(stream, new JpegEncoder());
            stream.Position = 0;

            return File(stream, "image/jpeg");
        }

        [HttpGet("tag/{id:int}")]
        public async Task<IEnumerable<PictureModel>> GetByTag([FromRoute] int id, [FromQuery] int size = 0, [FromQuery] int page = 0)
        {
            var query = from e in dbContext.PicturesTags
                        where e.TagId == id
                        select new PictureModel
                        {
                            Id = e.PictureId,
                            View = $"{Request.Scheme}://{Request.Host}" +
                            $"{Url.Action(nameof(GetPicture), "Picture", new { id = e.PictureId })}"
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

        [HttpPost("create")]
        public async Task<IActionResult> Post([FromForm] CreateModel createModel)
        {
            if (!createModel.Picture.IsImage())
            {
                return BadRequest(new ErrorModel("Ошибка изображения."));
            }

            var fileName = createModel.Picture.FileName;

            using var img = await Image.LoadAsync(createModel.Picture.OpenReadStream());
            img.Save(path + fileName, new JpegEncoder());

            var picture = new Picture
            {
                Path = fileName,
                OriginalWidth = img.Width,
                OriginalHeight = img.Height,
                Vertical = createModel.Vertical
            };

            using var transaction = await dbContext.Database.BeginTransactionAsync();

            await dbContext.Pictures.AddAsync(picture);
            await dbContext.SaveChangesAsync();

            for (int i = 0; i < createModel.Tags.Length; i++)
            {
                if (createModel.Tags[i].Id == 0)
                {
                    await dbContext.Tags.AddAsync(createModel.Tags[i]);
                    await dbContext.SaveChangesAsync();
                }

                await dbContext.PicturesTags.AddAsync(new PictureTags
                {
                    PictureId = picture.Id,
                    TagId = createModel.Tags[i].Id
                });
            }

            await dbContext.SaveChangesAsync();

            await transaction.CommitAsync();

            //return CreatedAtAction(nameof(GetPicture), new { id = picture.Id }, picture);
            return Ok();
        }
    }

    public static class FormFileExtensions
    {
        public const int ImageMinimumBytes = 512;

        public static bool IsImage(this IFormFile postedFile)
        {
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                        postedFile.ContentType.ToLower() != "image/jpeg" &&
                        postedFile.ContentType.ToLower() != "image/pjpeg" &&
                        postedFile.ContentType.ToLower() != "image/gif" &&
                        postedFile.ContentType.ToLower() != "image/x-png" &&
                        postedFile.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
            {
                return false;
            }

            try
            {
                if (!postedFile.OpenReadStream().CanRead)
                {
                    return false;
                }

                if (postedFile.Length < ImageMinimumBytes)
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }

            //try
            //{
            //    using (var bitmap = new System.Drawing.Bitmap(postedFile.OpenReadStream()))
            //    {
            //    }
            //}
            //catch (Exception)
            //{
            //    return false;
            //}
            //finally
            //{
            //    postedFile.OpenReadStream().Position = 0;
            //}

            return true;
        }
    }
}