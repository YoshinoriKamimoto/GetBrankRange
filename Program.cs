// 使用可能レンジ算出処理
internal class Program
{
    private static void Main(string[] args)
    {
        // 全体レンジ
        Range totalRange = new Range(1, 10);
        Console.WriteLine("===全体レンジ===");
        Console.WriteLine($"Min:{totalRange.Min} Max:{totalRange.Max}");

        // 使用中レンジ
        List<Range> usedRanges = new List<Range>
        {
            new Range(2, 4),
            new Range(2, 3),
            new Range(7, 10),
            new Range(8, 12),
            new Range(20, 30)
        };
        Console.WriteLine("===使用中レンジ(全体)===");
        usedRanges.ForEach(item => Console.WriteLine($"Min:{item.Min} Max:{item.Max}"));
        
        // 全体レンジと範囲が重複する使用中レンジのみ取得
        List<Range> overlapUsedRanges = GetOverlapRanges(totalRange, usedRanges);
        Console.WriteLine("===使用中レンジ(全体レンジ範囲重複分)===");
        overlapUsedRanges.ForEach(item => Console.WriteLine($"Min:{item.Min} Max:{item.Max}"));

        // 使用中レンジマージ
        List<Range> mergedUsedRanges = MergeRanges(overlapUsedRanges);
        Console.WriteLine("===使用中レンジ(全体レンジ範囲重複分:マージ後)===");
        mergedUsedRanges.ForEach(item => Console.WriteLine($"Min:{item.Min} Max:{item.Max}"));

        // 使用可能レンジ
        List<Range> brankRanges = GetBrankRanges(mergedUsedRanges, totalRange);
        Console.WriteLine("===使用可能レンジ===");
        brankRanges.ForEach(item => Console.WriteLine($"Min:{item.Min} Max:{item.Max}"));
    }

    // 使用可能レンジの取得
    private static List<Range> GetBrankRanges(IReadOnlyList<Range> mergedRanges, Range totalRange)
    {
        // 全体レンジと使用中レンジから使用可能レンジを取得
        List<Range> brankRanges = new List<Range>();
        long current = totalRange.Min;
        foreach (Range range in mergedRanges)
        {
            if (current < range.Min)
            {
                brankRanges.Add(new Range(current, range.Min - 1));
            }
            current = range.Max + 1;
        }
        if (current <= totalRange.Max)
        {
            brankRanges.Add(new Range(current, totalRange.Max));
        }
        return brankRanges;
    }

    // 使用中レンジのマージ
    private static List<Range> MergeRanges(IReadOnlyList<Range> ranges)
    {
        List<Range> mergedRanges = new List<Range>();
        ranges = ranges.OrderBy(item => item.Min).ToList();

        // レンジのマージ
        foreach (Range range in ranges)
        {
            // ひとつめの要素を追加
            if (!mergedRanges.Any())
            {
                mergedRanges.Add(new Range(range.Min, range.Max));
                continue;
            }

            // 現在要素の最小値がマージリストの最後の要素の最大値より大きい場合(最小値が重複範囲にない場合)
            if (range.Min > mergedRanges.Last().Max)
            {
                // 新しい要素として追加
                mergedRanges.Add(new Range(range.Min, range.Max));
                continue;
            }

            // 現在要素の最小値がマージリストの最後の要素の最大値以下の場合(最小値が重複範囲にある場合)
            if (range.Min <= mergedRanges.Last().Max)
            {
                // 最後の要素の最大値より現在要素の最大値が大きければ代入
                if (range.Max > mergedRanges.Last().Max)
                {
                    mergedRanges.Last().Max = range.Max;
                }
            }
        }
        return mergedRanges;
    }

    // レンジ重複チェック
    private static bool IsOverlap(Range range1, Range range2)
    {
        // 2つのレンジの最小値・最大値がそれぞれ互いに重複する範囲がないかどうかチェック
        if (range2.Min <= range1.Min && range1.Min <= range2.Max)
        {
            return true;
        }
        if (range2.Min <= range1.Max && range1.Max <= range2.Max)
        {
            return true;
        }
        if (range1.Min <= range2.Min && range2.Min <= range1.Max)
        {
            return true;
        }
        if (range1.Min <= range2.Max && range2.Max <= range1.Max)
        {
            return true;
        }
        return false;
    }

    // 範囲が重複するレンジ取得
    private static List<Range> GetOverlapRanges(Range sourceRange, IReadOnlyList<Range> ranges)
    {
        List<Range> overlapRanges = new List<Range>();
        foreach (Range range in ranges)
        {
            if (IsOverlap(sourceRange, range))
            {
                overlapRanges.Add(range);
            }
        }
        return overlapRanges;
    }
}

// レンジ
public class Range
{
    public long Min { get; set; }
    public long Max { get; set; }

    public Range(long min, long max)
    {
        if (min > max)
        {
            throw new ArgumentException("minにmaxより大きな値を指定することはできません");
        }
        this.Min = min;
        this.Max = max;
    }
}