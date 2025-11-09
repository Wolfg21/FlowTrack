using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FlowTrack.Domain.ValueObjects;

public sealed record Currency
{
    private static readonly Dictionary<string, Currency> Registry = new(StringComparer.OrdinalIgnoreCase);
    private static readonly ReadOnlyDictionary<string, Currency> ReadOnlyRegistry =
        new(Registry);

    internal static readonly Currency None = Register("");

    public static readonly Currency Usd = Register("USD");
    public static readonly Currency Eur = Register("EUR");
    public static readonly Currency Gbp = Register("GBP");
    public static readonly Currency Cad = Register("CAD");

    private Currency(string code)
    {
        Code = code;
    }

    public string Code { get; }

    public static Currency FromCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Currency code can not be empty.", nameof(code));
        }

        var normalized = code.Trim().ToUpperInvariant();

        if (Registry.TryGetValue(normalized, out var currency))
        {
            return currency;
        }

        var created = new Currency(normalized);
        Registry[normalized] = created;
        return created;
    }

    public static IReadOnlyCollection<Currency> All => ReadOnlyRegistry.Values;

    private static Currency Register(string code)
    {
        var currency = new Currency(code);
        Registry[code] = currency;
        return currency;
    }
}
