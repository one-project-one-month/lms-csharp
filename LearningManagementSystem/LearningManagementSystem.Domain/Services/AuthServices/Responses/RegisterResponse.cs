using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearningManagementSystem.Domain.Services.AuthServices.Validators;
using LearningManagementSystem.Domain.Services.ResponseService;

namespace LearningManagementSystem.Domain.Services.AuthServices.Responses
{
    public class RegisterResponse()
    {
        public string? token { get; set; }
        public responseUser? user { get; set; }
        // public ApiDetails? details { get; set; }


    }
}