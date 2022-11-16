#nullable disable
using App.Contracts.BLL;
using App.DAL.EF;
using App.Domain;
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
    public class ApartmentController : Controller
    {
        private readonly IAppBLL _bll;

        public ApartmentController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Apartment
        public async Task<IActionResult> Index()
        {
            var res = await _bll.Apartment.GetAllAsync(User.GetUserId());
            return View(res);
            
        }

        // GET: Apartment/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var apartment = await _bll.Apartment.FirstOrDefaultAsync(id.Value);

            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        // GET: Apartment/Create
        public async Task<IActionResult> Create()
        {
            var vm = new ApartmentCreateEditVM();
            
            vm.OwnersSelectList = new SelectList(
                await _bll.Owner.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Name));
            
            vm.BuildingSelectList = new SelectList(
                await _bll.Building.GetAllAsync(),
                nameof(Building.Id),
                nameof(Building.Address));
            
            return View(vm);
        }

        // POST: Apartment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApartmentCreateEditVM vm)
        {
            if (ModelState.IsValid)
            {
                _bll.Apartment.Add(vm.Apartment);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            vm.OwnersSelectList = new SelectList(
                await _bll.Owner.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Name),
                vm.Apartment.OwnerId);
            
            vm.BuildingSelectList = new SelectList(
                await _bll.Payment.GetAllAsync(),
                nameof(Building.Id),
                nameof(Building.Address),
            vm.Apartment.OwnerId);
            
            // vm.OwnersSelectList = new SelectList(_context.Owners, "Id", "Email", apartment.OwnerId);
            return View(vm);
        }

        // GET: Apartment/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var apartment = await _bll.Apartment.FirstOrDefaultAsync(id.Value);

            if (apartment == null)
            {
                return NotFound();
            }

            var vm = new ApartmentCreateEditVM();
            
            vm.Apartment = apartment;

            vm.OwnersSelectList = new SelectList(
                await _bll.Owner.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Name),
                vm.Apartment.OwnerId);
            
            vm.BuildingSelectList = new SelectList(
                await _bll.Payment.GetAllAsync(),
                nameof(Building.Id),
                nameof(Building.Address),
                vm.Apartment.OwnerId);
            
            return View(vm);
        }

        // POST: Apartment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ApartmentCreateEditVM vm)
        {
            
            if (id != vm.Apartment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                try
                {
                    _bll.Apartment.Update(vm.Apartment);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ApartmentExists(vm.Apartment.Id))
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
            
            vm.OwnersSelectList = new SelectList(
                await _bll.Owner.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Name),
                vm.Apartment.OwnerId);
            
            vm.BuildingSelectList = new SelectList(
                await _bll.Payment.GetAllAsync(),
                nameof(Building.Id),
                nameof(Building.Address),
                vm.Apartment.OwnerId);
            
            return View(vm);
        }

        // GET: Apartment/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var apartment = await _bll.Apartment.FirstOrDefaultAsync(id.Value);
            
            if (apartment == null)
            {
                return NotFound();
            }

            return View(apartment);
        }

        // POST: Apartment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.BillPayment.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ApartmentExists(Guid id)
        {
            return await _bll.Apartment.ExistsAsync(id);
        }
    }
}