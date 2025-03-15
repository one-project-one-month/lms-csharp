namespace LearningManagementSystem.Domain.Services.UploadImage;

public interface IUploadImageRepository
{
    string UploadImage(IFormFile file);
}
