using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
namespace core.Security.Steganography
{
  public class ImageContentCheckerService : IImageContentCheckerService
  {
    public const int ImageMinimumBytes = 512;
    public bool IsImage(IFormFile postedFiles)
    {
      if (postedFiles.ContentType.ToLower() != "image/jpg" &&
                  postedFiles.ContentType.ToLower() != "image/jpeg" &&
                  postedFiles.ContentType.ToLower() != "image/pjpeg" &&
                  postedFiles.ContentType.ToLower() != "image/gif" &&
                  postedFiles.ContentType.ToLower() != "image/x-png" &&
                  postedFiles.ContentType.ToLower() != "image/png")
      {
        return false;
      }
      if (Path.GetExtension(postedFiles.FileName).ToLower() != ".jpg"
                  && Path.GetExtension(postedFiles.FileName).ToLower() != ".png"
                  && Path.GetExtension(postedFiles.FileName).ToLower() != ".gif"
                  && Path.GetExtension(postedFiles.FileName).ToLower() != ".jpeg")
      {
        return false;
      }
      try
      {
        if (!postedFiles.OpenReadStream().CanRead)
        {
          return false;
        }
        if (postedFiles.Length < ImageMinimumBytes)
        {
          return false;
        }
        byte[] buffer = new byte[ImageMinimumBytes];
        postedFiles.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
        string content = System.Text.Encoding.UTF8.GetString(buffer);
        if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
        {
          return false;
        }
      }
      catch
      {
        throw;
      }
      try
      {
        using (var bitmap = new System.Drawing.Bitmap(postedFiles.OpenReadStream()))
        {
        }
      }
      catch
      {
        throw;
      }
      finally
      {
        postedFiles.OpenReadStream().Position = 0;
      }
      return true;
    }
  }
}