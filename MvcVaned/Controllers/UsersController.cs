using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Claims;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using MvcVanced.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;

namespace MvcVanced.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        // GET: Users
        public Boolean isAdminUser() {
            if (User.Identity.IsAuthenticated) {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var s = UserManager.GetRoles(user.GetUserId());
                if (s != null && s.Count > 0 && s[0].ToString() == "Admin") {
                    return true;
                }
                else {
                    return false;
                }
            }
            return false;
        }

        public ActionResult Index() {
            if (User.Identity.IsAuthenticated) {
                var userIdentity = User.Identity;
                ViewBag.Name = userIdentity.Name;

                ViewBag.displayMenu = "No";

                if (isAdminUser()) {
                    ViewBag.displayMenu = "Yes";

                    var UserManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

                    //var s = UserManager.GetRoles(userIdentity.GetUserId());

                    List<User> users = new List<User>();
                    foreach (var user in UserManager.Users) {
                        users.Add(new User {
                            ID = user.Id,
                            Email = user.Email,
                            Username = user.UserName
                        });
                    }

                    return View(users);
                }

                return View();
            }
            else {
                ViewBag.Name = "Not Logged IN";
            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id) {
            if (User.Identity.IsAuthenticated) {
                if (isAdminUser()) {
                    if (id == null) {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    var UserManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var users = UserManager.Users;

                    if (!users.Any(x => x.Id.Equals(id))) {
                        return HttpNotFound();
                    }

                    var user = users.First(x => x.Id.Equals(id));
                    var roles = UserManager.GetRoles(user.Id).ToList();

                    var rolesList = context.Roles.ToList();
                    rolesList.Insert(0, new IdentityRole {
                        Name = ""
                    });
                    ViewBag.Name = new SelectList(rolesList, "Name", "Name");

                    return View(new UserEditViewModel {
                        UserRoles = ((roles?.Count > 0) ? roles[0] : ""),
                        Email = user.Email,
                        UserName = user.UserName
                    });
                }

                return View();
            }

            return View();
        }

        // POST: Users/Edit/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(UserEditViewModel model) {
            var rolesList = context.Roles.ToList();
            rolesList.Insert(0, new IdentityRole {
                Name = ""
            });
            ViewBag.Name = new SelectList(rolesList, "Name", "Name");

            if (ModelState.IsValid) {
                var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

                // get user object from the storage
                var user = await userManager.FindByIdAsync(model.Id);

                if (user == null) {
                    return HttpNotFound();
                }

                // change username and email
                user.UserName = model.UserName;
                user.Email = model.Email;

                // Persiste the changes
                var rr = await userManager.UpdateAsync(user);

                // Retrieve all roles
                var allRoles = context.Roles.ToList().Select(x => x.Name).ToArray();

                if (allRoles.Any(x => x.Equals(model.UserRoles))) {
                    // Remove all roles from user
                    foreach (var role in allRoles) {
                        await userManager.RemoveFromRoleAsync(user.Id, role);
                    }

                    // Add the role
                    await userManager.AddToRoleAsync(user.Id, model.UserRoles);

                    return RedirectToAction("Index");
                }
                else if (string.IsNullOrEmpty(model.UserRoles)) {
                    // Remove all roles from user
                    foreach (var role in allRoles) {
                        await userManager.RemoveFromRoleAsync(user.Id, role);
                    }

                    return RedirectToAction("Index");
                }

                return View(model);
            }

            return View(model);
        }

        // GET: APKs/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByIdAsync(id);

            if (user == null) {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: APKs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(string id) {
            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = await userManager.FindByIdAsync(id);
            var result = await userManager.DeleteAsync(user);

            return RedirectToAction("Index");
        }
    }
}