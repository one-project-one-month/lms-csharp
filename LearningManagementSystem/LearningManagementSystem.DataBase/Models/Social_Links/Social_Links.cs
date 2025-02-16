using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.DataBase.Models.Social_Links
{
    public class Social_Links
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        //public Guid id { get; set; } = Guid.NewGuid();
        public Guid course_id { get; set; } = Guid.NewGuid();
        public string? facebook { get; set; }
        public string? X { get; set; }
        public string? Telegram { get; set; }
        public string? Phone { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public bool DeleteFlag { get; set; } = false;
    }
}
