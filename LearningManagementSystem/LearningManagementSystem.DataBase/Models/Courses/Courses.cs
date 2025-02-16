using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.DataBase.Models.Courses
{
    public class Courses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        //public Guid id { get; set; } = Guid.NewGuid();
        public Guid instructors_id { get; set; }
        public string courseName { get; set; } = null!;
        public string thumbnail { get; set; } = null!;
        public bool is_available { get; set; } = false;
        public string type { get; set; } = null!;
        public string level { get; set; } = null!;
        public string description { get; set; } = null!;
        public DateTime duration { get; set; }
        public decimal original_price { get; set; } 
        public decimal current_price { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public bool DeleteFlag { get; set; } = false;
    }
}
