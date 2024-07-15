using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        // Private Attribute Of object implements IDepartmentRepository FRom BLL
        private readonly IUnitOfWork _unitOfWork; // Null

        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper) // Ask CLR To Create Object From DeprtmentRepository// Allows DEpaendancy Injection in CongigureServices Method on StartUp Class;
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //GetAll()
        public async Task<IActionResult> Index()
        {
            //ViewData["Message"] = "Hello Dept!";
            //ViewBag.Message = "sdds";
            var Departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            var ViewDepts = _mapper.Map<IEnumerable<DepartmentViewModel>>(Departments);
            return View(ViewDepts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Dept = _mapper.Map<Department>(model);
                _unitOfWork.DepartmentRepository.Add(Dept);
                int res = await _unitOfWork.CompleteAsync();
                if (res > 0)
                {
                    TempData["Message"] = "Department Add Successfully!!";
                }
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DetailsAsync(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var Model = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            var department = _mapper.Map<DepartmentViewModel>(Model);

            if (department == null)
                return NotFound();
            return View(ViewName, department);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await DetailsAsync(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int? id, DepartmentViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(model);
                _unitOfWork.DepartmentRepository.Update(department);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await DetailsAsync(id, "Delete");
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int? id, DepartmentViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(model);
                _unitOfWork.DepartmentRepository.Delete(department);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}