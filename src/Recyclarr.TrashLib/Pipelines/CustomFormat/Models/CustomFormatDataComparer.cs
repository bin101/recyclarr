using MoreLinq;

namespace Recyclarr.TrashLib;

public sealed class CustomFormatDataEqualityComparer : IEqualityComparer<CustomFormatData>
{
    public bool Equals(CustomFormatData? x, CustomFormatData? y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (ReferenceEquals(x, null) || ReferenceEquals(y, null) || x.GetType() != y.GetType())
        {
            return false;
        }

        return x.Id.Equals(y.Id) &&
            x.Name.Equals(y.Name, StringComparison.Ordinal) &&
            x.IncludeCustomFormatWhenRenaming.Equals(y.IncludeCustomFormatWhenRenaming) &&
            AllSpecificationsEqual(x.Specifications, y.Specifications);
    }

    private static bool AllSpecificationsEqual(
        IReadOnlyCollection<CustomFormatSpecificationData> first,
        IReadOnlyCollection<CustomFormatSpecificationData> second)
    {
        if (first.Count != second.Count)
        {
            return false;
        }

        return first
            .FullJoin(second, x => x.Name, _ => false, _ => false, SpecificationEqual)
            .All(x => x);
    }

    private static bool SpecificationEqual(CustomFormatSpecificationData a, CustomFormatSpecificationData b)
    {
        return a.Name.Equals(b.Name, StringComparison.Ordinal) &&
            Equals(a.Fields.Value, b.Fields.Value) &&
            a.Implementation.Equals(b.Implementation, StringComparison.Ordinal) &&
            a.Negate.Equals(b.Negate) &&
            a.Required.Equals(b.Required);
    }

    public int GetHashCode(CustomFormatData obj)
    {
        unchecked
        {
            var hashCode = obj.TrashId.GetHashCode();
            hashCode = (hashCode * 397) ^ obj.Id;
            return hashCode;
        }
    }
}
