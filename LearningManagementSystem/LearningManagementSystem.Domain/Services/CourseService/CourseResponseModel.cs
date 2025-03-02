using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Services.CourseService
{
    public class CourseResponseModel
    {
        public int id { get; set; }
        public string courseName { get; set; } = string.Empty;
        public string thumbnail { get; set; } = string.Empty;
        public bool isAvailable { get; set; }
        public string type { get; set; } = string.Empty;
        public DateTime duration { get; set; } 
        public string level { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public decimal currentPrice { get; set; }
        public decimal? originalPrice { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime? updatedAt { get; set; }

        public string instructorName { get; set; } = string.Empty;
        public string educationBackgroun { get; set; } = string.Empty;
        public string categoryName { get; set; } = string.Empty;

        public string  facebook {  get; set; } = string.Empty;

    }

}