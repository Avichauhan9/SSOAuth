

namespace SSO_Backend.Models.Mailing;

public class Attachment
{
    public string ContentType { get; set; } = null!;
    public byte[] Data { get; set; } = null!;
    public string FileName { get; set; } = null!;
}
