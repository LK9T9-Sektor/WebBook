using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public partial class Books
    {
        public Books()
        {
            History = new HashSet<History>();
        }

        public int Id { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Фото")]
        public byte[] Picture { get; set; }

        [StringLength(20, MinimumLength = 3)]
        [Display(Name = "Категория")]
        public string Category { get; set; }

        [StringLength(20, MinimumLength = 3)]
        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата публикации")]
        public DateTime Publish { get; set; }

        [Display(Name = "Номер публикации")]
        public int PublishNumber { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Издатель")]
        public string Publisher { get; set; }

        [Display(Name = "Страниц")]
        public int Pages { get; set; }

        [StringLength(30, MinimumLength = 3)]
        [Display(Name = "ISBN")]
        public string Isbn { get; set; }

        [StringLength(30, MinimumLength = 3)]
        [Display(Name = "Штрих код")]
        public string Barcode { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата регистрации")]
        public DateTime RegDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата списания")]
        public DateTime? OutDate { get; set; }

        [Display(Name = "Книга на руках?")]
        public bool Issued { get; set; }

        public ICollection<History> History { get; set; }
    }
}
