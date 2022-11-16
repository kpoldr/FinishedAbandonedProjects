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
    public class PersonController : Controller
    {
        private readonly IAppBLL _bll;

        public PersonController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Person
        public async Task<IActionResult> Index()
        {
            var res = await _bll.Person.GetAllAsync(User.GetUserId());
            return View(res);
            
        }

        // GET: Person/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var person = await _bll.Person.FirstOrDefaultAsync(id.Value);
            
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // GET: Person/Create
        public async Task<IActionResult> Create()
        {
            var vm = new PersonCreateEditVm();
            
            vm.ApartmentSelectList = new SelectList(
                await _bll.Apartment.GetAllAsync(),
                nameof(Bill.Id),
                nameof(Bill.Number));

            return View(vm);
        }

        // POST: Person/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PersonCreateEditVm vm)
        {
            if (ModelState.IsValid)
            {
                _bll.Person.Add(vm.Person);
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            vm.ApartmentSelectList = new SelectList(
                await _bll.Apartment.GetAllAsync(),
                nameof(Bill.Id),
                nameof(Bill.Number),
                vm.Person.Apartment);
            
            return View(vm);
        }

        // GET: Person/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var person = await _bll.Person.FirstOrDefaultAsync(id.Value);
            
            if (person == null)
            {
                return NotFound();
            }

            var vm = new PersonCreateEditVm();
            
            vm.Person = person;
            
            vm.ApartmentSelectList = new SelectList(
                await _bll.Apartment.GetAllAsync(),
                nameof(Bill.Id),
                nameof(Bill.Number),
                vm.Person.Apartment);
            
            return View(vm);
        }

        // POST: Person/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PersonCreateEditVm vm)
        {
            if (id != vm.Person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var personFromDb = await _bll.Person.FirstOrDefaultAsync(vm.Person.Id);

                if (personFromDb == null)
                {
                    return NotFound();
                }
                
                try
                {
                    personFromDb.Name?.SetTranslation(vm.Person.Name ?? "");
                    vm.Person.Name = personFromDb.Name;
                    
                    _bll.Person.Update(vm.Person);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await PersonExists(vm.Person.Id))
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
                nameof(Bill.Id),
                nameof(Bill.Number),
                vm.Person.Apartment);
            
            return View(vm);
        }

        // GET: Person/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var person = await _bll.Person.FirstOrDefaultAsync(id.Value);
            
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.Person.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> PersonExists(Guid id)
        {
            return await _bll.Person.ExistsAsync(id);
        }
    }
}
