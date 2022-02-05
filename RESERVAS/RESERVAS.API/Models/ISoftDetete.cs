using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RESERVAS.API.Models
{
    public class ISoftDetete
    {
        [Display(Name = "Creado")]
        public DateTime CREATED { get; set; }

        [Display(Name = "Modificado")]
        public DateTime UPDATED { get; set; }

        [Display(Name = "Eliminado")]
        public DateTime? DELETED { get; set; }
    }
}
