using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositorios;
using System.ComponentModel.DataAnnotations;

namespace Validaciones
{
    class Validaciones
    {

    }

    public sealed class CedulaUnica : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string cedula = Convert.ToString(value);
            bool unica = FachadaExpress.ValidarCedula(cedula);
            if (unica)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }

        }
    }
    public sealed class CelularUnico : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string celular = Convert.ToString(value);
            bool unico = FachadaExpress.NuevoCelular(celular);
            bool valido = FachadaExpress.CelularValido(celular);
            if (valido && unico)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }
    public sealed class PassValida : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string pass = Convert.ToString(value);
            bool valido = FachadaExpress.ValidarPassword(pass);
            if (valido)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }
    public sealed class EmailUnico : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string mail = Convert.ToString(value);
            bool valido = FachadaExpress.NuevoMail(mail);
            if (valido)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }
    public sealed class TituloUnico : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
     
            string titulo = Convert.ToString(value);
            bool valido = FachadaExpress.NuevoTitulo(titulo);
            if (valido)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }

    public sealed class RangoCuotas : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int cuotasIngresadas = Convert.ToInt32(value);
            int max = FachadaExpress.TraerCuotaMax();
            int min = FachadaExpress.TraerCuotaMin();
            if (cuotasIngresadas >= min && cuotasIngresadas <= max)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }
    public sealed class EdadUsuario : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime FechaNacimiento = Convert.ToDateTime(value);

            DateTime hoy = DateTime.Today;

            DateTime limite = hoy.AddYears(-21);
            RepoUsuarios usu = new RepoUsuarios();
            if (FechaNacimiento <= limite)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }
}

