using TargetsRest.Dto;
using TargetsRest.Models;

namespace TargetsRest.Services
{
    public interface ITargetService
    {
        Task<int> CreateTargetAsync(TargetDto targetDto);
        Task<TargetModel> DeterminingAStartingPosition(int id, PositionDto possitionDto);
        Task<TargetModel?> GetTargetByIdAsync(int id);
        Task<List<TargetModel>> GetAllTargetsAsync();
        Task<TargetModel?> MoveTarget(int targetId, MoveDto direction);

        //Task<UserModel> AuthenticateAsync(string email, string password);
    }
}
