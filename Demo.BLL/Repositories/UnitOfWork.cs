using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using System;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Lazy<IEmployeeRepository> employeeRepository;
        private readonly Lazy<IDepartmentRepository> departmentRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            employeeRepository = new Lazy<IEmployeeRepository>(new EmployeeRepository(_context));
            departmentRepository = new Lazy<IDepartmentRepository>(new DepartmentRepository(_context));
        }

        public IEmployeeRepository EmployeeRepository => employeeRepository.Value;
        public IDepartmentRepository DepartmentRepository => departmentRepository.Value;

        public async Task<int> CompleteAsync()
            => await _context.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            => await _context.DisposeAsync();
    }
}