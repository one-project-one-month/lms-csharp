using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.ViewModels
{
    public class CategoryViewModels
    {
        public string Category_Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public bool DeleteFlag { get; set; } = false;
    }
}
