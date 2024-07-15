using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IEmployeeRepository : IGerericRepository<Employee>
    {
        public Task<IEnumerable<Employee>> GetByName(string name);
    }
}