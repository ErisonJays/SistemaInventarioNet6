using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.Modelos
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required (ErrorMessage = "El nombre es requerido")]
        [MaxLength(60, ErrorMessage = "Nombre debe tener maximo 60 caracteres")]
        public string Nombre { get; set; }

        [MaxLength(100, ErrorMessage = "Descripcion debe tener maximo 100 caracteres")]
        [Required(ErrorMessage = "La descripcion es requerida")]
        public string Descripcion  { get; set; }

        [Required(ErrorMessage = "El estado es requerida")]
        public bool Estado { get; set; }


    }
}
