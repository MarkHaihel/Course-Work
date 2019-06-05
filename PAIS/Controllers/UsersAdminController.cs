using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PAIS.Models;
using PAIS.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PAIS.Controllers
{
    [Authorize(Roles = "user admin")]
    public class UsersAdminController : Controller
    {
        private UserManager<IdentityUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private ICommentRepository commentRepository;
        private IBlockedUserRepository blockedUserRepository;

        public UsersAdminController(RoleManager<IdentityRole> _roleManager, UserManager<IdentityUser> _userManager,
            ICommentRepository _commentRepository, IBlockedUserRepository _blockedUserRepository)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            commentRepository = _commentRepository;
            blockedUserRepository = _blockedUserRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<IdentityUser> Users = new List<IdentityUser>();
            Users = userManager.Users.ToList();

            return View(Users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            IdentityUser user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("Error");
            }
            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, LoginName = user.UserName };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.UserName = model.LoginName;

                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Error");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(string id)
        {
            IdentityUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return RedirectToAction("Error");
                }
            }
            foreach (var c in commentRepository.Comments.Where(c => c.OwnerId == id))
            {
                commentRepository.DeleteComment(c.CommentId);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditUserRoles(string userId)
        {
            // get user
            IdentityUser user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // gets user`s roles list
                var userRoles = await userManager.GetRolesAsync(user);
                var allRoles = roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return RedirectToAction("Error");
        }

        [HttpPost]
        public async Task<IActionResult> EditUserRoles(string userId, List<string> roles)
        {
            // gets user
            IdentityUser user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // gets user`s roles list
                var userRoles = await userManager.GetRolesAsync(user);
                // gets all roles
                var allRoles = roleManager.Roles.ToList();
                // gets roles list, that was added 
                var addedRoles = roles.Except(userRoles);
                // gets roles that was deleted
                var removedRoles = userRoles.Except(roles);

                await userManager.AddToRolesAsync(user, addedRoles);

                await userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("Index");
            }

            return RedirectToAction("Error");
        }

        [HttpGet]
        public IActionResult BlockUser(string UserId, string BookId)
        {
            // block user
            BlockedUser blockedUser = new BlockedUser { UserId = UserId };
            blockedUserRepository.SaveBlockedUser(blockedUser);

            // delete all his comments
            commentRepository.DeleteUserComments(UserId);

            return RedirectToAction("Details", "Book", new { bookId = BookId });
        }

        [HttpGet]
        public IActionResult UnblockUser(string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
            {
                RedirectToAction("Error");
            }
            string id = blockedUserRepository.BlockedUsers.First(u => u.UserId == UserId).UserId;
            blockedUserRepository.DeleteBlockedUser(id);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new  { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
