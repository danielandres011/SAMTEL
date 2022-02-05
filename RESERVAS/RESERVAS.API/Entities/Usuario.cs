using RESERVAS.API.Models;
using System.ComponentModel.DataAnnotations;

namespace RESERVAS.API.Entities
{
    public class Usuario : ISoftDetete
    {
        [Key]
        [Display(Name = "Identificador")]
        public int ID { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String NOMBRE { get; set; }

        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String APELLIDOS { get; set; }

        [Display(Name = "EMAIL")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String EMAIL { get; set; }

        [Display(Name = "DIRECCION")]
        public String? DIRECCION { get; set; }
    }
}
