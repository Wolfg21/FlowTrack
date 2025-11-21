namespace FlowTrack.Domain.Institutions;

public sealed class InstitutionBranding
{
    public InstitutionBranding(
        string? primaryColorHex,
        string? logo,
        string? url)
    {
        PrimaryColorHex = primaryColorHex;
        Logo = logo;
        Url = url;
    }
    public string? PrimaryColorHex { get; private set; }

    public string? Logo { get; private set; }

    public string? Url { get; private set; }
}