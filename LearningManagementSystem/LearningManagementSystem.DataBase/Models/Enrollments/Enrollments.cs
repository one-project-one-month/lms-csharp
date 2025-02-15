using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.DataBase.Models.Enrollments
{
    public class Enrollments
    {
        [Key]
        public Guid id { get; set; } = Guid.NewGuid();
        public Guid user_id { get; set; } = Guid.NewGuid();
        public Guid course_id { get; set; } = Guid.NewGuid();
        public DateTime enrollment_date { get; set; } = DateTime.UtcNow;
        public bool is_completed { get; set; } = false;
        public DateTime? completed_date { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public bool DeleteFlag { get; set; } = false;
    }
}
