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
            new Range(2, 3),
            new Range(2, 4),
            new Range(7, 10),
            new Range(15, 20)
        };
        Console.WriteLine("===使用中レンジ===");
        usedRanges.ForEach(item => Console.WriteLine($"Min:{item.Min} Max:{item.Max}"));
        
        // 使用中レンジマージ
        List<Range> mergedRanges = MergeRanges(usedRanges);
        Console.WriteLine("===使用中レンジ(マージ後)===");
        mergedRanges.ForEach(item => Console.WriteLine($"Min:{item.Min} Max:{item.Max}"));

        // 使用可能レンジ
        List<Range> brankRanges = GetBrankRanges(mergedRanges, totalRange);
        Console.WriteLine("===使用可能レンジ===");
        brankRanges.ForEach(item => Console.WriteLine($"Min:{item.Min} Max:{item.Max}"));
    }

    // 使用可能レンジの取得
    private static List<Range> GetBrankRanges(List<Range> mergedRanges, Range totalRange)
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

            // 使用中レンジの最大値が全体レンジ以上の場合、ループ終了
            if (range.Max >= totalRange.Max)
            {
                break;
            }
        }

        if (current <= totalRange.Max)
        {
            brankRanges.Add(new Range(current, totalRange.Max));
        }
        return brankRanges;
    }

    // 使用中レンジのマージ
    private static List<Range> MergeRanges(List<Range> ranges)
    {
        List<Range> mergedRanges = new List<Range>();

        // 最小値でソート
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