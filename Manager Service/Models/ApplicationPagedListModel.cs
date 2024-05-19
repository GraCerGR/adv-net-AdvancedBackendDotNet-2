using Manager_Service.Models.DTO;

namespace Manager_Service.Models
{
    public class ApplicationPagedListModel
    {
        public List<ApplicationDto> Applications { get; set; }
        public PageInfoModel Pagination { get; set; }
    }
}
