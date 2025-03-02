using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.ViewModels
{
    public class CourseRequestModel
    {
        [Required]
        public int instructorId { get; set; }

        [Required]
        public int categoryId { get; set; }

        //public int socialLinkId { get; set; }

        [Required]
        public string courseName { get; set; } = string.Empty;

        [Required]
        public string type { get; set; } = string.Empty;

        [Required]
        public string level { get; set; } = string.Empty;

        public DateTime duration { get; set; }

        public DateTime createAt { get; set; }


        [Required]
        public string description { get; set; } = string.Empty;

        [Required]
        public decimal currentPrice { get; set; }


        public string thumbnail { get; set; } = string.Empty;
        public bool isAvailable { get; set; }
        public decimal? originalPrice { get; set; }

    }
}