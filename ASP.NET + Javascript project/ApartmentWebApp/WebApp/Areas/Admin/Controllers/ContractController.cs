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
    public class ContractController : Controller
    {
        private readonly IAppBLL _bll;

        public ContractController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Contract
        public async Task<IActionResult> Index()
        {
            var res = await _bll.Contract.GetAllAsync(User.GetUserId());
            return View(res);
            
        }

        // GET: Contract/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var contract = await _bll.Contract.FirstOrDefaultAsync(id.Value);
            
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // GET: Contract/Create
        public async Task<IActionResult> Create()
        {
            var vm = new ContractCreateEditVm();
            
            vm.AssociationSelectList = new SelectList(
                await _bll.Bill.GetAllAsync(),
                nameof(Association.Id),
                nameof(Association.Name));
            
            vm.OwnerSelectList = new SelectList(
                await _bll.Payment.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Id));

            return View(vm);
        }

        // POST: Contract/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContractCreateEditVm vm)
        {
            if (ModelState.IsValid)
            {
                _bll.Contract.Add(vm.Contract);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            vm.AssociationSelectList = new SelectList(
                await _bll.Bill.GetAllAsync(),
                nameof(Association.Id),
                nameof(Association.Name),
                vm.Contract.AssociationId);
            
            vm.OwnerSelectList = new SelectList(
                await _bll.Payment.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Id),
                vm.Contract.OwnerId);
            
            return View(vm);
        }

        // GET: Contract/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var contract = await _bll.Contract.FirstOrDefaultAsync(id.Value);
            
            if (contract == null)
            {
                return NotFound();
            }

            var vm = new ContractCreateEditVm();

            vm.Contract = contract;
            
            vm.AssociationSelectList = new SelectList(
                await _bll.Bill.GetAllAsync(),
                nameof(Association.Id),
                nameof(Association.Name),
                vm.Contract.AssociationId);
            
            vm.OwnerSelectList = new SelectList(
                await _bll.Payment.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Id),
                vm.Contract.OwnerId);
            
            return View(vm);
        }

        // POST: Contract/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ContractCreateEditVm vm)
        {
            if (id != vm.Contract.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                var contractFromDb = await _bll.Contract.FirstOrDefaultAsync(vm.Contract.Id);
                
                if (contractFromDb == null)
                {
                    return NotFound();
                }


                try
                {
                    contractFromDb.Name.SetTranslation(vm.Contract.Name);
                    contractFromDb.Description.SetTranslation(vm.Contract.Description);

                    vm.Contract.Name = contractFromDb.Name;
                    vm.Contract.Description = contractFromDb.Description;

                    _bll.Contract.Update(vm.Contract);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ContractExists(vm.Contract.Id))
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
                await _bll.Bill.GetAllAsync(),
                nameof(Association.Id),
                nameof(Association.Name),
                vm.Contract.AssociationId);
            
            vm.OwnerSelectList = new SelectList(
                await _bll.Payment.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Id),
                vm.Contract.OwnerId);
            
            return View(vm);
        }

        // GET: Contract/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var contract = await _bll.Contract.FirstOrDefaultAsync(id.Value);
           
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // POST: Contract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            await _bll.Contract.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ContractExists(Guid id)
        {
            return await _bll.Contract.ExistsAsync(id);
            
        }
    }
}
