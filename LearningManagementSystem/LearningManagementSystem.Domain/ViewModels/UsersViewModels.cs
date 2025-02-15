using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.ViewModels
{
    public class UsersViewModels
    {
        public string username { get; set; } = null!;
        public string email { get; set; } = null!;
        public string password { get; set; } = null!;
        public string phone { get; set; } = null!;
        public DateOnly dob { get; set; }
        public string address { get; set; } = null!;
        public string profile_photo { get; set; } = null!;
        public string role_id { get; set; } = null!; // Validation Required for allow only student or instructor
        public bool is_available { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
    }
}
