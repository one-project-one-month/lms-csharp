namespace LearningManagementSystem.Domain.Services.Social_linksService;

public interface ISocial_linksRepository
{
    Task<Social_linksViewModels> CreateSocial_links(Social_linksViewModels requests);
    Task<bool?> DeleteSocial_links(int id);
    Task<List<Social_linksViewModels>> GetSocial_links();
    Task<List<Social_linksViewModels>> GetSocial_linksById(int id);
    Task<Social_linksViewModels> PatchSocial_links(int id, Social_linksViewModels requests);
    Task<Social_linksViewModels> UpdateSocial_links(int id, Social_linksViewModels requests);
}