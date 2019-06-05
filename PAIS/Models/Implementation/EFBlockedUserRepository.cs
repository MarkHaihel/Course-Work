using System.Collections.Generic;
using System.Linq;

namespace PAIS.Models
{
    public class EFBlockedUserRepository: IBlockedUserRepository
    {
        private ApplicationDbContext context;

        public EFBlockedUserRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<BlockedUser> BlockedUsers => context.BlockedUsers;

        public void SaveBlockedUser(BlockedUser blockedUser)
        {
            if (!context.BlockedUsers.Where(n => n.UserId == blockedUser.UserId).Any())
            {
                context.BlockedUsers.Add(blockedUser);
            }
            context.SaveChanges();
        }

        public BlockedUser DeleteBlockedUser(string blockedUserId)
        {
            BlockedUser dbEntry = context.BlockedUsers
                .FirstOrDefault(p => p.UserId == blockedUserId);

            if (dbEntry != null)
            {
                context.BlockedUsers.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }

        public BlockedUser GetBlockedUser(string blockedUserId)
        {
            return BlockedUsers
                .Where(n => n.UserId == blockedUserId)
                .OrderBy(n => n.UserId)
                .First();
        }
    }
}
