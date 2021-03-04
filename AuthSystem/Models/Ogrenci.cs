using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Models
{
    public class Ogrenci
    {
        [Key]
        public int ID { get; set; }
        public string adi { get; set; }
        public string soyadi { get; set; }
        public string adres { get; set; }

        public string Ulkesi { get; set; }

        public string ResimYolu { get; set; }
        [NotMapped]
        public IFormFile ResimDosyası { get; set; }
    }
}
