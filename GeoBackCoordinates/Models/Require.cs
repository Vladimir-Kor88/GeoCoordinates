using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GeoBackCoordinates.Models
{
    public class Require
    {
        [Required(ErrorMessage = "Не указан адрес")]
        public string address { get; set; }
   
        [Range(1, 50, ErrorMessage = "Недопустимое значение")]
        public int frequency { get; set; }

        [Required(ErrorMessage = "Не указан файл")]
        public string file { get; set; }
    }
}
