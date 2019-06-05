using System.Collections.Generic;
using System.Linq;

namespace PAIS.Models
{
    public class EFCommentRepository: ICommentRepository
    {
        private ApplicationDbContext context;

        public EFCommentRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Comment> Comments => context.Comments;

        public void SaveComment(Comment comment)
        {
            if (comment.CommentId == 0)
            {
                context.Comments.Add(comment);
            }
            else
            {
                Comment dbEntry = context.Comments
                    .FirstOrDefault(p => p.CommentId == comment.CommentId);
                if (dbEntry != null)
                {
                    dbEntry.Text = comment.Text;
                    dbEntry.Time = comment.Time;
                }
            }
            context.SaveChanges();
        }

        public Comment DeleteComment(int commentId)
        {
            Comment dbEntry = context.Comments
                .FirstOrDefault(p => p.CommentId == commentId);

            if (dbEntry != null)
            {
                context.Comments.Remove(dbEntry);
                context.SaveChanges();
            }

            return dbEntry;
        }
        public void DeleteUserComments(string userId)
        {
            foreach (var c in Comments.Where(c => c.OwnerId == userId))
            {
                Comment dbEntry = context.Comments
                    .FirstOrDefault(p => p.CommentId == c.CommentId);

                if (dbEntry != null)
                {
                    context.Comments.Remove(dbEntry);
                }
            }

            context.SaveChanges();
        }

        public Comment GetComment(int commentId)
        {
            return Comments
                .Where(n => n.CommentId == commentId)
                .OrderBy(n => n.CommentId)
                .First();
        }
    }
}
