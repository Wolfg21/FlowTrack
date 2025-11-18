namespace FlowTrack.Domain.Transactions;

public sealed record PersonalFinanceCategory(
    PrimaryCategory Primary,
    DetailedCategory Detailed,
    CategoryDescription Description
);