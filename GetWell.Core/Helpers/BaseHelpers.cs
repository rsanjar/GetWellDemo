using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RestSharp;

namespace GetWell.Core.Helpers;

public static class BaseHelpers
{
    private const int ExifOrientationTagId = 274;

    public static string GetBaseUrl
    {
        get
        {
#if DEBUG
            return "https://localhost:44341";
#endif

#pragma warning disable CS0162
            // ReSharper disable once HeuristicUnreachableCode
            return "https://dashboard.getwell.uz";
#pragma warning restore CS0162
        }
    }

    public static async Task<bool> SendSms(string phone, string message)
    {
        if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(message))
            return false;

        phone = CleanPhone(phone);
        
        var client = new RestClient("http://185.8.212.184/smsgateway/");
        var request = new RestRequest("http://185.8.212.184/smsgateway/", Method.Post);
            
        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        request.AddParameter("login", "getwel");
        request.AddParameter("password", "Ul7awEtd3L8Lgm47TmW2");
        request.AddParameter("data", "[{\"phone\":\"" + phone + "\",\"text\":\"" + message + "\"}]");
			
        var response = await client.ExecuteAsync(request);

        var isSuccess = !(!string.IsNullOrWhiteSpace(response.Content) && response.Content.Contains("error"));

        return isSuccess;
    }

    public static string CleanPhone(string phone)
    {
        phone = phone.Replace("+", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Trim();

        if (phone.Length == 9) //998680728
            phone = $"998{phone}";
        
        return phone;
    }

    public static Image ResizeByWidth(this Image inputImage, int newWidth)
    {
        inputImage.NormalizeOrientation();

        if (inputImage.Width <= newWidth)
            return inputImage;

        var targetW = newWidth;
        var targetH = (int)(inputImage.Height * (newWidth / (float)inputImage.Width));
        
        Bitmap bmPhoto = new Bitmap(targetW, targetH, PixelFormat.Format24bppRgb);
        bmPhoto.SetResolution(inputImage.HorizontalResolution, inputImage.VerticalResolution);

        using(inputImage)
        using (Graphics grPhoto = Graphics.FromImage(bmPhoto))
        {
            grPhoto.FillRectangle(Brushes.White, 0, 0, targetW, targetH);
            grPhoto.DrawImage(inputImage, new Rectangle(0, 0, targetW, targetH), 0, 0, inputImage.Width, inputImage.Height, GraphicsUnit.Pixel);

            return bmPhoto;
        }
    }

    public static Image ResizeImage(this Image inputImage, Size size)  
    {
        inputImage.NormalizeOrientation();
        
        int sourceWidth = inputImage.Width;  
        int sourceHeight = inputImage.Height;
        float nPercentW = size.Width / (float)sourceWidth;
        float nPercentH = size.Height / (float)sourceHeight;
        float nPercent = nPercentH < nPercentW ? nPercentH : nPercentW;
        int destWidth = (int)(sourceWidth * nPercent);  
        int destHeight = (int)(sourceHeight * nPercent);

        if (destWidth > sourceWidth && destHeight > sourceHeight)
        {
            destWidth = sourceWidth;
            destHeight = sourceHeight;
        }

        Bitmap b = new Bitmap(destWidth, destHeight);

        using (Graphics g = Graphics.FromImage(b))
        {
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;  
            g.DrawImage(inputImage, 0, 0, destWidth, destHeight);  
            
            return b;  
        }
    }

    public static string ResizeBase64(string input)
    {
        byte[] bytes = Convert.FromBase64String(input);

        using (MemoryStream ms = new MemoryStream(bytes))
        using(Image image = Image.FromStream(ms))
        using(Image resizedImage = ResizeImage(image, new Size(200, 300)))
        using (MemoryStream ms2 = new MemoryStream())
        {
            resizedImage.Save(ms2, ImageFormat.Jpeg);

            string result = Convert.ToBase64String(ms2.ToArray());

            return result;
        }
    }

    public static void NormalizeOrientation(this Image image)
    {
        if (Array.IndexOf(image.PropertyIdList, ExifOrientationTagId) > -1)
        {
            int orientation = image.GetPropertyItem(ExifOrientationTagId).Value[0];

            if (orientation >= 1 && orientation <= 8)
            {
                switch (orientation)
                {
                    case 2:
                        image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case 3:
                        image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 4:
                        image.RotateFlip(RotateFlipType.Rotate180FlipX);
                        break;
                    case 5:
                        image.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case 6:
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 7:
                        image.RotateFlip(RotateFlipType.Rotate270FlipX);
                        break;
                    case 8:
                        image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }

                image.RemovePropertyItem(ExifOrientationTagId);
            }
        }
    }

    public static ImageCodecInfo GetEncoder(ImageFormat format)  
    {  
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();  
        foreach (ImageCodecInfo codec in codecs)  
        {  
            if (codec.FormatID == format.Guid)  
            {  
                return codec;  
            }  
        }  
        return null;  
    }
    
    public static async Task SaveImage(IFormFile imageFile, ImageFormat imageFormat, string filePath, Size size)
    {
        if (string.IsNullOrWhiteSpace(filePath)) 
            return;
        
        await using(var stream = new MemoryStream())
        {
            await imageFile.CopyToAsync(stream);

            using (var imageStream = Image.FromStream(stream))
            using(var image = imageStream.ResizeImage(size))
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                ImageCodecInfo imageEncoder = GetEncoder(imageFormat);
                Encoder myEncoder = Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 95L);
                myEncoderParameters.Param[0] = myEncoderParameter;

                image.Save(fileStream, imageEncoder, myEncoderParameters);
                        
                await fileStream.FlushAsync();
            }
        }
    }
}