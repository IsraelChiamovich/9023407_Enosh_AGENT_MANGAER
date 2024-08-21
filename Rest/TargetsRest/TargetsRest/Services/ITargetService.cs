using TargetsRest.Dto;
using TargetsRest.Models;

namespace TargetsRest.Services
{
    public interface ITargetService
    {
        Task<TargetModel> CreateTargetAsync(TargetDto targetDto);
        Task<TargetModel> DeterminingAStartingPosition(int id, PossitionDto possitionDto);
        Task<TargetModel?> GetTargetByIdAsync(int id);
        Task<List<TargetModel>> GetAllTargetsAsync();
        /*Task<UserModel?> FindByEmailAsync(string email);
        Task<List<UserModel>> GetAllUsersAsync();
        Task<UserModel?> GetUserByIdAsync(int id);
        Task<UserModel?> UpdateUserAsync(int id, UserModel model);
        Task<UserModel?> DeledeUserAsync(int id);
        Task<UserModel> AuthenticateAsync(string email, string password);*/
    }
}
