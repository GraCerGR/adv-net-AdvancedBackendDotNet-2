using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Manager_Service.Models
{
    public class QueueProgramsModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public List<Guid> Queue { get; set; }


    }
}
