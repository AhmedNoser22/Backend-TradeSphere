namespace TradeSphere.Infrastructure.Email;
public sealed class EmailSettings
{
    public string SmtpHost { get; set; } = default!;
    public int SmtpPort { get; set; }
    public string SenderEmail { get; set; } = default!;
    public string SenderName { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public bool UseSsl { get; set; } = true;
}