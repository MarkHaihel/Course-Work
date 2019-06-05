﻿using System.Linq;

namespace PAIS.Models
{
    public interface IBlockedUserRepository
    {
        IQueryable<BlockedUser> BlockedUsers { get; set; }

        void SaveBlockedUser(BlockedUser blockedUser);

        BlockedUser DeleteBlockedUser(string blockedUserId);

        BlockedUser GetBlockedUser(string blockedUserId);
    }
}
