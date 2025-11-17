using FlowTrack.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTrack.Domain.Accounts;
public sealed class Balances
{
    public Balances(
        Money? available, 
        Money? current, 
        Money? limit, 
        string? isoCurrencyCode, 
        string? unofficialCurrencyCode)
    {
        Available = available;
        Current = current;
        Limit = limit;
        IsoCurrencyCode = isoCurrencyCode;
        UnofficialCurrencyCode = unofficialCurrencyCode;
    }
    
    public Money? Available { get; private set; }
    
    public Money? Current { get; private set; }

    public Money? Limit { get; private set; }
    
    public string? IsoCurrencyCode { get; private set; }

    public string? UnofficialCurrencyCode { get; private set; }
}
