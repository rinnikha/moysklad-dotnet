using MoySklad.Api.Entities.Base;

namespace MoySklad.Api.Utils;

public static class EntityAttributeExtensions
{
    private const string BaseUrl = "https://api.moysklad.ru/api/remap/1.2/";

    // ── Read helpers ─────────────────────────────────────────────────────────

    public static EntityAttribute? GetAttribute(this List<EntityAttribute>? attributes, string name)
    {
        if (attributes == null || string.IsNullOrWhiteSpace(name))
            return null;

        return attributes.FirstOrDefault(a =>
            a.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) == true);
    }

    public static EntityAttribute? GetAttributeById(this List<EntityAttribute>? attributes, string id)
    {
        if (attributes == null || string.IsNullOrWhiteSpace(id))
            return null;

        return attributes.FirstOrDefault(a => a.Id == id);
    }

    public static object? GetValue(this List<EntityAttribute>? attributes, string name)
        => attributes.GetAttribute(name)?.Value;

    public static bool HasAttribute(this List<EntityAttribute>? attributes, string name)
        => attributes.GetAttribute(name) != null;

    // ── Meta builder ─────────────────────────────────────────────────────────

    /// <summary>
    /// Builds the required attribute Meta for the MoySklad API.
    /// <para>
    /// <paramref name="entityTypePath"/> is the entity segment used in the repository,
    /// e.g. <c>"entity/product"</c>, <c>"entity/customerorder"</c>.
    /// </para>
    /// <para>
    /// <paramref name="attributeId"/> is the UUID of the attribute metadata record.
    /// </para>
    /// Result href: <c>https://api.moysklad.ru/api/remap/1.2/{entityTypePath}/metadata/attributes/{attributeId}</c>
    /// </summary>
    public static Meta BuildAttributeMeta(string entityTypePath, string attributeId)
    {
        if (string.IsNullOrWhiteSpace(entityTypePath))
            throw new ArgumentException("entityTypePath cannot be null or empty", nameof(entityTypePath));
        if (string.IsNullOrWhiteSpace(attributeId))
            throw new ArgumentException("attributeId cannot be null or empty", nameof(attributeId));

        var href = $"{BaseUrl}{entityTypePath.TrimStart('/')}/metadata/attributes/{attributeId}";

        return new Meta
        {
            Href = href,
            Type = "attributemetadata",
            MediaType = "application/json"
        };
    }

    // ── Upsert (unified set/add) ──────────────────────────────────────────────

    /// <summary>
    /// Upserts an attribute by id: removes any existing attribute with the same id,
    /// then adds a fresh one with the provided value.
    /// <para>
    /// The required Meta (href, type, mediaType) is built automatically from
    /// <paramref name="entityTypePath"/> and <paramref name="attributeId"/>.
    /// Pass a pre-built <paramref name="meta"/> to override.
    /// </para>
    /// </summary>
    public static List<EntityAttribute> UpsertAttribute(
        this List<EntityAttribute>? attributes,
        string attributeId,
        object? value,
        string? entityTypePath = null,
        string? name = null,
        string? type = null,
        Meta? meta = null)
    {
        var list = attributes == null
            ? new List<EntityAttribute>()
            : attributes.Where(a => a.Id != attributeId).ToList();

        var resolvedMeta = meta
            ?? (entityTypePath != null
                ? BuildAttributeMeta(entityTypePath, attributeId)
                : null);

        list.Add(new EntityAttribute
        {
            Id = attributeId,
            Name = name,
            Value = value,
            Type = type ?? InferType(value),
            Meta = resolvedMeta,
        });

        return list;
    }

    // ── Remove ───────────────────────────────────────────────────────────────

    public static List<EntityAttribute> RemoveAttribute(
        this List<EntityAttribute>? attributes,
        string name)
    {
        if (attributes == null) return new List<EntityAttribute>();

        return attributes.Where(a =>
            !a.Name?.Equals(name, StringComparison.OrdinalIgnoreCase) == true).ToList();
    }

    public static List<EntityAttribute> RemoveAttributeById(
        this List<EntityAttribute>? attributes,
        string id)
    {
        if (attributes == null) return new List<EntityAttribute>();

        return attributes.Where(a => a.Id != id).ToList();
    }

    public static List<EntityAttribute> ClearAttributes(this List<EntityAttribute>? attributes)
        => new List<EntityAttribute>();

    // ── Misc ─────────────────────────────────────────────────────────────────

    public static Dictionary<string, object?> ToDictionary(this List<EntityAttribute>? attributes)
    {
        if (attributes == null) return new Dictionary<string, object?>();

        return attributes
            .Where(a => !string.IsNullOrWhiteSpace(a.Name))
            .ToDictionary(a => a.Name!, a => a.Value);
    }

    /// <summary>
    /// Infer attribute type from value based on MoySklad API specification.
    /// </summary>
    private static string InferType(object? value) => value switch
    {
        null => EntityAttributeType.String,
        string => EntityAttributeType.String,
        bool => EntityAttributeType.Boolean,
        long => EntityAttributeType.Long,
        int => EntityAttributeType.Long,
        decimal => EntityAttributeType.Double,
        double => EntityAttributeType.Double,
        float => EntityAttributeType.Double,
        DateTime => EntityAttributeType.Time,
        Dictionary<string, object> => EntityAttributeType.CustomEntity,
        _ => EntityAttributeType.String
    };
}

