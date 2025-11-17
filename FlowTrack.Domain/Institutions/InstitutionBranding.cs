namespace FlowTrack.Domain.Institutions;

public sealed class InstitutionBranding
{
    public InstitutionBranding(
        string primaryColorHex,
        string logoUrl)
    {
        PrimaryColorHex = primaryColorHex;
        LogoUrl = logoUrl;
    }
    public string? PrimaryColorHex { get; private set; }
    public string? LogoUrl { get; private set; }
}