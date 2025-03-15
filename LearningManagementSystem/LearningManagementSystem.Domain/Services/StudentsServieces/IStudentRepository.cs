namespace LearningManagementSystem.Domain.Services.StudentsServieces;

public interface IStudentRepository
{
    StudentsViewModels CreateStudent(StudentsViewModels student);

    List<StudentsViewModels> GetStudents();

    StudentsViewModels GetStudent(int id);

    bool DeleteStudent(int id);

    StudentsViewModels UpdateStudent(int id, StudentsViewModels student);
}
