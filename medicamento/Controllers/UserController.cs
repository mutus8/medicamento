using medicamento.Models.Account;
using medicamento.Models.Account.Extensions;
using medicamento.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace medicamento.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class UserController : Controller
    {
        private readonly UserManager<Users> userManager;

        public UserController(UserManager<Users> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<ActionResult> Index()
        {
            var users = userManager.Users.ToList();
            var userRoles = new List<(Users User, IList<string> Roles)>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userRoles.Add((user, roles));
            }

            return View(userRoles);
            //return View(this.userManager.Users.ToList());
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        [Route("User/Edit/{userId}")]
        public async Task<ActionResult> Edit(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            return View(user.ToViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(id);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return View(model);
                }
                var role = await userManager.GetRolesAsync(user);

                if (user.UserName != model.UserName)
                    user.FullName = model.FullName;
                
                if (user.FullName != model.FullName)
                    user.FullName = model.FullName;

                if (user.Email != model.Email)
                    user.Email = model.Email;

                if(!role.Contains(model?.Role))
                    await userManager.AddToRoleAsync(user, model.Role);

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

            }

            return View(model);
          
        }

        public async Task<ActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction("Index");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = string.Join(" ", result.Errors.Select(e => e.Description));
                    return RedirectToAction("Index");
                }

            }
        }
        
    }
}
