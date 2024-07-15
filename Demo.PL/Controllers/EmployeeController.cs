using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        //private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(/*IEmployeeRepository employeeRepository*/ IUnitOfWork unitOfWork, IMapper mapper /*, IDepartmentRepository departmentRepository*/ ) // Di For Employee Repository
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            //_departmentRepository = departmentRepository;
        }

        public async Task<IActionResult> Index(string searchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(searchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetByName(searchInput);
            }

            var Result = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);

            return View(Result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }
                else
                {
                    model.ImageName = DocumentSettings.DefaultProfilePic;
                }

                var Emp = _mapper.Map<Employee>(model);
                _unitOfWork.EmployeeRepository.Add(Emp);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id is null)
                return BadRequest();

            var Emp = await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
            var ViewEmp = _mapper.Map<EmployeeViewModel>(Emp);
            if (Emp is null)
                return NotFound();

            return View(ViewEmp);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, nameof(Edit));
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel model)
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                if (!(DocumentSettings.IsDefaultPictureOrNull(model.ImageName)))
                {
                    DocumentSettings.DeleteFile(model.ImageName, "images");
                }
                model.ImageName = DocumentSettings.UploadFile(model.Image, "images");

                var Emp = _mapper.Map<Employee>(model);
                _unitOfWork.EmployeeRepository.Update(Emp);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id, nameof(Delete));
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int? id, EmployeeViewModel model)
        {
            if (id != model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                if (!(DocumentSettings.IsDefaultPictureOrNull(model.ImageName)))
                {
                    DocumentSettings.DeleteFile(model.ImageName, "images");
                }
                var Emp = _mapper.Map<Employee>(model);
                _unitOfWork.EmployeeRepository.Delete(Emp);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}