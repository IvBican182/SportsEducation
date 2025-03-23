namespace Aperta_web_app.Services.interfaces
{
    public interface IClubService
    {
        Task SetStripeAccountIdAsync(int clubId, string stripeAccountId);

        Task<string> GetStripeAccountIdAsync(int clubId);


    }
}
