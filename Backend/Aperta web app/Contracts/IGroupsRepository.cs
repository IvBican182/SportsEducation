using Aperta_web_app.Data;

namespace Aperta_web_app.Contracts
{
    public interface IGroupsRepository : IGenericRepository<Group>
    {
        Task<Group> GetGroupDetails(int id);
    }
}
