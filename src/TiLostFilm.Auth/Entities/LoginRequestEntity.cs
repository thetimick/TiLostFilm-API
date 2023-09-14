namespace TiLostFilm.Auth.Entities;

public record LoginRequestEntity
{
    public string Login { get; set; }
    public string Password { get; set; }
    
    public LoginRequestEntity(string login, string password)
    {
        Login = login;
        Password = password;
    }
};