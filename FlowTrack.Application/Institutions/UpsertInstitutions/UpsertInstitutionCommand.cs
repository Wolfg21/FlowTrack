using FlowTrack.Application.Abstractions.Messaging;
using FlowTrack.Domain.Institutions;
using FlowTrack.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTrack.Application.Institutions.UpsertInstitutions;
public sealed record UpsertInstitutionCommand(
    string PlaidInsitutionId,
    string Name,
    List<string> CountryCodes,
    List<string> Products,
    string? PrimaryColorHex,
    string? Logo,
    string? Url) : ICommand<Guid>;
