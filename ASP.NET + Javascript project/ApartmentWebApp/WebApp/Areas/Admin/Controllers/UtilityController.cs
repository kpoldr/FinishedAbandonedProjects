#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using WebApp.Areas.Admin.ViewModels;

namespace WebApp.Areas_Admin_Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class UtilityController : Controller
    {
        private readonly IAppBLL _bll;

        public UtilityController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Utility
        public async Task<IActionResult> Index()
        {
            var res = await _bll.Utility.GetAllAsync(User.GetUserId());
            return View(res);
            
        }

        // GET: Utility/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var utility = await _bll.Utility.FirstOrDefaultAsync(id.Value);
            
            if (utility == null)
            {
                return NotFound();
            }

            return View(utility);
        }

        // GET: Utility/Create
        public async Task<IActionResult> Create()
        {

            var vm = new UtilityCreateEditVm();
            
            vm.ApartmentSelectList = new SelectList(
                await _bll.Apartment.GetAllAsync(),
                nameof(Apartment.Id),
                nameof(Apartment.Number));
            
            vm.BuildingSelectList = new SelectList(
                await _bll.Building.GetAllAsync(),
                nameof(Building.Id),
                nameof(Building.Address));
            
            
            return View(vm);
        }

        // POST: Utility/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UtilityCreateEditVm vm)
        {
            if (ModelState.IsValid)
            {
                _bll.Utility.Add(vm.Utility);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            vm.ApartmentSelectList = new SelectList(
                await _bll.Apartment.GetAllAsync(),
                nameof(Apartment.Id),
                nameof(Apartment.Number),
                vm.Utility.ApartmentId);
            
            vm.BuildingSelectList = new SelectList(
                await _bll.Building.GetAllAsync(),
                nameof(Building.Id),
                nameof(Building.Address),
                vm.Utility.BuildingId);
            
            return View(vm);
        }

        // GET: Utility/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var utility = await _bll.Utility.FirstOrDefaultAsync(id.Value);
            
            if (utility == null)
            {
                return NotFound();
            }

            var vm = new UtilityCreateEditVm();

            vm.Utility = utility;
            
            vm.ApartmentSelectList = new SelectList(
                await _bll.Apartment.GetAllAsync(),
                nameof(Apartment.Id),
                nameof(Apartment.Number),
                vm.Utility.ApartmentId);
            
            vm.BuildingSelectList = new SelectList(
                await _bll.Building.GetAllAsync(),
                nameof(Building.Id),
                nameof(Building.Address),
                vm.Utility.BuildingId);
            
            return View(vm);
        }

        // POST: Utility/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UtilityCreateEditVm vm)
        {
            if (id != vm.Utility.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                var utilityFromDb = await _bll.Utility.FirstOrDefaultAsync(vm.Utility.Id);

                if (utilityFromDb == null)
                {
                    return NotFound();
                }
                
                try
                {
                    utilityFromDb.Name.SetTranslation(vm.Utility.Name);
                    vm.Utility.Name = utilityFromDb.Name;
                    
                    _bll.Utility.Update(vm.Utility);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await UtilityExists(vm.Utility.Id))
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
            
            vm.ApartmentSelectList = new SelectList(
                await _bll.Apartment.GetAllAsync(),
                nameof(Apartment.Id),
                nameof(Apartment.Number),
                vm.Utility.ApartmentId);
            
            vm.BuildingSelectList = new SelectList(
                await _bll.Building.GetAllAsync(),
                nameof(Building.Id),
                nameof(Building.Address),
                vm.Utility.BuildingId);
            
            return View(vm);
        }

        // GET: Utility/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var utility = await _bll.Utility.FirstOrDefaultAsync(id.Value);
            
            if (utility == null)
            {
                return NotFound();
            }

            return View(utility);
        }

        // POST: Utility/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.Payment.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UtilityExists(Guid id)
        {
            return await _bll.BillPayment.ExistsAsync(id);
        }
    }
}
