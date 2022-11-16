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
    public class BillPaymentController : Controller
    {
        private readonly IAppBLL _bll;

        public BillPaymentController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: BillPayment
        public async Task<IActionResult> Index()
        {
            var res = await _bll.BillPayment.GetAllAsync(User.GetUserId());
            return View(res);
        }

        // GET: BillPayment/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billPayment = await _bll.BillPayment.FirstOrDefaultAsync(id.Value);
            
            if (billPayment == null)
            {
                return NotFound();
            }

            return View(billPayment);
        }

        // GET: BillPayment/Create
        public async Task<IActionResult> Create()
        {
            var vm = new BillPaymentCreateEditVm();
            
            vm.BillSelectList = new SelectList(
                await _bll.Bill.GetAllAsync(),
                nameof(Bill.Id),
                nameof(Bill.Number));
            
            vm.PaymentSelectList = new SelectList(
                await _bll.Payment.GetAllAsync(),
                nameof(Bill.Id));

            return View(vm);
        }

        // POST: BillPayment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BillPaymentCreateEditVm vm)
        {
            if (ModelState.IsValid)
            {
                _bll.BillPayment.Add(vm.BillPayment);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            vm.BillSelectList = new SelectList(
                await _bll.Bill.GetAllAsync(),
                nameof(Bill.Id),
                nameof(Bill.Number),
                vm.BillPayment.BillId);
            
            vm.PaymentSelectList = new SelectList(
                await _bll.Payment.GetAllAsync(),
                nameof(Bill.Id),
                nameof(Bill.Number),
                vm.BillPayment.PaymentId);

            // ViewData["BillId"] = new SelectList(_context.Bills, "Id", "Id", billPayment.BillId);
            // ViewData["PaymentId"] = new SelectList(_context.Payments, "Id", "Id", billPayment.PaymentId);
            return View(vm);
        }

        // GET: BillPayment/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var billPayment = await _bll.BillPayment.FirstOrDefaultAsync(id.Value);
            
            if (billPayment == null)
            {
                return NotFound();
            }

            var vm = new BillPaymentCreateEditVm();
            
            vm.BillPayment = billPayment;
            
            vm.BillSelectList = new SelectList(
                await _bll.Bill.GetAllAsync(),
                nameof(Bill.Id),
                nameof(Bill.Number),
                vm.BillPayment.BillId);
            
            vm.PaymentSelectList = new SelectList(
                await _bll.Payment.GetAllAsync(),
                nameof(Bill.Id),
                nameof(Bill.Number),
                vm.BillPayment.PaymentId);
            
            return View(vm);
        }

        // POST: BillPayment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, BillPaymentCreateEditVm vm)
        {
            if (id != vm.BillPayment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var billPaymentFromDb = await _bll.BillPayment.FirstOrDefaultAsync(vm.BillPayment.Id);

                if (billPaymentFromDb == null)
                {
                    return NotFound();
                }
                
                try
                {
                    
                    _bll.BillPayment.Update(vm.BillPayment);
                    await _bll.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await BillPaymentExists(vm.BillPayment.Id))
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
                vm.BillPayment.BillId);
            
            vm.PaymentSelectList = new SelectList(
                await _bll.Payment.GetAllAsync(),
                nameof(Bill.Id),
                nameof(Bill.Number),
                vm.BillPayment.PaymentId);
            
            return View(vm);
        }

        // GET: BillPayment/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var billPayment = await _bll.BillPayment.FirstOrDefaultAsync(id.Value);
            
            if (billPayment == null)
            {
                return NotFound();
            }

            return View(billPayment);
        }

        // POST: BillPayment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            
            await _bll.BillPayment.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> BillPaymentExists(Guid id)
        {
            return await _bll.BillPayment.ExistsAsync(id);
        }
    }
}