public class AttributeBuilder
{
    private readonly List<EntityAttribute> _attributes = new();
    private readonly string? _entityTypePath;

    public AttributeBuilder(string? entityTypePath = null)
    {
        _entityTypePath = entityTypePath;
    }

    /// <summary>Create a new builder instance.</summary>
    public static AttributeBuilder Create(string? entityTypePath = null) => new(entityTypePath);

    public AttributeBuilder AddString(string attributeId, object? value, string? name = null)
        => Add(attributeId, value, EntityAttributeType.String, name);

    public AttributeBuilder AddText(string attributeId, object? value, string? name = null)
        => Add(attributeId, value, EntityAttributeType.Text, name);

    public AttributeBuilder AddLong(string attributeId, long? value, string? name = null)
        => Add(attributeId, value, EntityAttributeType.Long, name);

    public AttributeBuilder AddDouble(string attributeId, decimal? value, string? name = null)
        => Add(attributeId, value, EntityAttributeType.Double, name);

    public AttributeBuilder AddBoolean(string attributeId, bool? value, string? name = null)
        => Add(attributeId, value, EntityAttributeType.Boolean, name);

    public AttributeBuilder AddDateTime(string attributeId, DateTime? value, string? name = null)
        => Add(attributeId, value?.ToString("yyyy-MM-dd HH:mm:ss"), EntityAttributeType.Time, name);

    public AttributeBuilder AddLink(string attributeId, string? url, string? name = null)
        => Add(attributeId, url, EntityAttributeType.Link, name);

    public AttributeBuilder AddFile(string attributeId, string? fileReference, string? name = null)
        => Add(attributeId, fileReference, EntityAttributeType.File, name);

    public AttributeBuilder AddCustomEntity(string attributeId, Dictionary<string, object>? entityRef,
        string? name = null)
        => Add(attributeId, entityRef, EntityAttributeType.CustomEntity, name);

    /// <summary>
    /// Add attribute by id with explicit type. Meta href is built automatically when the
    /// builder was created with an <c>entityTypePath</c>.
    /// </summary>
    public AttributeBuilder Add(string attributeId, object? value, string type, string? name = null)
    {
        Meta? meta = _entityTypePath != null
            ? EntityAttributeExtensions.BuildAttributeMeta(_entityTypePath, attributeId)
            : null;

        _attributes.Add(new EntityAttribute
        {
            Id = attributeId,
            Name = name,
            Value = value,
            Type = type,
            Meta = meta,
        });
        return this;
    }

    /// <summary>Build the attributes list.</summary>
    public List<EntityAttribute> Build() => _attributes;
}