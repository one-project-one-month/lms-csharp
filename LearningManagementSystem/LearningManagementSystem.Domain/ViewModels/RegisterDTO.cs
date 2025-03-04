using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.ViewModels
{
    public class RegisterDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateOnly DOB { get; set; }
        public string Address { get; set; }
        public string ProfilePhoto { get; set; }
        public int RoleId { get; set; }
    }
}
