using FlowTrack.Application.Abstractions.Messaging;
using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.Institutions;
using FlowTrack.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowTrack.Application.Institutions.UpsertInstitutions;
internal sealed class UpsertInstitutionCommandHandler : ICommandHandler<UpsertInstitutionCommand, Guid>
{
    private readonly IInstitutionRepository _institutionRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpsertInstitutionCommandHandler(IInstitutionRepository institutionRepository, IUnitOfWork unitOfWork)
    {
        _institutionRepository = institutionRepository;
        _unitOfWork = unitOfWork;
    }
    private Country? MapCountry(string code) => Enum.TryParse<Country>(code, true, out var c) ? c : null;
    private Product? MapProduct(string name) => Enum.TryParse<Product>(name, true, out var p) ? p : null;

    public async Task<Result<Guid>> Handle(UpsertInstitutionCommand request, CancellationToken cancellationToken)
    {
        var plaidId = new ExternalId(request.PlaidInsitutionId);

        var institution = await _institutionRepository.GetByPlaidIdAsync(plaidId, cancellationToken);

        var countries = request.CountryCodes
            .Select(MapCountry)
            .OfType<Country>().ToList();
        var products = request.Products
            .Select(MapProduct)
            .OfType<Product>()
            .ToList();

        var branding = new InstitutionBranding(request.PrimaryColorHex, request.Logo, request.Url);

        if(institution is not null) { return Result.Success(institution.Id.Value); }

        institution = Institution.Create(
            plaidId, 
            new InstitutionName(request.Name), 
            countries, 
            products, 
            branding);
        
        _institutionRepository.Add(institution);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(institution.Id.Value);
    }
}
