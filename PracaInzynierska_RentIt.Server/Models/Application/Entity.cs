using System.ComponentModel.DataAnnotations;

namespace PracaInzynierska_RentIt.Server.Models.Application;

public abstract class Entity
{
    public virtual Guid Id { get; set; }
    public virtual DateTime? ModifiedTime { get; set; }
    public virtual String? ModifiedBy { get; set; }
    [Required(ErrorMessage = "Pole CreateTime nie może być puste")]
    public virtual DateTime CreateTime { get; set; }
    [Required(ErrorMessage = "Pole CreateBy nie może być puste")]
    public virtual String CreateBy { get; set; }
}