
namespace LearningManagementSystem.Domain.Services.EnrollmentService;

public interface IEnrollmentRepository
{
    Task<EnrollmentViewModels> CreateEnrollment(EnrollmentViewModels request);
    Task<bool?> DeleteEnrollment(int id);
    Task<List<EnrollmentViewModels>> GetEnrollment(int id);
    Task<List<EnrollmentViewModels>> GetEnrollments();
    Task<EnrollmentViewModels> PatchEnrollment(int id, EnrollmentViewModels request);
    Task<EnrollmentViewModels> UpdateEnrollment(int id, EnrollmentViewModels request);
}