using System;
using System.ComponentModel.DataAnnotations;

namespace GYM.Models
{
    public class Rutina
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        public string DiaSemana { get; set; } 

        public string EnfoqueMuscular { get; set; }

        public string Descripcion { get; set; }
    }
}