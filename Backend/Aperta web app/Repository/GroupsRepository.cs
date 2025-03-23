using Aperta_web_app.Contracts;
using Aperta_web_app.Data;
using Microsoft.EntityFrameworkCore;

namespace Aperta_web_app.Repository
{
    public class GroupsRepository : GenericRepository<Group>, IGroupsRepository
    {
        private readonly AppDbContext _context;
        public GroupsRepository(AppDbContext context) : base(context)
        {
            this._context = context;
            
        }

        public async Task<Group> GetGroupDetails(int id)
        {
            return await _context.Groups.FirstOrDefaultAsync(q => q.Id == id);
        }
        

    }
}
