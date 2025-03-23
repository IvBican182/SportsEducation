using Aperta_web_app.Contracts;
using Aperta_web_app.Data;
using Microsoft.EntityFrameworkCore;

namespace Aperta_web_app.Repository
{
    public class ClubsRepository : GenericRepository<Club>, IClubsRepository
    {
        private readonly AppDbContext _context;
        public ClubsRepository(AppDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<Club> GetClubDetails(int id)
        {
           return await _context.Clubs.FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task UpdateClubBillingInfo(int id, bool billingStatus)
        {
            var club = await _context.Clubs.FirstOrDefaultAsync(q => q.Id == id);

            if(club == null)
            {
                throw new Exception("Club not found!");
            }

            club.BillingInfo = billingStatus;

            await _context.SaveChangesAsync();
        }
    }
}























