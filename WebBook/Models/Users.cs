using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebBook.Models
{
    public partial class Users
    {
        public Users()
        {
            History = new HashSet<History>();
        }

        public int Id { get; set; }

        [StringLength(15, MinimumLength = 3)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [StringLength(35, MinimumLength = 5)]
        [Display(Name = "Почта")]
        public string Email { get; set; }

        [StringLength(15, MinimumLength = 3)]
        [Display(Name = "Роль")]
        public string Role { get; set; }

        public ICollection<History> History { get; set; }
    }
}
