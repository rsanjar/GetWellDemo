using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using GetWell.Core.Enums;
using GetWell.Core.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace GetWell.Dashboard.Controllers;

public class BaseImageController : BaseController
{
    #region ctor

    private readonly IWebHostEnvironment _webHostEnvironment;

    public BaseImageController(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    #endregion

    protected async Task<string> SaveJpegImage(int fileID, IFormFile imageFile, Size size, ImageFolderEnum folder, int parentFileID = 0)
    {
        return await SaveImage(fileID, imageFile, ImageFormat.Jpeg, size, folder, parentFileID);
    }

    protected async Task<string> SavePngImage(int fileID, IFormFile imageFile, Size size, ImageFolderEnum folder, int parentFileID = 0)
    {
        return await SaveImage(fileID, imageFile, ImageFormat.Png, size, folder, parentFileID);
    }

    protected async Task<string> SaveImage(int fileID, IFormFile imageFile, ImageFormat imageFormat, Size size, ImageFolderEnum folder, int parentFileID = 0)
    {
        var file = GetImageFilePath(fileID, imageFile, imageFormat, folder, parentFileID);

        if (string.IsNullOrWhiteSpace(file.filePath)) 
            return null;
        
        await using(var stream = new MemoryStream())
        {
            await imageFile.CopyToAsync(stream);

            using (var imageStream = Image.FromStream(stream))
            using(var image = imageStream.ResizeImage(size))
            await using (var fileStream = new FileStream(file.filePath, FileMode.Create))
            {
                ImageCodecInfo imageEncoder = BaseHelpers.GetEncoder(imageFormat);
                Encoder myEncoder = Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 95L);
                myEncoderParameters.Param[0] = myEncoderParameter;

                image.Save(fileStream, imageEncoder, myEncoderParameters);
                        
                await fileStream.FlushAsync();
            }
        }

        return file.fileUrl;
    }

    private (string filePath, string fileUrl) GetImageFilePath(int fileID, IFormFile imageFile, ImageFormat format, ImageFolderEnum folder, int parentFileID = 0)
    {
        string imageFolder = Path.Combine(_webHostEnvironment.WebRootPath, $"assets\\images\\{folder}");
        var fileName = string.Empty;

        if (fileID <= 0 || imageFile == null)
            return (fileName, fileName);

        var id = Guid.NewGuid();

        fileName = $"{fileID}_{id}.{format}";

        if(parentFileID > 0)
            fileName = $"{parentFileID}_{fileID}_{id}.{format}";

        string fileUrl = $"{BaseHelpers.GetBaseUrl}/assets/images/{folder}/{fileName}".ToLower();
        string filePath = Path.Combine(imageFolder, fileName).ToLower();

        return (filePath, fileUrl);
    }
}