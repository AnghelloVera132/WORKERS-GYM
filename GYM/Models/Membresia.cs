using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYM.Models
{
    public class Membresia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MiembroId { get; set; }

        [ForeignKey("MiembroId")]
        public Miembro? Miembro { get; set; }

        [Required]
        public string Tipo { get; set; }

        [Required]
        public decimal Precio { get; set; }

        public DateTime FechaInicio { get; set; } = DateTime.Now;

        public DateTime FechaFin { get; set; }

        public string Estado { get; set; } = "Activa";
    }
}