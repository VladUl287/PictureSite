using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using OneOf;
using react_Api.Database;
using react_Api.Database.Models;
using react_Api.Infrastructure.Extensions;
using react_Api.Models;
using react_Api.ModelsDto;
using react_Api.Services.Contract;
using react_Api.ViewModels;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace react_Api.Services
{
    public class PictureService : IPictureService
    {
        private readonly string path;
        private readonly DatabaseContext dbContext;

        public PictureService(DatabaseContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;

            path = $@"{webHostEnvironment.ContentRootPath}\Files\pictures\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public async Task<List<PictureModel>> GetAll(int size = 0, int page = 0)
        {
            var query =
               from e in dbContext.Pictures
               select new PictureModel
               {
                   Id = e.Id
               };

            if (size > 0 && page > 0)
            {
                int skip = size * (page - 1);
                query = query
                    .Skip(skip)
                    .Take(size);
            }

            return await query
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<PictureModel> Get(int id)
        {
            var query = from e in dbContext.Pictures
                        select new PictureModel
                        {
                            Id = e.Id,
                            View = e.Name,
                            OriginalHeight = e.OriginalHeight,
                            OriginalWidth = e.OriginalWidth,
                            Vertical = e.Vertical,
                            Tags = e.PicturesTags
                                     .Select(e => e.Tag)
                                     .ToList()
                        };

            return await query
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<PictureModel>> GetByTag(int id, int size = 0, int page = 0)
        {
            var query = from e in dbContext.PicturesTags
                        where e.TagId == id
                        select new PictureModel
                        {
                            Id = e.PictureId
                        };

            if (size > 0 && page > 0)
            {
                int skip = size * (page - 1);
                query = query
                    .Skip(skip)
                    .Take(size);
            }

            return await query
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<OneOf<Stream, NotCorrectSize, FileNotExists>> Download(int id, Size size)
        {
            if (size.Width < 0 || size.Height < 0)
            {
                return new NotCorrectSize();
            }

            var name = await dbContext.Pictures
                .Where(e => e.Id == id)
                .Select(e => new { e.Name, e.OriginalWidth, e.OriginalHeight })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (name is null)
            {
                return new FileNotExists();
            }

            if (!File.Exists(path + name))
            {
                return new FileNotExists();
            }

            return await LoadImage(path + name, size.Width, size.Height);
        }

        public async Task<OneOf<Picture, NotCorrectData>> Create(CreatePictureModel createModel)
        {
            if (!createModel.Picture.IsImage())
            {
                return new NotCorrectData();
            }

            var fileName = createModel.Picture.FileName;

            using var img = await Image.LoadAsync(createModel.Picture.OpenReadStream());
            img.Save(path + fileName, new JpegEncoder());

            var picture = new Picture
            {
                Name = fileName,
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

            return picture;
        }

        public async Task<string> GetFilePath(int id)
        {
            var name = await dbContext.Pictures
                .Where(e => e.Id == id)
                .Select(e => e.Name)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return path + name;
        }

        private static async Task<Stream> LoadImage(string path, int width, int height)
        {
            using var img = await Image.LoadAsync(path);

            if (img.Width != width || img.Height != height)
            {
                img.Mutate(op => op.Resize(width, height));
            }

            var stream = new MemoryStream();
            img.Save(stream, new JpegEncoder());
            stream.Position = 0;

            return stream;
        }
    }
}