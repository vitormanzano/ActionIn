using System.ComponentModel.DataAnnotations;

namespace ActionIn.Authentication.AntiCorruption;

public class SmtpSettings
{
    public const string SectionName = "Smtp";

    [Required]
    public string Host { get; set; } = string.Empty;

    [Range(1, 65535)]
    public int Port { get; set; }

    public string? Username { get; set; }
    public string? Password { get; set; }

    [Required]
    public string FromName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string FromAddress { get; set; } = string.Empty;
}
