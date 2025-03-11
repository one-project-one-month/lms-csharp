namespace LearningManagementSystem.Domain.ViewModels;

public class LessonViewModel
{

    [JsonIgnore]
    public int id { get; set; }

    public int course_id { get; set; }

    public string title { get; set; } = null!;

    public string? videoUrl { get; set; }

    public string lessonDetail { get; set; } = null!;

    public bool is_available { get; set; } = false;

    public DateTime created_at { get; set; }

    public DateTime? updated_at { get; set; }

    [JsonIgnore]
    public bool isDeleted { get; set; } = false;


}
