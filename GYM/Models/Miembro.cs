using System;
using System.ComponentModel.DataAnnotations;

namespace GYM.Models
{
    public class Miembro
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        public string DNI { get; set; }

        [Required]
        public string Correo { get; set; }

        [Required]
        public string Contraseña { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}