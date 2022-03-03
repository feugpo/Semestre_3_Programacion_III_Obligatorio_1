using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Validaciones;

namespace MVC.Models
{
    public class ViewModelUsuario
    {
        public int Id { get; set; }
        //[Required][MaxLength(10, ErrorMessage ="El nombre no puede tener más de 10 caracteres")]
        [Required]
        [CedulaUnica(ErrorMessage = "La cédula ingresada ya está registrada.")]
        [StringLength(8)]
        public string Cedula { get; set; }

        [Required]
        [StringLength(30)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(30)]
        public string Apellido { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha De Nacimiento")]
        [EdadUsuario(ErrorMessage = "Tiene que ser mayor a 21 años.")]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        [StringLength(9)]
        [CelularUnico(ErrorMessage = "El numero que ingresó ya está registrado.")]
        public string Celular { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$", ErrorMessage = "El correo ingresado no es válido.")]
        [EmailUnico(ErrorMessage = "El correo que ingresó ya está registrado.")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [StringLength(30)]
        [PassValida(ErrorMessage = "Contraseña debe tener al menos 6 caracteres, 1 mayúscula, 1 minúscula y un numero.")]
        public string Password { get; set; }

        
        public string Rol { get; set; }
    }
}

