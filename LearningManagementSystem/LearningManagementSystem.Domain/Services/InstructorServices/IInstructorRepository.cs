namespace LearningManagementSystem.Domain.Services.InstructorServices;

public interface IInstructorRepository
{
    InstructorViewModels CreateInstructor(InstructorViewModels reqModel);

    List<InstructorViewModels> GetInstructors();

    InstructorViewModels GetInstructorById(int id);

    bool DeleteInstructor(int id);

    InstructorViewModels UpdateInstructor(int id, InstructorViewModels instUser);
}
