using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GYM.Models
{
    public class Asistencia
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MiembroId { get; set; }

        [Required]
        public DateTime FechaHoraEntrada { get; set; }

        [ForeignKey("MiembroId")]
        public Miembro? Miembro { get; set; }
    }
}