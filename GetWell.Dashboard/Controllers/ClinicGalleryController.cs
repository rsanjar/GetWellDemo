using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GetWell.Core;
using GetWell.Core.Enums;
using GetWell.Core.Helpers;
using GetWell.Dashboard.ViewComponents;
using GetWell.DTO;
using GetWell.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GetWell.Dashboard.Controllers
{
    [Authorize(Roles = UserRoles.AdminOrClinic)]
    public class ClinicGalleryController : BaseController
    {
        #region ctor

        private readonly IClinicGalleryService _clinicGalleryService;
        private readonly IClinicService _clinicService;
        private readonly ILogger<ClinicGalleryController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ClinicGalleryController(IClinicGalleryService clinicGalleryService, 
            IClinicService clinicService,
            ILogger<ClinicGalleryController> logger,
            IWebHostEnvironment webHostEnvironment)
        {
            _clinicGalleryService = clinicGalleryService;
            _clinicService = clinicService;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        #endregion

        [HttpGet("[controller]")]
        [HttpGet("[controller]/{clinicID:int}")]
        public async Task<ActionResult> Index(int clinicID)
        {
            var result = await _clinicService.GetAsync(User.ID(clinicID));

            return View(result);
        }
        
        public async Task<ActionResult> Gallery(int clinicID)
        {
            return await Task.FromResult(ViewComponent(typeof(ClinicGalleryPhotosViewComponent), new { clinicID = User.ID(clinicID) }));
        }

        [HttpGet("[controller]/upload/")]
        [HttpGet("[controller]/{clinicID:int}/upload/")]
        public async Task<ActionResult> Upload(int clinicID)
        {
            return await Task.FromResult(View(new ClinicGallery { ClinicID = User.ID(clinicID)}));
        }

        [HttpPost]
        public async Task<ContentResult> UploadImage(ClinicGallery item)
        {
            if(item.Image == null)
                ModelState.AddModelError(nameof(item.Image), "Image cannot be null");

            if (!ModelState.IsValid)
                return await ContentResultAsync(Crud.ValidationError);

            if (item.ClinicID != User.ID(item.ClinicID))
                return await ContentResultAsync(Crud.AccessDeniedError);

            await SaveImage(item);

            var result = await _clinicGalleryService.SaveAsync(item);

            return await ContentResultAsync(result);
        }

        [HttpPost]
        public async Task<ContentResult> Delete(int id)
        {
            var item = await _clinicGalleryService.GetAsync(id);

            if (item == null || item.ID <= 0)
                return await ContentResultAsync(new CrudResponse(Crud.ItemNotFoundError));

            if (item.ClinicID != User.ID(item.ClinicID))
                return await ContentResultAsync(Crud.AccessDeniedError);

            var result = await _clinicGalleryService.DeleteAsync(id);

            if (result.IsSuccess)
            {
                var file = GetImageFilePath(item, true);
                System.IO.File.Delete(file);
            }

            return await ContentResultAsync(result);
        }

        #region Private Methods

        private string GetImageFilePath(ClinicGallery item, bool getOnlyPath = false)
        {
            if (item.Image == null && !getOnlyPath)
                return null;

            string responsiveFolder = item.IsMobileImage ? "mobile" : "desktop";
            string imageFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets\\images\\clinics", responsiveFolder, User.ID(item.ClinicID).ToString());
            string filePath = null;

            if (!Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);
            
            if (item.ID > 0 && !string.IsNullOrWhiteSpace(item.ImageUrl))
                item.ImageFileName = item.ImageUrl.Split('/').Last();
            
            if(item.Image != null)
                item.ImageFileName = $"{Guid.NewGuid()}{Path.GetExtension(item.Image.FileName)}".ToLower();

            if(User.ID(item.ClinicID) > 0)
                item.ImageUrl = $"{BaseHelpers.GetBaseUrl}/assets/images/clinics/{responsiveFolder}/{User.ID(item.ClinicID)}/{item.ImageFileName}";

            if(!string.IsNullOrWhiteSpace(item.ImageFileName))
                filePath = Path.Combine(imageFolder, item.ImageFileName);

            return filePath;
        }

        private async Task SaveImage(ClinicGallery item)
        {
            string filePath = GetImageFilePath(item);

            if (filePath == null)
                return;

            try
            {
                await using(var stream = new MemoryStream())
                {
                    await item.Image.CopyToAsync(stream);
                    using (var imageStream = Image.FromStream(stream))
                    using(var image = imageStream.ResizeByWidth(1920))
                    await using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.Save(fileStream, ImageFormat.Jpeg);
                        
                        await fileStream.FlushAsync();
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error while saving the clinic gallery image");
            }
        }
        
        #endregion
    }
}