using Manager_Service.Context;
using Manager_Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Manager_Service.Models.DTO;
using Manager_Service.Models;
using User_Service.Models;

namespace Manager_Service.Services
{
    public class ProgramsService : IProgramsService
    {
        private readonly ApplicationContext _context;
        private readonly Handbook_Service.Context.ApplicationContext _contextH;
        private readonly User_Service.Context.ApplicationContext _contextU;

        public ProgramsService(ApplicationContext context, Handbook_Service.Context.ApplicationContext contextH, User_Service.Context.ApplicationContext contextU)
        {
            _context = context;
            _contextH = contextH;
            _contextU = contextU;
        }

        public async Task<Handbook_Service.Models.ProgramPagedListModel> GetPrograms(ProgramSearchModel programSearchModel)
        {
            IQueryable<Handbook_Service.Models.EducationProgramModel> query = _contextH.EducationPrograms as IQueryable<Handbook_Service.Models.EducationProgramModel>;

            if (!string.IsNullOrEmpty(programSearchModel.Faculty))
            {
                query = query.Where(p => p.Faculty.Name.ToLower().Contains(programSearchModel.Faculty.ToLower()));
            }

            if (!string.IsNullOrEmpty(programSearchModel.EducationLevel))
            {
                query = query.Where(p => p.EducationLevel.Name.ToLower().Contains(programSearchModel.EducationLevel.ToLower()));
            }

            if (!string.IsNullOrEmpty(programSearchModel.EducationForm))
            {
                query = query.Where(p => p.EducationForm.ToLower().Contains(programSearchModel.EducationForm.ToLower()));
            }

            if (!string.IsNullOrEmpty(programSearchModel.Language))
            {
                query = query.Where(p => p.Language.ToLower().Contains(programSearchModel.Language.ToLower()));
            }

            if (!string.IsNullOrEmpty(programSearchModel.Code))
            {
                query = query.Where(p => p.Code.ToLower().Contains(programSearchModel.Code.ToLower()));
            }

            if (!string.IsNullOrEmpty(programSearchModel.Name))
            {
                query = query.Where(p => p.Name.ToLower().Contains(programSearchModel.Name.ToLower()));
            }

            int page = programSearchModel.Page;
            int size = programSearchModel.Size;
            var programs = new List<Handbook_Service.Models.EducationProgramModel>();

            if (query != null)
            {
                programs = await query.Skip((page - 1) * size).Take(size).ToListAsync();
            }

            var programPagedListModel = new Handbook_Service.Models.ProgramPagedListModel
            {
                Programs = programs,
                Pagination = new Handbook_Service.Models.PageInfoModel
                {
                    Size = size,
                    Count = (query.Count()/size) == 0 ? 1 : (query.Count() / size),
                    Current = page
                }
            };

            return programPagedListModel;
        }

        public async Task CreateQueuePrograms(Guid userId, List<Guid> programs)
        {
            //Проверка существования пользователя с userId
            var user = await _contextU.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId.ToString());
            if (user == null)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status401Unauthorized.ToString(), "User not exists");
                throw ex;
            }
            //Проверка существования всех программ с List<Guid> programs
            foreach (var programId in programs)
            {
                var existingProgramId = await _contextH.EducationPrograms.FirstOrDefaultAsync(u => u.Id == programId);
                if (existingProgramId == null)
                {
                    var ex = new Exception();
                    ex.Data.Add(StatusCodes.Status404NotFound.ToString(), $"Program with ID {programId} not found");
                    throw ex;
                }
            }

            _context.QueuePrograms.RemoveRange(_context.QueuePrograms.Where(qp => qp.UserId == userId));
            await _context.SaveChangesAsync();

            QueueProgramsModel queue = new QueueProgramsModel
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Queue = programs
            };

            try
            {
                await _context.QueuePrograms.AddAsync(queue);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    Console.WriteLine("Inner Exception: " + innerException.Message);
                    innerException = innerException.InnerException;
                }
            }
        }

        public async Task<IQueryable<QueueProgramsModel>> GetQueuePrograms(Guid userId)
        {
            //Проверка существования пользователя с userId
            var user = await _contextU.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId.ToString());
            if (user == null)
            {
                var ex = new Exception();
                ex.Data.Add(StatusCodes.Status401Unauthorized.ToString(), "User not exists");
                throw ex;
            }

            IQueryable<QueueProgramsModel> query = _context.QueuePrograms;
            return query.Where(p => p.UserId == userId);

        }
    }
}
