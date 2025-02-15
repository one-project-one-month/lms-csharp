using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.DataBase.Models.Lessons
{
    public class Lessons
    {
        [Key]
        public Guid id { get; set; } = Guid.NewGuid();
        public Guid course_id { get; set; } = Guid.NewGuid();
        public string title { get; set; } = null!;
        public string videoUrl { get; set; } = null!;
        public string lessonDetail { get; set; } = null!;
        public bool is_available { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public bool DeleteFlag { get; set; } = false;
    }
}
