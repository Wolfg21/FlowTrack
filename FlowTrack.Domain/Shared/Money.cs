using System;

namespace FlowTrack.Domain.Shared;

public sealed record Money(decimal Amount, Currency Currency)
{
    public static Money operator +(Money first, Money second)
    {
        EnsureSameCurrency(first, second);
        return new Money(first.Amount + second.Amount, first.Currency);
    }

    public static Money operator -(Money first, Money second)
    {
        EnsureSameCurrency(first, second);
        return new Money(first.Amount - second.Amount, first.Currency);
    }

    public static Money Zero() => new(0, Currency.None);

    public static Money Zero(Currency currency) => new(0, currency);

    public static Money From(decimal amount, Currency currency) => new(amount, currency);

    public static Money From(decimal amount, string currencyCode) =>
        new(amount, Currency.FromCode(currencyCode));

    public static Money FromPlaid(decimal amount, string? isoCurrencyCode, string? unofficialCurrencyCode) =>
        new(amount, Currency.FromPlaid(isoCurrencyCode, unofficialCurrencyCode));

    public bool IsZero() => Amount == 0;

    public Money Negate() => new(-Amount, Currency);

    private static void EnsureSameCurrency(Money first, Money second)
    {
        if (first.Currency != second.Currency)
        {
            throw new InvalidOperationException("Currencies must match.");
        }
    }
}
