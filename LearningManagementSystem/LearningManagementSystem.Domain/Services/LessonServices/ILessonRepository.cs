using LearningManagementSystem.Domain.Models;
using LearningManagementSystem.Domain.ViewModels;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Services.LessonServices
{
    public interface ILessonRepository
    {
        Result<LessonResponseModel> CreateLesson(LessonViewModel lesson);

        List<LessonViewModel> GetLessons();

        LessonViewModel? GetLessonById(int id);

        List<LessonViewModel> GetLessonsByCourseId(int courseId);

        LessonViewModel? UpdateLesson(int id, LessonViewModel lesson);

        LessonViewModel? PatchLesson(int id, LessonViewModel lesson);

        bool DeleteLesson(int id);
    }
}
