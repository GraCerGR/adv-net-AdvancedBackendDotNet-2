using MVC.Models.Handbook;

namespace MVC.Models.Programs
{
    public class ApplicationPagedListModel
    {
        public List<ApplicationDto> Applications { get; set; }
        public PageInfoModel Pagination { get; set; }
    }
}
