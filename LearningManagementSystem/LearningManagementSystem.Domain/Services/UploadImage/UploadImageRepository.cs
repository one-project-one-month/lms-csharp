namespace LearningManagementSystem.Domain.Services.UploadImage;

public class UploadImageRepository : IUploadImageRepository
{
    public string UploadImage(IFormFile file)
    {
        var filePath = "";
        if (file is not null && file.Length > 0)
        {
            filePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadImages", file.FileName);

            var uploadDirectory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory!);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }
        return filePath;
    }
}
