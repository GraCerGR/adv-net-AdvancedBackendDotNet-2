namespace MVC.Models
{
public class ManagerDto
{
    public Guid Id { get; set; }
    public UserDto UserId { get; set; }
    public bool MainManager { get; set; }
}
}