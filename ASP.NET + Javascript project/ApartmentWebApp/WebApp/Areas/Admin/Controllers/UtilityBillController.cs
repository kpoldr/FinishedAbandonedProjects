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
    public class UtilityBillController : Controller
    {
        private readonly IAppBLL _bll;

        public UtilityBillController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: UtilityBill
        public async Task<IActionResult> Index()
        {
            var res = await _bll.UtilityBill.GetAllAsync(User.GetUserId());
            return View(res);
            
        }

        // GET: UtilityBill/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var utilityBill = await _bll.UtilityBill.FirstOrDefaultAsync(id.Value);
            
            if (utilityBill == null)
            {
                return NotFound();
            }

            return View(utilityBill);
        }

        // GET: UtilityBill/Create
        public async Task<IActionResult> Create()
        {

            var vm = new UtilityBillCreateEditVm();
            
            vm.BillSelectList = new SelectList(
                await _bll.Bill.GetAllAsync(),
                nameof(Bill.Id),
                nameof(Bill.Number));
            
            vm.UtilitySelectList = new SelectList(
                await _bll.Utility.GetAllAsync(),
                nameof(Utility.Id),
                nameof(Utility.Name));
            
            
            return View(vm);
        }

        // POST: UtilityBill/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UtilityBillCreateEditVm vm)
        {
            if (ModelState.IsValid)
            {
                _bll.UtilityBill.Add(vm.UtilityBill);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            vm.BillSelectList = new SelectList(
                await _bll.Bill.GetAllAsync(),
                nameof(Bill.Id),
                nameof(Bill.Number),
                vm.UtilityBill.BillId);
            
            vm.UtilitySelectList = new SelectList(
                await _bll.Utility.GetAllAsync(),
                nameof(Utility.Id),
                nameof(Utility.Name),
                vm.UtilityBill.UtilityId);
            
            return View(vm);
        }

        // GET: UtilityBill/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var utilityBill = await _bll.UtilityBill.FirstOrDefaultAsync(id.Value);
            
            if (utilityBill == null)
            {
                return NotFound();
            }

            var vm = new UtilityBillCreateEditVm();

            vm.UtilityBill = utilityBill;
            
            
            vm.BillSelectList = new SelectList(
                await _bll.Bill.GetAllAsync(),
                nameof(Bill.Id),
                nameof(Bill.Number),
                vm.UtilityBill.BillId);
            
            vm.UtilitySelectList = new SelectList(
                await _bll.Utility.GetAllAsync(),
                nameof(Utility.Id),
                nameof(Utility.Name),
                vm.UtilityBill.UtilityId);
            
            return View(vm);
        }

        // POST: UtilityBill/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UtilityBillCreateEditVm vm)
        {
            if (id != vm.UtilityBill.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var utilityBillFromDb = await _bll.UtilityBill.FirstOrDefaultAsync(vm.UtilityBill.Id);

                if (utilityBillFromDb == null)
                {
                    return NotFound();
                }
                
                try
                {
                    _bll.UtilityBill.Update(vm.UtilityBill);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await UtilityBillExists(vm.UtilityBill.Id))
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
            
            vm.BillSelectList = new SelectList(
                await _bll.Bill.GetAllAsync(),
                nameof(Bill.Id),
                nameof(Bill.Number),
                vm.UtilityBill.BillId);
            
            vm.UtilitySelectList = new SelectList(
                await _bll.Utility.GetAllAsync(),
                nameof(Utility.Id),
                nameof(Utility.Name),
                vm.UtilityBill.UtilityId);
            
            return View(vm);
        }

        // GET: UtilityBill/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var utilityBill = await _bll.UtilityBill.FirstOrDefaultAsync(id.Value);
            
            if (utilityBill == null)
            {
                return NotFound();
            }

            return View(utilityBill);
        }

        // POST: UtilityBill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.UtilityBill.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UtilityBillExists(Guid id)
        {
            return await _bll.BillPayment.ExistsAsync(id);
        }
    }
}
