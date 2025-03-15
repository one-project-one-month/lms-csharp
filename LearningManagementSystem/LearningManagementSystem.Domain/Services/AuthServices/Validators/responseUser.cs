using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Services.AuthServices.Validators
{
    public class responseUser
    {
        public int id { get; set; }
        public string? username { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public DateOnly dob { get; set; }
        public string? address { get; set; }
        public string? profile_photo { get; set; }
        public string? role { get; set; }
        // public string? token { get; set; }
    }
}