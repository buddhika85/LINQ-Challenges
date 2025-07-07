namespace LINQ_Challeges;

public static class MathExtensions
{
    public static decimal StandardDeviation(this IEnumerable<decimal> values)
    {
        if (values == null) throw new ArgumentNullException(nameof(values));

        var valueList = values.ToList();
        if (valueList.Count == 0) return 0;

        decimal mean = valueList.Average();
        decimal sumOfSquares = valueList.Sum(v => (v - mean) * (v - mean));
        decimal variance = sumOfSquares / valueList.Count;

        return (decimal)Math.Sqrt((double)variance);
    }
}