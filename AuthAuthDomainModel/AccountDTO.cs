using System.ComponentModel.DataAnnotations;

namespace AuthAuthDomainModel;

[Serializable()]
public class AccountDTO
{
    [Required]
    public string Contact;
    [Required]
    public byte[] Login ;
    [Required]
    public byte[] Password;
}
