using Aperta_web_app.Data;
using Aperta_web_app.Services.interfaces;

namespace Aperta_web_app.Services.Implementations
{
    public class ClubService : IClubService
    {
        private readonly AppDbContext _dbcontext;
        public ClubService(AppDbContext dbcontext) 
        { 
            _dbcontext = dbcontext;
        }

        public async Task SetStripeAccountIdAsync(int clubId, string stripeAccountId)
        {
            var club = await _dbcontext.Clubs.FindAsync(clubId);

            if (club == null) 
            {
                throw new Exception("Club not found");
            }

            club.StripeId = stripeAccountId;
            await _dbcontext.SaveChangesAsync();
        }

        public async Task<string> GetStripeAccountIdAsync(int clubId) 
        { 
            var club = await _dbcontext.Clubs.FindAsync(clubId);

            if (club == null)
            {
                throw new Exception("Club not found");
            }

            return club.StripeId;
        }
    }
}
