using System;
using System.Collections.Generic;
using System.Linq;

namespace FlowTrack.Domain.Shared;

public record Currency
{
    internal static readonly Currency None = new("", true);

    // Plaid supports all ISO 4217 currency codes and the following unofficial codes.
    public static readonly IReadOnlyCollection<string> PlaidUnofficialCurrencyCodes = new[]
    {
        "ADA", // Cardano
        "BAT", // Basic Attention Token
        "BCH", // Bitcoin Cash
        "BNB", // Binance Coin
        "BTC", // Bitcoin
        "BTG", // Bitcoin Gold
        "BSV", // Bitcoin Satoshi Vision
        "CNH", // Chinese Yuan (offshore)
        "DASH",
        "DOGE",
        "ETC", // Ethereum Classic
        "ETH", // Ethereum
        "GBX", // Pence sterling
        "LSK", // Lisk
        "NEO",
        "OMG", // OmiseGO
        "QTUM",
        "USDT", // Tether
        "XLM", // Stellar Lumen
        "XMR", // Monero
        "XRP", // Ripple
        "ZEC", // Zcash
        "ZRX"  // 0x
    };

    // Plaid ISO-4217 currency codes (hard-coded for validation).
    public static readonly IReadOnlyCollection<string> PlaidIsoCurrencyCodes = new[]
    {
        "AED", "AFN", "ALL", "AMD", "ANG", "AOA", "ARS", "AUD", "AWG", "AZN",
        "BAM", "BBD", "BDT", "BGN", "BHD", "BIF", "BMD", "BND", "BOB", "BOV",
        "BRL", "BSD", "BTN", "BWP", "BYN", "BZD",
        "CAD", "CDF", "CHE", "CHF", "CHW", "CLF", "CLP", "CNY", "COP", "COU",
        "CRC", "CUC", "CUP", "CVE", "CZK",
        "DJF", "DKK", "DOP", "DZD",
        "EGP", "ERN", "ETB", "EUR",
        "FJD", "FKP",
        "GBP", "GEL", "GHS", "GIP", "GMD", "GNF", "GTQ", "GYD",
        "HKD", "HNL", "HRK", "HTG", "HUF",
        "IDR", "ILS", "INR", "IQD", "IRR", "ISK",
        "JMD", "JOD", "JPY",
        "KES", "KGS", "KHR", "KMF", "KPW", "KRW", "KWD", "KYD", "KZT",
        "LAK", "LBP", "LKR", "LRD", "LSL", "LYD",
        "MAD", "MDL", "MGA", "MKD", "MMK", "MNT", "MOP", "MRU", "MUR", "MVR", "MWK", "MXN", "MXV", "MYR", "MZN",
        "NAD", "NGN", "NIO", "NOK", "NPR", "NZD",
        "OMR",
        "PAB", "PEN", "PGK", "PHP", "PKR", "PLN", "PYG",
        "QAR",
        "RON", "RSD", "RUB", "RWF",
        "SAR", "SBD", "SCR", "SDG", "SEK", "SGD", "SHP", "SLE", "SLL", "SOS", "SRD", "SSP", "STN", "SVC", "SYP", "SZL",
        "THB", "TJS", "TMT", "TND", "TOP", "TRY", "TTD", "TWD", "TZS",
        "UAH", "UGX", "USD", "USN", "UYI", "UYU", "UYW", "UZS",
        "VED", "VES", "VND", "VUV",
        "WST",
        "XAF", "XAG", "XAU", "XBA", "XBB", "XBC", "XBD", "XCD", "XDR", "XOF", "XPD", "XPF", "XPT", "XSU", "XTS", "XUA", "XXX",
        "YER",
        "ZAR", "ZMW", "ZWL"
    };

    private static readonly HashSet<string> UnofficialLookup =
        new(PlaidUnofficialCurrencyCodes, StringComparer.OrdinalIgnoreCase);

    private static readonly HashSet<string> IsoLookup =
        new(PlaidIsoCurrencyCodes, StringComparer.OrdinalIgnoreCase);

    private Currency(string code, bool isUnofficial)
    {
        Code = code.ToUpperInvariant();
        IsUnofficial = isUnofficial;
    }

    public string Code { get; init; }

    public bool IsUnofficial { get; init; }

    public static Currency FromCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ApplicationException("Currency code is required.");
        }

        var normalized = code.Trim().ToUpperInvariant();

        if (!LooksLikeThreeLetterCode(normalized))
        {
            throw new ApplicationException("Currency code must be a three-letter string.");
        }

        if (UnofficialLookup.Contains(normalized))
        {
            return new Currency(normalized, true);
        }

        if (IsoLookup.Contains(normalized))
        {
            return new Currency(normalized, false);
        }

        throw new ApplicationException("Currency code is not recognized as ISO 4217 or Plaid unofficial.");
    }

    public static Currency FromPlaid(string? isoCurrencyCode, string? unofficialCurrencyCode)
    {
        if (!string.IsNullOrWhiteSpace(isoCurrencyCode) && !string.IsNullOrWhiteSpace(unofficialCurrencyCode))
        {
            throw new ApplicationException("Currency code must be either ISO or unofficial, not both.");
        }

        if (!string.IsNullOrWhiteSpace(unofficialCurrencyCode))
        {
            return FromCode(unofficialCurrencyCode);
        }

        if (!string.IsNullOrWhiteSpace(isoCurrencyCode))
        {
            return FromCode(isoCurrencyCode);
        }

        return None;
    }

    private static bool LooksLikeThreeLetterCode(string code) =>
        code.Length == 3 && code.All(char.IsLetter);
}
