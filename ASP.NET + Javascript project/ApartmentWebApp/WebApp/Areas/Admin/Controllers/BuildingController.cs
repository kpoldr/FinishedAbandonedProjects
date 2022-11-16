#nullable disable
using App.Contracts.BLL;
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
    public class BuildingController : Controller
    {
        private readonly IAppBLL _bll;

        public BuildingController(IAppBLL bll)
        {
            _bll = bll;
        }

        // GET: Building
        public async Task<IActionResult> Index()
        {
            
            var res = await _bll.Building.GetAllAsync(User.GetUserId());
            return View(res);
            
        }

        // GET: Building/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var building = await _bll.Building.FirstOrDefaultAsync(id.Value);
            
            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }

        // GET: Building/Create
        public async Task<IActionResult> Create()
        {
            var vm = new BuilidngCreateEditVM();
            
            vm.AssociationSelectList = new SelectList(
                await _bll.Association.GetAllAsync(),
                nameof(App.BLL.DTO.Association.Id),
                nameof(App.BLL.DTO.Association.Name));

            return View(vm);
        }

        // POST: Building/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BuilidngCreateEditVM vm)
        {
            if (ModelState.IsValid)
            {
             
                _bll.Building.Add(vm.Building);

                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            vm.AssociationSelectList = new SelectList(
                await _bll.Association.GetAllAsync(),
                nameof(App.BLL.DTO.Association.Id),
                nameof(App.BLL.DTO.Association.Name),
                vm.Building.AssociationId);
            
            return View(vm);
        }

        // GET: Building/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var building = await _bll.Building.FirstOrDefaultAsync(id.Value);
            
            if (building == null)
            {
                return NotFound();
            }

            var vm = new BuilidngCreateEditVM();

            vm.Building = building;
            
            vm.AssociationSelectList = new SelectList(
                await _bll.Association.GetAllAsync(),
                nameof(App.BLL.DTO.Association.Id),
                nameof(App.BLL.DTO.Association.Name),
                vm.Building.AssociationId);
            
            return View(vm);
        }

        // POST: Building/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, BuilidngCreateEditVM vm)
        {
            if (id != vm.Building.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var buildingFromDb = await _bll.Building.FirstOrDefaultAsync(vm.Building.Id);

                if (buildingFromDb == null)
                {
                    return NotFound();
                }
                
                try
                {
                    buildingFromDb.Address.SetTranslation(vm.Building.Address);

                    vm.Building.Address = buildingFromDb.Address;

                    _bll.Building.Update(vm.Building);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await BuildingExists(vm.Building.Id))
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
                nameof(App.BLL.DTO.Association.Id),
                nameof(App.BLL.DTO.Association.Name),
                vm.Building.AssociationId);
            
            // ViewData["AssociationId"] = new SelectList(_context.Associations, "Id", "Email", building.AssociationId);
            return View(vm);
        }

        // GET: Building/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var building = await _bll.Building.FirstOrDefaultAsync(id.Value);
            
            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }

        // POST: Building/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _bll.Building.RemoveAsync(id);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> BuildingExists(Guid id)
        {
            return await _bll.Building.ExistsAsync(id);
        }
    }
}
