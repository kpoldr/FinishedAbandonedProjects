#nullable disable
using App.BLL.DTO.Identity;
using App.Contracts.BLL;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Admin.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    // [Authorize(Roles = "admin")]
    public class AssociationController : Controller
    {
        private readonly ILogger<AssociationController> _logger;
        private readonly IAppBLL _bll;

        public AssociationController(IAppBLL bll, ILogger<AssociationController> logger)
        {
            _bll = bll;
            _logger = logger;
        }

        // GET: Association
        public async Task<IActionResult> Index()
        {
            var res = await _bll.Association.GetAllAsync(User.GetUserId());
            return View(res);
        }

        // GET: Association/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var association = await _bll.Association.FirstOrDefaultAsync(id.Value);

            if (association == null)
            {
                return NotFound();
            }

            return View(association);
        }

        // GET: Association/Create
        public Task<IActionResult> Create()
        {
            var vm = new AssociationCreateEditVM();
            
            return Task.FromResult<IActionResult>(View(vm));
        }

        // POST: Association/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AssociationCreateEditVM vm)
        {
            // Console.Write("This modelstate is:" + (ModelState.IsValid));

            if (ModelState.IsValid)
            {
                vm.Association.AppUserId = User.GetUserId();
                _bll.Association.Add(vm.Association);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // vm.AppUserSelectList = new SelectList(
            //     .Users
            //         .OrderBy(o => o.UserName)
            //         .Select(x => new {x.Id, x.UserName})
            //         .ToListAsync(),
            //     nameof(AppUser.Id),
            //     nameof(AppUser.UserName),
            //     vm.Association.AppUserId);

            return View(vm);
        }

        // GET: Association/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var association = await _bll.Association.FirstOrDefaultAsync(id.Value);
            if (association == null)
            {
                return NotFound();
            }

            var vm = new AssociationCreateEditVM();

            vm.Association = association;

            vm.AppUserSelectList = new SelectList(
                await _bll.AppUser.GetAllAsync(),
                nameof(AppUser.Id),
                nameof(AppUser.FirstLastName),
                vm.Association.AppUserId);

            return View(vm);
        }

        // POST: Association/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AssociationCreateEditVM vm)
        {
            if (id != vm.Association.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var associationFromDb = await _bll.Association.FirstOrDefaultAsync(vm.Association.Id);
                
                if (associationFromDb == null)
                {
                    return NotFound();
                }

                // vm.Association.AppUserId = User.GetUserId();
                try
                {
                    associationFromDb.Name.SetTranslation(vm.Association.Name);
                    associationFromDb.Description?.SetTranslation(vm.Association.Description ?? "");
                    associationFromDb.BankName?.SetTranslation(vm.Association.BankName ?? "");

                    vm.Association.Name = associationFromDb.Name;
                    vm.Association.Description = associationFromDb.Description;
                    vm.Association.BankName = associationFromDb.BankName;
                    
                    _bll.Association.Update(vm.Association);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await AssociationExists(vm.Association.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            
            return View(vm);
        }

        // GET: Association/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var association = await _bll.Association
                .FirstOrDefaultAsync(id.Value);
            if (association == null)
            {
                return NotFound();
            }

            return View(association);
        }

        // POST: Association/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var association = await _bll.Association.FirstOrDefaultAsync(id);
            await _bll.Association.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> AssociationExists(Guid id)
        {
            return await _bll.Association.ExistsAsync(id);
        }
    }
}