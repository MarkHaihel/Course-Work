using System.Collections.Generic;
using System.Linq;

namespace PAIS.Models
{
    public class EFBlockedUserRepository: IBlockedUserRepository
    {
        private ApplicationDbContext context;
        public IQueryable<BlockedUser> BlockedUsers { get; set; }

        public void SaveBlockedUser(BlockedUser blockedUser)
        {
            if (blockedUser.BlockedUserId == null)
            {
                context.BlockedUsers.Add(blockedUser);
            }
            else
            {
                BlockedUser dbEntry = context.BlockedUsers
                    .FirstOrDefault(p => p.BlockedUserId == blockedUser.BlockedUserId);
                if (dbEntry != null)
                {
                    dbEntry.UserId = blockedUser.UserId;
                }
            }
            context.SaveChanges();
        }

        public BlockedUser DeleteBlockedUser(string blockedUserId)
        {
            BlockedUser dbEntry = context.BlockedUsers
                .FirstOrDefault(p => p.BlockedUserId == blockedUserId);

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
                .Where(n => n.BlockedUserId == blockedUserId)
                .OrderBy(n => n.BlockedUserId)
                .First();
        }
    }
}
