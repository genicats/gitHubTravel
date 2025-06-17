//這個是用來代表一筆支出的Entity
//這個Entity 會對應到資料庫中的Expense資料表
//這個Entity 會被Entity Framework Core用來建立資料庫中的表格
//這個Entity 會有幾個欄位，分別是Id、Amount、Date和Description
//Id 是一個整數，是這筆支出的唯一識別碼
//Amount 是一個浮點數，是這筆支出的金額
//Date 是一個日期時間，是這筆支出的日期
//Description 是一個字串，是這筆支出的描述
//Category 是一個字串，是這筆支出的類別
namespace ExpenseAPI.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
    }
}