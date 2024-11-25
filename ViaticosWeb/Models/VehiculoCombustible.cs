using System;
using System.ComponentModel.DataAnnotations;

namespace ViaticosWeb.Models
{
    public class VehiculoCombustible
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "El kilometraje es requerido")]
        [Display(Name = "Kilometraje")]
        public decimal Kilometraje { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Display(Name = "Cantidad")]
        public decimal Cantidad { get; set; }

        [Required(ErrorMessage = "El grifo es requerido")]
        [Display(Name = "Grifo")]
        public int GrifoId { get; set; }

        [Required(ErrorMessage = "La gerencia es requerida")]
        [Display(Name = "Gerencia")]
        public int GerenciaId { get; set; }

        [Required(ErrorMessage = "La unidad es requerida")]
        [Display(Name = "Unidad")]
        public int UnidadId { get; set; }

        [Required(ErrorMessage = "La placa es requerida")]
        [Display(Name = "Placa")]
        public int PlacaId { get; set; }

        [Display(Name = "Marca")]
        public string Marca { get; set; }

        [Display(Name = "Año")]
        public string AnioVehiculo { get; set; }

        [Display(Name = "Tipo Vehículo")]
        public string TipoVehiculo { get; set; }

        [Display(Name = "Área Asignada")]
        public string AreaAsignada { get; set; }

        [Display(Name = "Asignado a")]
        public string Asignado { get; set; }

        [Display(Name = "Estado")]
        public string Estado { get; set; }

        [Display(Name = "Combustible")]
        public string Combustible { get; set; }

        public string CodigoActivo { get; set; }
        public string CodigoCombustible { get; set; }
        public int TipoCombustibleId { get; set; }
    }

    public class Grifo
    {
        public int Item { get; set; }
        public string Descripcion { get; set; }
    }

    public class Area
    {
        public int AreCod { get; set; }
        public string AreNom { get; set; }
    }

    public class Unidad
    {
        public int AreCod { get; set; }
        public int AreDep { get; set; }
        public string AreNom { get; set; }
    }

    public class Placa
    {
        public int Item { get; set; }
        public string VehPla { get; set; }
    }
}