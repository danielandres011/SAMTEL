using RESERVAS.API.Models;
using System.ComponentModel.DataAnnotations;

namespace RESERVAS.API.Entities
{
    public class Reserva : ISoftDetete
    {
        [Key]
        [Display(Name = "Identificador")]
        public int ID { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Usuario ID_USUARIO { get; set; }

        [Display(Name = "Hotel")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Hotel ID_HOTEL { get; set; }

        [Display(Name = "Habitación")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public int ID_HABITACION { get; set; }

        [Display(Name = "Fecha de entrada")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime CHECKIN { get; set; }

        [Display(Name = "Fecha de salida")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public DateTime CHECKOUT { get; set; }

        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Boolean ESTADO { get; set; }
    }
}
