namespace UsersModule.Domain.Entities;
public class User : EntityBase
{
    public string Name { get; set; } =  string.Empty;
    public string Email { get; set; } =  string.Empty;
    public bool EmailConfirmed { get; set; }
    public string Password { get; set; } =  string.Empty;
    public Guid UserId { get; set; }
}