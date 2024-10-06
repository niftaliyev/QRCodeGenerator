using Microsoft.AspNetCore.Mvc;
using QRCoder;
using SixLabors.ImageSharp;

namespace MyWebApp.Controllers;

public class QRCodeController : Controller
{
    [HttpGet]
    public IActionResult Index(string text=null)
    {
        if (text != null)
        {
            var qrCodeText = text;
            using (var qrCodeStream = GenerateQRCode(qrCodeText))
            {
                string base64Image = Convert.ToBase64String(qrCodeStream.ToArray());
                ViewBag.QRCodeImage = $"data:image/png;base64,{base64Image}";
            }
        }
        return View();
    }
    public MemoryStream GenerateQRCode(string text)
    {
        using (var qrGenerator = new QRCodeGenerator())
        {
            using (var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q))
            using (var qrCode = new QRCode(qrCodeData))
            {
                using (var bitmap = qrCode.GetGraphic(20))
                {
                    using (var ms = new MemoryStream())
                    {
                        // Save the QR code as a PNG image
                        bitmap.SaveAsPng(ms);
                        ms.Position = 0; // Reset the stream position
                        return ms;
                    }
                }
            }
        }
    }
}
