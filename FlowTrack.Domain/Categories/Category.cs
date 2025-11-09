using FlowTrack.Domain.Abstractions;
using FlowTrack.Domain.Categories.Events;
using System;

namespace FlowTrack.Domain.Categories;

public sealed class Category : Entity
{
    private Category(
        Guid id,
        Guid? userId,
        string name,
        CategoryType type,
        string? color,
        string? icon,
        Guid? parentCategoryId,
        bool isSystemCategory,
        bool isActive,
        int displayOrder) : base(id)
    {
        UserId = userId;
        Name = name;
        Type = type;
        Color = color;
        Icon = icon;
        ParentCategoryId = parentCategoryId;
        IsSystemCategory = isSystemCategory;
        IsActive = isActive;
        DisplayOrder = displayOrder;
    }

    private Category()
    {
    }

    public Guid? UserId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public CategoryType Type { get; private set; }

    public string? Color { get; private set; }

    public string? Icon { get; private set; }

    public Guid? ParentCategoryId { get; private set; }

    public bool IsSystemCategory { get; private set; }

    public bool IsActive { get; private set; }

    public int DisplayOrder { get; private set; }

    public static Category CreateSystem(
        string name,
        CategoryType type,
        int displayOrder,
        string? color = null,
        string? icon = null,
        Guid? parentCategoryId = null)
    {
        var category = new Category(
            Guid.NewGuid(),
            null,
            name,
            type,
            color,
            icon,
            parentCategoryId,
            true,
            true,
            displayOrder);

        category.RaiseDomainEvent(new CategoryCreatedDomainEvent(category.Id, null, true));

        return category;
    }

    public static Category CreateCustom(
        Guid userId,
        string name,
        CategoryType type,
        int displayOrder,
        string? color = null,
        string? icon = null,
        Guid? parentCategoryId = null)
    {
        var category = new Category(
            Guid.NewGuid(),
            userId,
            name,
            type,
            color,
            icon,
            parentCategoryId,
            false,
            true,
            displayOrder);

        category.RaiseDomainEvent(new CategoryCreatedDomainEvent(category.Id, userId, false));

        return category;
    }

    public void Rename(string name)
    {
        Name = name;
        Touch();
    }

    public void SetColor(string? color)
    {
        Color = color;
        Touch();
    }

    public void SetIcon(string? icon)
    {
        Icon = icon;
        Touch();
    }

    public void SetParent(Guid? parentCategoryId)
    {
        ParentCategoryId = parentCategoryId;
        Touch();
    }

    public void SetDisplayOrder(int order)
    {
        DisplayOrder = order;
        Touch();
    }

    public void Activate()
    {
        if (IsActive)
        {
            return;
        }

        IsActive = true;
        Touch();
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        Touch();
    }

    public void ChangeType(CategoryType type)
    {
        Type = type;
        Touch();
    }
}
