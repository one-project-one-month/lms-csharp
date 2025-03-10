using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningManagementSystem.Domain.Services.AuthServices.Responses
{
    public class RefreshTokenResponse
    {
        public string? refreshToken { get; set; }
        public string? id { get; set; }
    }
}