namespace ActionIn.Authentication.AntiCorruption;

public class SmtpSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string FromName { get; set; } = string.Empty;
    public string FromAddress { get; set; } = string.Empty;


}


