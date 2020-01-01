using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SikayetKayit.Models.ViewModel
{
    public class LoginViewModel
    {
        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage = "{0} Alanı Boş Geçilemez"), StringLength(25, ErrorMessage = "{0} max. {1} Karakter Olmalı.")]
        public string Username { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} Alanı Boş Geçilemez"), DataType(DataType.Password), StringLength(25, ErrorMessage = "{0} max. {1} Karakter Olmalı.")]
        public string Password { get; set; }
    }
}