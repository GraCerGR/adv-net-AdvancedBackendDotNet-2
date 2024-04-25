﻿using Handbook_Service.Models;
using Handbook_Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace Handbook_Service.Services.Interfaces
{
    public interface IHandbookService
    {
        Task<string> GetFaculties();
        Task<string> GetEducationLevels();
    }
}
