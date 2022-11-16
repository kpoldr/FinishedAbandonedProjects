#nullable disable
using App.Contracts.BLL;
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
    public class BillController : Controller
    {
        private readonly IAppBLL _bll;

        public BillController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Bill
        public async Task<IActionResult> Index()
        {
            var res = await _bll.Bill.GetAllAsync(User.GetUserId());
            return View(res);
        }

        // GET: Bill/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bill = await _bll.Bill.FirstOrDefaultAsync(id.Value);
            
            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }

        // GET: Bill/Create
        public async Task<IActionResult> Create()
        {
            
            var vm = new BillCreateEditVM();
            vm.ApartmentSelectList = new SelectList(
                await _bll.Apartment.GetAllAsync(),
                nameof(Apartment.Id),
                nameof(Apartment.Number));
            vm.OwnerSelectList = new SelectList(
                await _bll.Owner.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Name));
            
            return View(vm);
        }

        // POST: Bill/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BillCreateEditVM vm)
        {
            if (ModelState.IsValid)
            {
                _bll.Bill.Add(vm.Bill);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            vm.ApartmentSelectList = new SelectList(
                await _bll.Apartment.GetAllAsync(),
                nameof(Apartment.Id),
                nameof(Apartment.Number),
                vm.Bill.ApartmentId);
            vm.OwnerSelectList = new SelectList(
                await _bll.Owner.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Name),
                vm.Bill.OwnerId);
            
            return View(vm);
        }

        // GET: Bill/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var bill = await _bll.Bill.FirstOrDefaultAsync(id.Value);
            
            if (bill == null)
            {
                return NotFound();
            }

            var vm = new BillCreateEditVM();
            
            vm.Bill = bill;
            
            vm.ApartmentSelectList = new SelectList(
                await _bll.Apartment.GetAllAsync(),
                nameof(Apartment.Id),
                nameof(Apartment.Number),
                vm.Bill.ApartmentId);
            vm.OwnerSelectList = new SelectList(
                await _bll.Owner.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Name),
                vm.Bill.OwnerId);

            
            return View(vm);
        }

        // POST: Bill/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, BillCreateEditVM vm)
        {
            if (id != vm.Bill.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                
                var billFromDb = await _bll.Bill.FirstOrDefaultAsync(vm.Bill.Id);
                
                if (billFromDb == null)
                {
                    return NotFound();
                }
                
                try
                {
                    
                    _bll.Bill.Update(vm.Bill);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await BillExists(vm.Bill.Id))
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
                vm.Bill.ApartmentId);
            vm.OwnerSelectList = new SelectList(
                await _bll.Owner.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Name),
                vm.Bill.OwnerId);
            
            return View(vm);
        }

        // GET: Bill/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var bill = await _bll.Bill
                .FirstOrDefaultAsync(id.Value);
            
            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);
        }

        // POST: Bill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            // var bill = await _bll.Bill.FirstOrDefaultAsync(id);
            await _bll.Bill.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> BillExists(Guid id)
        {
            return await _bll.Bill.ExistsAsync(id);
        }
    }
}
