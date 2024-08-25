namespace TargetsRest.Services
{
    public interface IJwtService
    {
        string CreateToken(string name);
    }
}
