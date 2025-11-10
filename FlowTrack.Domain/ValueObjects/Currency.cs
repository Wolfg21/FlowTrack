using System;
using System.Collections.Generic;
using System.Linq;

namespace FlowTrack.Domain.ValueObjects;

public record Currency
{
    internal static readonly Currency None = new("");
    public static readonly Currency Usd = new("USD");
    public static readonly Currency Eur = new("EUR");
    public static readonly Currency Gbp = new("GBP");
    public static readonly Currency Cad = new("CAD");
    public static readonly Currency Aud = new("AUD");
    public static readonly Currency Jpy = new("JPY");
    public static readonly Currency Chf = new("CHF");
    public static readonly Currency Sek = new("SEK");

    private Currency(string code) => Code = code;

    public string Code { get; init; }

    public static Currency FromCode(string code)
    {
        return All.FirstOrDefault(c => string.Equals(c.Code, code, StringComparison.OrdinalIgnoreCase)) ??
               throw new ApplicationException("The currency code is invalid");
    }

    public static readonly IReadOnlyCollection<Currency> All = new[]
    {
        Usd,
        Eur,
        Gbp,
        Cad,
        Aud,
        Jpy,
        Chf,
        Sek
    };
}
