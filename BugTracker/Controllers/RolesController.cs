﻿namespace BugTracker.Controllers
{
    public class RolesController : Controller
    {
        private RoleManager<IdentityRole> roleManager;
        private UserManager<AppUser> userManager;
        public RolesController(RoleManager<IdentityRole> roleMgr, UserManager<AppUser> userMgr)
        {
            roleManager = roleMgr;
            userManager = userMgr;
        }

        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.ToListAsync();
            var viewModel = new RoleIndexViewModel
            {
                Roles = await roleManager.Roles.ToListAsync(),
                Users = new SelectList(users, "Id", "UserName"),
                AssignToId = ""
            };


            return View(viewModel);

        }


        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        [HttpGet]
        public IActionResult Edit(string? id)
        {
            var viewModel = new RoleFormViewModel();

            if (id == null)
                return View("RoleForm", viewModel);

            var role = roleManager.Roles.Where(r => r.Id == id).FirstOrDefault();

            if (role == null)
                return NotFound();

            viewModel.Name = role.Name;
            viewModel.Id = role.Id;
            return View("RoleForm", viewModel);

        }


        [HttpPost]
        public async Task<IActionResult> Save(RoleFormViewModel viewModel)
        {
            // post from an update form
            if (viewModel.Id != null)
            {
                //get the role from the DB
                var role = await roleManager.FindByIdAsync(viewModel.Id);

                if (role == null)
                {
                    return NotFound();
                }

                // set the new name
                await roleManager.SetRoleNameAsync(role, viewModel.Name);
                // this has to be updated, may be the equivalent to _context.saveChanges()
                IdentityResult _result = await roleManager.UpdateAsync(role);

                if (_result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(_result);
            }

            // Here it is a new Role.

            IdentityResult result = await roleManager.CreateAsync(new IdentityRole(viewModel.Name));

            if (result.Succeeded)
                return RedirectToAction("Index");
            else
                Errors(result);

            return View("RoleForm", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("Index");

                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "No role found");
            return View("Index", roleManager.Roles);
        }

        [HttpPost]
        public async Task<IActionResult> Assign(string roleName, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            await userManager.AddToRoleAsync(user, roleName);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var users = await userManager.Users.ToListAsync();
            var usersInRole = await userManager.GetUsersInRoleAsync(role.Name);
            var usersNotInRole = (usersInRole.Count > 0)
                                        ? users.Where(u => !usersInRole.Contains(u)).ToList()
                                        : users;

            var viewModel = new RoleDetailsViewModel
            {
                Role = role,
                UsersInRole = (usersInRole.Count() > 0)
                                    ? new SelectList(usersInRole, "Id", "UserName")
                                    : null,
                UsersNotInRole = (usersNotInRole.Count() > 0)
                                    ? new SelectList(usersNotInRole, "Id", "UserName")
                                    : null
            };

            return View(viewModel);
        }





    }
}
