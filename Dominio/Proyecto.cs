using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Proyecto
    {
        public int Id { get; set; }
        public int Solicitante { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public int Cuotas { get; set; }
        public string Imagen { get; set; } //COMO ES EL FORMATO?
        public decimal TasaInteres { get; set; }
        public int Integrantes { get; set; }
        public string Experiencia { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }

    }
}
