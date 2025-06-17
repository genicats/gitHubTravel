using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ExpenseAPI.Models
{
    public static class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ExpenseContext(
                serviceProvider.GetRequiredService<DbContextOptions<ExpenseContext>>()))
            {
                // Look for any expenses.
                if (context.Expenses.Any())
                {
                    return;   // DB has been seeded
                }

                context.Expenses.AddRange(
                    new Expense
                    {
                        Description = "Lunch",
                        Amount = 12.50M,
                        Date = DateTime.Now,
                        Category = "Food"
                    },
                    new Expense
                    {
                        Description = "Taxi",
                        Amount = 25.00M,
                        Date = DateTime.Now,
                        Category = "Transport"
                    }
                );

                context.SaveChanges();
            }
        }

    }
}