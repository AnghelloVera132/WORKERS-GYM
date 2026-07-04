using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYM.Models
{
    public class Pago
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MembresiaId { get; set; }

        [ForeignKey("MembresiaId")]
        public Membresia? Membresia { get; set; }

        [Required]
        public decimal Monto { get; set; }

        public DateTime FechaPago { get; set; } = DateTime.Now;

        [Required]
        public string MetodoPago { get; set; }

        public string Estado { get; set; } = "Pagado";
    }
}