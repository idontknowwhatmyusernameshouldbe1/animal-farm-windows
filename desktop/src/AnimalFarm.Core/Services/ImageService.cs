using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using AnimalFarm.Core.Storage;

namespace AnimalFarm.Core.Services;

public sealed class ImageService
{
    private const int MaxSide = 1600;
    private const long JpegQuality = 82L;

    private readonly AppPaths _paths;

    public ImageService(AppPaths paths)
    {
        _paths = paths;
    }

    public string SaveImportedImage(string sourcePath, string petId)
    {
        _paths.EnsureCreated();

        var destination = Path.Combine(_paths.ImagesDirectory, $"{petId}.jpg");
        using var source = Image.FromFile(sourcePath);
        using var resized = Resize(source, MaxSide);
        SaveJpeg(resized, destination, JpegQuality);
        return destination;
    }

    public void DeleteImage(string? imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
        {
            return;
        }

        try
        {
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
        catch (IOException)
        {
            // Ignore locked/missing files during delete.
        }
        catch (UnauthorizedAccessException)
        {
            // Ignore permission issues during delete.
        }
    }

    private static Image Resize(Image source, int maxSide)
    {
        var scale = Math.Min(1d, maxSide / (double)Math.Max(source.Width, source.Height));
        var width = Math.Max(1, (int)Math.Round(source.Width * scale));
        var height = Math.Max(1, (int)Math.Round(source.Height * scale));

        if (width == source.Width && height == source.Height)
        {
            return new Bitmap(source);
        }

        var bitmap = new Bitmap(width, height);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        graphics.DrawImage(source, 0, 0, width, height);
        return bitmap;
    }

    private static void SaveJpeg(Image image, string path, long quality)
    {
        var encoder = ImageCodecInfo
            .GetImageEncoders()
            .First(codec => codec.FormatID == ImageFormat.Jpeg.Guid);

        using var parameters = new EncoderParameters(1);
        parameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
        image.Save(path, encoder, parameters);
    }
}
