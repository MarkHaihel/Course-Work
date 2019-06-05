using System.Linq;

namespace PAIS.Models
{
    public interface ICommentRepository
    {
        IQueryable<Comment> Comments { get; }

        void SaveComment(Comment comment);

        Comment DeleteComment(int commentId);

        void DeleteUserComments(string userId);

        Comment GetComment(int commentId);
    }
}
