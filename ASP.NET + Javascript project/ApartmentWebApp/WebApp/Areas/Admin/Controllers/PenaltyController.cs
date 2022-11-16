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
    public class PenaltyController : Controller
    {
        private readonly IAppBLL _bll;

        public PenaltyController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Penalty
        public async Task<IActionResult> Index()
        {
            var res = await _bll.Penalty.GetAllAsync(User.GetUserId());
            return View(res);
            
        }

        // GET: Penalty/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var penalty = await _bll.Penalty.FirstOrDefaultAsync(id.Value);
            
            if (penalty == null)
            {
                return NotFound();
            }

            return View(penalty);
        }

        // GET: Penalty/Create
        public async Task<IActionResult> Create()
        {
            var vm = new PenaltyCreateEditVm();
            
            vm.BillSelectList = new SelectList(
                await _bll.Bill.GetAllAsync(),
                nameof(Bill.Id),
                nameof(Bill.Number));

            vm.OwnerSelectList = new SelectList(
                await _bll.Owner.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Name));

            return View(vm);
        }

        // POST: Penalty/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PenaltyCreateEditVm vm)
        {
            if (ModelState.IsValid)
            {
                _bll.Penalty.Add(vm.Penalty);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.BillSelectList = new SelectList(
                await _bll.Bill.GetAllAsync(),
                nameof(Bill.Id),
                nameof(Bill.Number),
                vm.Penalty.BillId);
            
            vm.OwnerSelectList = new SelectList(
                await _bll.Owner.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Name),
                vm.Penalty.OwnerId);
            
            return View(vm);
        }

        // GET: Penalty/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var penalty = await _bll.Penalty.FirstOrDefaultAsync(id.Value);
            if (penalty == null)
            {
                return NotFound();
            }

            var vm = new PenaltyCreateEditVm();
            
            vm.Penalty = penalty;
            
            vm.BillSelectList = new SelectList(
                await _bll.Bill.GetAllAsync(),
                nameof(Bill.Id),
                nameof(Bill.Number),
                vm.Penalty.BillId);
            
            vm.OwnerSelectList = new SelectList(
                await _bll.Owner.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Name),
                vm.Penalty.OwnerId);
            
            return View(vm);
        }

        // POST: Penalty/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PenaltyCreateEditVm vm)
        {
            if (id != vm.Penalty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var penaltyFromDb = await _bll.Penalty.FirstOrDefaultAsync(vm.Penalty.Id);

                if (penaltyFromDb == null)
                {
                    return NotFound();
                }
                
                try
                {
                    penaltyFromDb.PenaltyName.SetTranslation(vm.Penalty.PenaltyName);
                    vm.Penalty.PenaltyName = penaltyFromDb.PenaltyName;
                    
                    _bll.Penalty.Update(vm.Penalty);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await PenaltyExists(vm.Penalty.Id))
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
                vm.Penalty.BillId);
            
            vm.OwnerSelectList = new SelectList(
                await _bll.Owner.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Name),
                vm.Penalty.OwnerId);
            
            return View(vm);
        }

        // GET: Penalty/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var penalty = await _bll.Penalty.FirstOrDefaultAsync(id.Value);
            
            if (penalty == null)
            {
                return NotFound();
            }

            return View(penalty);
        }

        // POST: Penalty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.Penalty.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PenaltyExists(Guid id)
        {
            return await _bll.BillPayment.ExistsAsync(id);
        }
    }
}
