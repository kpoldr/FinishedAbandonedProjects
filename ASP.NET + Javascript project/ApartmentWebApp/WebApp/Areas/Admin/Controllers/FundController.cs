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
    public class FundController : Controller
    {
        private readonly IAppBLL _bll;

        public FundController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Fund
        public async Task<IActionResult> Index()
        {
            var res = await _bll.Fund.GetAllAsync(User.GetUserId());
            return View(res);
            
        }

        // GET: Fund/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fund = await _bll.Fund.FirstOrDefaultAsync(id.Value);
            
            if (fund == null)
            {
                return NotFound();
            }

            return View(fund);
        }

        // GET: Fund/Create
        public async Task<IActionResult> Create()
        {
            var vm = new FundCreateEditVm();
            
            vm.AssociationSelectList = new SelectList(
                await _bll.Association.GetAllAsync(),
                nameof(Association.Id),
                nameof(Association.Name));

            // ViewData["AssociationId"] = new SelectList(_context.Associations, "Id", "Description");
            return View(vm);
        }

        // POST: Fund/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FundCreateEditVm vm)
        {
            if (ModelState.IsValid)
            {
                _bll.Association.FirstOrDefaultAsync(vm.Fund.AssociationId).Result!.Funds!.Add(vm.Fund);
                _bll.Fund.Add(vm.Fund);
                
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            vm.AssociationSelectList = new SelectList(
                await _bll.Association.GetAllAsync(),
                nameof(Association.Id),
                nameof(Association.Name),
                vm.Fund.AssociationId);
            
            // ViewData["AssociationId"] = new SelectList(_context.Associations, "Id", "Description", fund.AssociationId);
            return View(vm);
        }

        // GET: Fund/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var fund = await _bll.Fund.FirstOrDefaultAsync(id.Value);
            
            if (fund == null)
            {
                return NotFound();
            }

            var vm = new FundCreateEditVm();

            vm.Fund = fund;
            
            vm.AssociationSelectList = new SelectList(
                await _bll.Association.GetAllAsync(),
                nameof(Association.Id),
                nameof(Association.Name),
                vm.Fund.AssociationId);
            
            return View(vm);
        }

        // POST: Fund/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FundCreateEditVm vm)
        {
            if (id != vm.Fund.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                var fundFromDb = await _bll.Fund.FirstOrDefaultAsync(vm.Fund.Id);

                if (fundFromDb == null)
                {
                    return NotFound();
                }


                try
                {
                    fundFromDb.Name.SetTranslation(vm.Fund.Name);

                    vm.Fund.Name = fundFromDb.Name;

                    _bll.Fund.Update(vm.Fund);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await FundExists(vm.Fund.Id))
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
            
            vm.AssociationSelectList = new SelectList(
                await _bll.Association.GetAllAsync(),
                nameof(Association.Id),
                nameof(Association.Name),
                vm.Fund.AssociationId);
            
            return View(vm);
        }

        // GET: Fund/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var fund = await _bll.Fund.FirstOrDefaultAsync(id.Value);
            
            if (fund == null)
            {
                return NotFound();
            }

            return View(fund);
        }

        // POST: Fund/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.Fund.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> FundExists(Guid id)
        {
            return await _bll.Fund.ExistsAsync(id);
        }
    }
}
