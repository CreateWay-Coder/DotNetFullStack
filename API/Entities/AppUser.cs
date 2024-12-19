namespace API.Entities;

public class AppUser
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required byte[] PassportHash { get; set; }
    public required byte[] PassportSalt { get; set; }
}