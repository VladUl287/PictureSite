using OneOf;
using react_Api.Database.Models;
using react_Api.Models;
using react_Api.ModelsDto;
using react_Api.ViewModels;
using SixLabors.ImageSharp;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace react_Api.Services.Contract
{
    public interface IPictureService
    {
        Task<PictureModel> Get(int id);

        Task<string> GetFilePath(int id);

        Task<List<PictureModel>> GetAll(int size = 0, int page = 0);

        Task<IEnumerable<PictureModel>> GetByTag(int id, int size = 0, int page = 0);

        Task<OneOf<Picture, NotCorrectData>> Create(CreatePictureModel createModel);

        Task<OneOf<Stream, NotCorrectSize, FileNotExists>> Download(int id, Size size);
    }
}