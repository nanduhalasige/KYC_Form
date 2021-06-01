using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KYC_Form.Data;
using KYC_Form.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace KYC_Form.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            return View(await _context.Employee.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult InsertNewKYC(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var IdString = "kyc_" + employee.FirstName[0] + "_";
                var rowCount = 0;
                try
                {
                    rowCount = InsertOrUpdateKYC(employee, IdString, "INSERT");

                    //_context.Add(employee);
                    //await _context.SaveChangesAsync();
                    if (rowCount > 0)
                        return Json(new { message = "Success" });
                    else
                        return Json(new { message = "Falied", data = employee });
                }
                catch (Exception ex)
                {
                    return Json(new { message = ex.Message, data = employee });
                }
            }
            return Json(new { message = "ModelState invalid", modelStateValid = false, data = employee });
        }

        private int InsertOrUpdateKYC(Employee employee, string Id, string ProcType)
        {
            var connection = (SqlConnection)_context.Database.GetDbConnection();
            var rows = 0;
            var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "sp_InsertUpdateClientData";

            command.Parameters.AddWithValue("@ProcType", ProcType);
            command.Parameters.AddWithValue("@FirstName", employee.FirstName);
            command.Parameters.AddWithValue("@LastName", employee.LastName);
            command.Parameters.AddWithValue("@ClientType", employee.ClientType);
            command.Parameters.AddWithValue("@Email", employee.Email);
            command.Parameters.AddWithValue("@MobileNumber", employee.MobileNumber);
            command.Parameters.AddWithValue("@Title", employee.Title);
            command.Parameters.AddWithValue("@Gender", employee.Gender);
            command.Parameters.AddWithValue("@Id_Prefix", Id);
            connection.Open();
            rows = command.ExecuteNonQuery();
            connection.Close();

            return rows;
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        public JsonResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var IdString = employee.Id;
                var rowCount = 0;
                try
                {
                    rowCount = InsertOrUpdateKYC(employee, IdString, "UPDATE");
                    if (rowCount > 0)
                        return Json(new { message = "Success" });
                    else
                        return Json(new { message = "Falied", data = employee });
                }
                catch (Exception ex)
                {
                    return Json(new { message = ex.Message, data = employee });
                }
            }
            return Json(new { message = "ModelState invalid", modelStateValid = false, data = employee });
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var employee = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(string id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}