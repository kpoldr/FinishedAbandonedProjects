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
    [Authorize(Roles = "admin")]
    public class OwnerController : Controller
    {
        private readonly IAppBLL _bll;

        public OwnerController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Owner
        public async Task<IActionResult> Index()
        {
            var res = await _bll.Owner.GetAllAsync(User.GetUserId());
            
            return View(res);
        }

        // GET: Owner/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var owner = await _bll.Owner.FirstOrDefaultAsync(id.Value);
            
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // GET: Owner/Create
        public Task<IActionResult> Create()
        {
            var vm = new OwnerCreateEditVm();
            
            return Task.FromResult<IActionResult>(View(vm));
        }

        // POST: Owner/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OwnerCreateEditVm vm)
        {
            
            if (ModelState.IsValid)
            {
                vm.Owner.AppUserId = User.GetUserId();
                _bll.Owner.Add(vm.Owner);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            vm.AppUserSelectList = new SelectList(
                await _bll.AppUser.GetAllAsync(),
                nameof(AppUser.Id),
                nameof(AppUser.FirstLastName),
                vm.Owner.AppUserId);
            
            return View(vm);
        }

        // GET: Owner/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _bll.Owner.FirstOrDefaultAsync(id.Value);
            if (owner == null)
            {
                return NotFound();
            }

            var vm = new OwnerCreateEditVm();
            vm.Owner = owner;
            
            vm.AppUserSelectList = new SelectList(
                await _bll.AppUser.GetAllAsync(),
                nameof(AppUser.Id),
                nameof(AppUser.FirstLastName),
                vm.Owner.AppUserId);

            
            return View(vm);
        }

        // POST: Owner/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, OwnerCreateEditVm vm)
        {

            if (id != vm.Owner.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                var ownerFromDb = await _bll.Owner.FirstOrDefaultAsync(vm.Owner.Id);

                if (ownerFromDb == null)
                {
                    return NotFound();
                }
                
                try
                {
                    ownerFromDb.Name.SetTranslation(vm.Owner.Name);
                    vm.Owner.Name = ownerFromDb.Name;
                    
                    _bll.Owner.Update(vm.Owner);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    Console.WriteLine("Failed :((((");
                    if (!await OwnerExists(vm.Owner.Id))
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
            
            vm.AppUserSelectList = new SelectList(
                await _bll.AppUser.GetAllAsync(),
                nameof(AppUser.Id),
                nameof(AppUser.FirstLastName),
                vm.Owner.AppUserId);
            
            return View(vm);
        }

        // GET: Owner/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var owner = await _bll.Owner.FirstOrDefaultAsync(id.Value);
            
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // POST: Owner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var owner = await _bll.Owner.FirstOrDefaultAsync(id);
            await _bll.Owner.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> OwnerExists(Guid id)
        {
            return await _bll.Owner.ExistsAsync(id);
        }
    }
}
