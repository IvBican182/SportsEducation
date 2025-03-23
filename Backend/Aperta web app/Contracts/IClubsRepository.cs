using Aperta_web_app.Data;

namespace Aperta_web_app.Contracts
{
    //inherits GenericRepository tasks, but we can also add specific tasks for "Clubs" only
    public interface IClubsRepository : IGenericRepository<Club>
    {
        Task<Club> GetClubDetails(int id);

        Task UpdateClubBillingInfo(int id, bool billingStatus);
    }
}
