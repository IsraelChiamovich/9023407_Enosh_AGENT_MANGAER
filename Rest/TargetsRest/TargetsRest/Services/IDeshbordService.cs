namespace TargetsRest.Services
{
    public interface IDeshbordService
    {
        Task<int> GetNumOfAllAgentsAsync();
        Task<int> GetNumOfAllActiveAgentsAsync();
        Task<int> GetNumOfAllTargetsAsync();
        Task<int> GetNumOfAllEliminateTargetsAsync();
        Task<int> GetNumOfAllMissionsAsync();
        Task<int> GetNumOfAllAssignedMissionsAsync();
        Task<int> GetNumOfAllRelevantMissionsAsync();

    }
}
