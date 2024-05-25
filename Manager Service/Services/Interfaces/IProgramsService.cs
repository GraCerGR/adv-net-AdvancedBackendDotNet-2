﻿using Handbook_Service.Models;
using Manager_Service.Models;
using Manager_Service.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Manager_Service.Services.Interfaces
{
    public interface IProgramsService
    {
        Task<Handbook_Service.Models.ProgramPagedListModel> GetPrograms(ProgramSearchModel programSearchModel);

        Task<IActionResult> CreateQueuePrograms(Guid userId, List<Guid> programs, Guid? managerId);

        Task<IQueryable<QueueProgramsModel>> GetQueuePrograms(Guid userId);
    }
}
