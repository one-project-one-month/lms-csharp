using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.DataBase.Models.Instructors
{
    public class Instructors
    {
        [Key]
        public Guid id { get; set; } = Guid.NewGuid();
        public Guid user_id { get; set; }
        public string nrc { get; set; } = null!;
        public string edu_background { get; set; } = null!;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public bool DeleteFlag { get; set; } = false;
    }
}
