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
    public class PaymentController : Controller
    {
        private readonly IAppBLL _bll;

        public PaymentController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Payment
        public async Task<IActionResult> Index()
        {
            var res = await _bll.Payment.GetAllAsync(User.GetUserId());
            return View(res);
        }

        // GET: Payment/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _bll.Payment.FirstOrDefaultAsync(id.Value);

            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payment/Create
        public async Task<IActionResult> Create()
        {
            var vm = new PaymentCreateEditVm();

            vm.FundSelectList = new SelectList(
                await _bll.Fund.GetAllAsync(),
                nameof(Fund.Id),
                nameof(Fund.Name));

            vm.OwnerSelectList = new SelectList(
                await _bll.Owner.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Name));

            vm.PersonSelectList = new SelectList(
                await _bll.Person.GetAllAsync(),
                nameof(Person.Id),
                nameof(Person.Name));

            return View(vm);
        }

        // POST: Payment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentCreateEditVm vm)
        {
            if (ModelState.IsValid)
            {
                _bll.Payment.Add(vm.Payment);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            vm.FundSelectList = new SelectList(
                await _bll.Fund.GetAllAsync(),
                nameof(Fund.Id),
                nameof(Fund.Name),
                vm.Payment.FundId);

            vm.OwnerSelectList = new SelectList(
                await _bll.Owner.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Name),
                vm.Payment.OwnerId);

            vm.PersonSelectList = new SelectList(
                await _bll.Person.GetAllAsync(),
                nameof(Person.Id),
                nameof(Person.Name),
                vm.Payment.PersonId);

            return View(vm);
        }

        // GET: Payment/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var payment = await _bll.Payment.FirstOrDefaultAsync(id.Value);
            
            if (payment == null)
            {
                return NotFound();
            }

            var vm = new PaymentCreateEditVm();
            
            vm.Payment = payment;
            
            vm.FundSelectList = new SelectList(
                await _bll.Fund.GetAllAsync(),
                nameof(Fund.Id),
                nameof(Fund.Name),
                vm.Payment.FundId);

            vm.OwnerSelectList = new SelectList(
                await _bll.Owner.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Name),
                vm.Payment.OwnerId);

            vm.PersonSelectList = new SelectList(
                await _bll.Person.GetAllAsync(),
                nameof(Person.Id),
                nameof(Person.Name),
                vm.Payment.PersonId);

            return View(vm);
        }

        // POST: Payment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PaymentCreateEditVm vm)
        {
            if (id != vm.Payment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var payment = await _bll.Payment.FirstOrDefaultAsync(vm.Payment.Id);

                if (payment == null)
                {
                    return NotFound();
                }

                
                try
                {
                    _bll.Payment.Update(vm.Payment);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await PaymentExists(vm.Payment.Id))
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

            vm.FundSelectList = new SelectList(
                await _bll.Fund.GetAllAsync(),
                nameof(Fund.Id),
                nameof(Fund.Name),
                vm.Payment.FundId);

            vm.OwnerSelectList = new SelectList(
                await _bll.Owner.GetAllAsync(),
                nameof(Owner.Id),
                nameof(Owner.Name),
                vm.Payment.OwnerId);

            vm.PersonSelectList = new SelectList(
                await _bll.Person.GetAllAsync(),
                nameof(Person.Id),
                nameof(Person.Name),
                vm.Payment.PersonId);

            return View(vm);
        }

        // GET: Payment/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var payment = await _bll.Payment.FirstOrDefaultAsync(id.Value);
            
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.Payment.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PaymentExists(Guid id)
        {
            return await _bll.Payment.ExistsAsync(id);
        }
    }
}