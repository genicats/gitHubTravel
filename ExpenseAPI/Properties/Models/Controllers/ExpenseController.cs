// 這個API Controller 會使用Expense.cs這個Entity來對應到資料庫中的Expense資料表
// 這個API Controller 會使用ExpenseContext.cs這個DbContext來對應到資料庫
// API 路徑是/api/Expense
// 這個API Controller 會有幾個方法，分別是Get、Post、Put和Delete，分別用來取得所有支出、建立新的支出、更新支出和刪除支出
// 這個API Controller 會使用依賴注入來取得ExpenseContext
// 這個API Controller 會使用ASP.NET Core的特性來處理HTTP請求和回應
// 這個API Controller 會使用ASP.NET Core的路由來定義API路徑

using ExpenseAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Net;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace ExpenseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ExpenseContext _context;
        private readonly ILogger<ExpenseController> _logger;

        public ExpenseController(ExpenseContext context, ILogger<ExpenseController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses()
        {
            return await _context.Expenses.ToListAsync();
        }

        // GET: api/expenses/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            return expense;
        }
        // POST: api/expenses
        // 如果 description  是 Lunch且date在早上11點前，則會回傳 400 Bad Request 並且回覆 【This Time Can not buy Lunch】
        /// <summary>
        /// Creates a new expense.
        /// This method accepts an Expense object, validates it, and adds it to the database.
        /// If the model state is invalid, it returns a BadRequest with the model state errors.
        /// If the expense description is "Lunch" and the date is before 11 AM,
        /// it returns a BadRequest with a specific message.
        /// If the expense is successfully created, it returns a CreatedAtAction result with the created expense and a 201 status code.
        /// </summary>
        /// <param name="expense"></param>
        /// <returns>expense物件</returns>
        [HttpPost]
        public async Task<ActionResult<Expense>> PostExpense(Expense expense)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for PostExpense: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            if (expense.Description == "Lunch" && expense.Date.TimeOfDay < new TimeSpan(11, 0, 0))
            {
                return BadRequest("This Time Can not buy Lunch");
            }


            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExpense), new { id = expense.Id }, expense);
        }
        // PUT: api/expenses/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(int id, Expense expense)
        {
            if (id != expense.Id)
            {
                _logger.LogWarning("Expense ID mismatch in PutExpense: {Id} != {ExpenseId}", id, expense.Id);
                return BadRequest();
            }

            _context.Entry(expense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }
        // DELETE: api/expenses/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                _logger.LogWarning("Attempted to delete non-existing expense with ID: {Id}", id);
                return NotFound();
            }   

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.Id == id);
        }
    }
}
// This code defines an API controller for managing expenses in an application.
// It includes methods for retrieving, creating, updating, and deleting expenses.
// The controller uses Entity Framework Core to interact with a database context
// and is secured with authorization attributes.