using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Services.AuthServices.Requests
{
    public class RefreshTokenRequest
    {
        public string? token { get; set; }
        public int? id { get; set; }

    }
}