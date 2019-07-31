using System;
using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public partial class History
    {
        public int Id { get; set; }

        [Display(Name = "Книга")]
        public int BookId { get; set; }

        [Display(Name = "Читатель")]
        public int UserId { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата выдачи")]
        public DateTime Issue { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата возврата")]
        public DateTime? ReturnDate { get; set; }

        [Display(Name = "Книги")]
        public Books Book { get; set; }

        [Display(Name = "Читатель")]
        public Users User { get; set; }

        public History(int BookId, int UserId)
        {
            this.BookId = BookId;
            this.UserId = UserId;
            Issue = DateTime.Now;
        }
    }
}
