namespace Ratatoskr.Core.Models;

public class Customer
{
    public int Id { get; set; }

    // 🔀 Privatkunde oder Firma
    public bool IsCompany { get; set; } = false;

    // 🧾 Firmenkunde
    public string? CompanyName { get; set; }

    // 👤 Privatkunde
    public string Prename { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;

    // 📫 Adresse
    public string Address { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    // ☎ Kontakt
    public string? Email { get; set; }
    public string? Phone { get; set; }

    // ⚠ Aktivstatus (für Löschvermerk)
    public bool IsActive { get; set; } = true;

    // 📘 Für Anzeigezwecke
    public string FullName => IsCompany
        ? CompanyName ?? "Unbenannte Firma"
        : $"{Prename} {Surname}";
}
