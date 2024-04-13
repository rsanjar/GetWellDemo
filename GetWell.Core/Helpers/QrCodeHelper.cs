using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using QRCoder;

namespace GetWell.Core.Helpers;

public class QrCodeHelper
{
	public static string GenerateQrCodeImage(string input)
	{
		using var generator = new QRCodeGenerator();
		using var data = generator.CreateQrCode(input, QRCodeGenerator.ECCLevel.L);
		using var code = new QRCode(data);
		using var bitmap = code.GetGraphic(20, Color.Black, Color.White, null, 0);
		using (var stream = new MemoryStream())
		{
			bitmap.Save(stream, ImageFormat.Jpeg);
			stream.Position = 0;
			var imageBase64 = Convert.ToBase64String(stream.ToArray());
                        
			return imageBase64;
		}
	}

	public static string Base64Encode(string input)
	{
		return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
	}

	public static string Base64Decode(string base64EncodedData)
	{
		var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
		return Encoding.UTF8.GetString(base64EncodedBytes);
	}

	public static MemoryStream ConvertBase64ToStream(string base64EncodedData)
	{
		return new MemoryStream(Convert.FromBase64String(base64EncodedData));
	}

	public static Image ConvertBase64ToImage(string base64EncodedData)
	{
		Image image;

		using (MemoryStream ms = ConvertBase64ToStream(base64EncodedData))
		{
			image = Image.FromStream(ms);
		}

		return image;
	}
}