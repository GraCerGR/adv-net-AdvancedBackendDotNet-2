namespace MVC.Models.Handbook
{
    public class ProgramPagedListModel
    {
        public List<EducationProgramModel> Programs { get; set; }
        public PageInfoModel Pagination { get; set; }
    }
}
