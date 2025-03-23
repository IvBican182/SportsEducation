using Aperta_web_app.Data;
using Aperta_web_app.Models.Group;
using Aperta_web_app.Models.User;
using Aperta_web_app.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace Aperta_web_app.Services.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly AppDbContext _context;

        public GroupService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<GroupsWithUsersDto>> GetGroupsWithUsersAsync(int clubId)
        {
            var groups = await _context.Groups.Where(q => q.ClubId == clubId)
           .Include(g => g.Users)
           .Select(g => new GroupsWithUsersDto
           {
               Id = g.Id,
               Name = g.Name,
               Users = g.Users.Select(u => new UserInfoDto
               {
                   Id = u.Id,
                   FirstName = $"{u.FirstName} {u.LastName}", // Assuming FirstName/LastName exist in ApplicationUser
                   Email = u.Email
               }).ToList()
           })
           .ToListAsync();

            return groups;
        }

        public async Task<List<GroupDto>> GetClubGroups(int clubId)
        {
            var groups = await _context.Groups.Where(q => q.ClubId == clubId).ToListAsync();

            if (groups.Count == 0)// Check if the list is empty
            {
                throw new Exception("No groups found!");
            }

            var groupDtos = groups.Select(group => new GroupDto
            {
                Id = group.Id,
                Name = group.Name
            }).ToList();

            return groupDtos;


        }

        public async Task<List<GroupsWithUsersDto>> UpdateUserGroupAsync(string userId, int groupId)
        {
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null)
            {
                return null;
            }

            var group = await _context.Groups.FindAsync(groupId);
            if (group == null)
            {
                return null; // Or throw an exception
            }

            user.GroupId = groupId;

            await _context.SaveChangesAsync();

            var groups = await _context.Groups
                .Where(g => g.ClubId == group.ClubId) // Use the ClubId of the updated group
                .Include(g => g.Users)
                .Select(g => new GroupsWithUsersDto
                {
                  Id = g.Id,
                  Name = g.Name,
                  Users = g.Users.Select(u => new UserInfoDto
                   {
                      Id = u.Id,
                       FirstName = $"{u.FirstName} {u.LastName}", // Assuming FirstName/LastName exist in ApplicationUser
                       Email = u.Email
                   }).ToList()
                })
                .ToListAsync();

              return groups;
        }

        public async Task<List<GroupsWithUsersDto>> DeleteGroupAsync(int groupId)
        {
            // Find the group by its Id
            var group = await _context.Groups
                .Include(g => g.Users) // Include Users if necessary for cascade delete or checks
                .FirstOrDefaultAsync(g => g.Id == groupId);

            if (group == null)
            {
                throw new KeyNotFoundException("Group not found.");  // Return a more specific error
            }

            if (group.Users.Any())
            {
                throw new InvalidOperationException("Cannot delete a group that contains players. Create a new group and move your players before deleting");
            }

            var clubId = group.ClubId; // Cache the ClubId before removing

            // Remove the group
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            // Fetch updated list of groups for the same club
            var groups = await _context.Groups
                .Where(g => g.ClubId == clubId) // Use cached ClubId
                .Include(g => g.Users) // Include Users to construct DTO
                .Select(g => new GroupsWithUsersDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    Users = g.Users.Select(u => new UserInfoDto
                    {
                        Id = u.Id,
                        FirstName = $"{u.FirstName} {u.LastName}", // Assuming FirstName/LastName exist
                        Email = u.Email
                    }).ToList()
                })
                .ToListAsync();

            return groups; // Return updated groups

        }


    }
}
