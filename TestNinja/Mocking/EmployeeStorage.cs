using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    public interface IEmployeeStorage
    {
        void DeleteEmployee(int id);
    }
    public class EmployeeStorage
    {
        private EmployeeContext _db;

        public EmployeeStorage()
        {
            _db = new EmployeeContext();
        }

        public void DeleteEmployee(int id)
        {
            var employee = _db.Employees.Find(id);

            if (employee != null)
            {
                _db.Employees.Remove(employee);
                _db.SaveChanges();
            }
        }
    }
}
