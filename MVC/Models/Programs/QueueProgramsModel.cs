using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace MVC.Models.Programs
{
    public class QueueProgramsModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public List<Guid> Queue { get; set; }


    }
}
