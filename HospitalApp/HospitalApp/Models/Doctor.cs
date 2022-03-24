using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalApp.Models
{
    public class Doctor
    {
        //всички полета са задължителни полетата за име трябва да започват с голяма буква и да съдържат само български символи
        //полето за ЕГН трябва да е 10-цифрено и само числа
        [Required(ErrorMessage ="Полето за име не може да е празно") , RegularExpression(@"^[А-Я]{1}[а-я]+", ErrorMessage = "Името не е въведено правилно")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Полето за презиме не може да е празно"), RegularExpression(@"^[А-Я]{1}[а-я]+", ErrorMessage = "Презимето не е въведено правилно")]
        public string MiddleName { get; set; }
        [Required(ErrorMessage = "Полето за фамилия не може да е празно"), RegularExpression(@"^[А-Я]{1}[а-я]+", ErrorMessage = "Фамилията не е въведено правилно")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Полето за специалност не може да е празно"), RegularExpression(@"[А-Я][а-я]+", ErrorMessage = "Специалността не е въведена правилно")]
        public string Type { get; set; }
        [Required(ErrorMessage = "Полето за ЕГН не може да е празно"), RegularExpression(@"^[0-9]{10}", ErrorMessage = "ЕГН не е въведено правилно")]
        public string NIN { get; set; }

    }
}
