namespace Handbook_Service.Models
{
    public class ProgramPagedListModel
    {
        public List<EducationProgramModel> Programs { get; set; }
        public PageInfoModel Pagination { get; set; }
    }
}
