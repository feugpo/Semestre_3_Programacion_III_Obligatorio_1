using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Validaciones;

namespace MVC.Models
{
    public class ViewModelProyectoCoop
    {
        public int Id { get; set; }
        public int Solicitante { get; set; }
        [Required]
        [TituloUnico(ErrorMessage = "El nombre del proyecto ingresado ya existe.")]
        [StringLength(50)]
        public string Titulo { get; set; }
        [Required]
        [StringLength(150)]
        public string Descripcion { get; set; }
        [Required]
        public decimal Monto { get; set; }
        [Required]
        [Range(1,10000, ErrorMessage = "Ingrese un numero positivo.")]
        [RangoCuotas(ErrorMessage = "El valor de cuotas ingresado está fuera de rango.")]
        public int Cuotas { get; set; }

        public HttpPostedFileBase Imagen { get; set; } //COMO ES EL FORMATO?

        public decimal TasaInteres { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Ingrese un numero positivo.")]
        public int Integrantes { get; set; }
        
        [StringLength(150)]
        public string Experiencia { get; set; }
     
        public string Tipo { get; set; }

        public string Estado { get; set; }
 
        public DateTime FechaCreacion { get; set; }
    }
}
