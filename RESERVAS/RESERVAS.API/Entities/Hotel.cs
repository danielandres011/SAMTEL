using RESERVAS.API.Models;
using System.ComponentModel.DataAnnotations;

namespace RESERVAS.API.Entities
{
    public class Hotel : ISoftDetete
    {
        [Key]
        [Display(Name = "Identificador")]
        public int ID { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String NOMBRE { get; set; }

        [Display(Name = "País")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public String PAIS { get; set; }

        [Display(Name = "Latitud")]
        public Decimal? LATITUD { get; set; }

        [Display(Name = "Longitud")]
        public Decimal? LONGITUD { get; set; }

        [Display(Name = "DESCRIPCION")]
        public String? DESCRIPCION { get; set; }

        [Display(Name = "Activo")]
        public Boolean ACTIVO { get; set; }

        [Display(Name = "HABITACIONES")]
        public int HABITACIONES { get; set; }
    }
}
