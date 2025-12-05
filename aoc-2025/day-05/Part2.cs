using AocUtils;

internal sealed class Part2
{
	private sealed class Range : IEquatable<Range>
	{
		public Range(long low, long high)
		{
			Low = low;
			High = high;
		}

		public readonly long Low;
		public readonly long High;

		public bool Equals(Range? other)
		{
			if (other is null)
				return false;
			if (ReferenceEquals(this, other))
				return true;
			return Low == other.Low && High == other.High;
		}

		public override bool Equals(object? obj)
		{
			return ReferenceEquals(this, obj) || obj is Range other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Low, High);
		}
	}

	public void Run()
	{
		IntervalTree<long, Range> ranges = [];

		var sr = new StreamReader("input.txt");
		var line = sr.ReadLine();
		var i = 0;
		while (!string.IsNullOrEmpty(line))
		{
			var parts = line.Split('-');
			if (parts.Length != 2)
				break;

			var r = new Range(long.Parse(parts[0]), long.Parse(parts[1]));
			var overlappingLow = ranges.FindAllOverlapping(r.Low);
			var overlappingHigh = ranges.FindAllOverlapping(r.High);
			
			// Both ends of our range overlap an existing range.
			if (overlappingLow.Count == 1 && overlappingHigh.Count == 1)
			{
				// We are completely within another range. Just ignore.
				var low = overlappingLow[0];
				var high = overlappingHigh[0];
				if (low.Equals(high))
				{
					line = sr.ReadLine();
					continue;
				}

				// Check for any ranges completely within this range and delete them.
				DeleteAllSubRanges(ranges, r);

				// Now combine the low range and the high range into one longer range that covers both.
				ranges.Delete(high, high.Low, high.High);
				ranges.Delete(low, low.Low, low.High);
				var newRange = new Range(low.Low, high.High);
				ranges.Insert(newRange, newRange.Low, newRange.High);
			}

			// We only overlap a range on the low end.
			else if (overlappingLow.Count == 1)
			{
				// Extend the low end forward to our range end.
				var low = overlappingLow[0];
				ranges.Delete(low, low.Low, low.High);
				var newRange = new Range(low.Low, r.High);
				ranges.Insert(newRange, newRange.Low, newRange.High);
			}

			// We only overlap a range on the high end.
			else if (overlappingHigh.Count == 1)
			{
				// Extend the high end backward to our range start.
				var high = overlappingHigh[0];
				ranges.Delete(high, high.Low, high.High);
				var newRange = new Range(r.Low, high.High);
				ranges.Insert(newRange, newRange.Low, newRange.High);
			}

			// We don't overlap any range on our ends
			else
			{
				// There may still be ranges fully within our range. Delete them.
				DeleteAllSubRanges(ranges, r);

				ranges.Insert(r, r.Low, r.High);
			}

			line = sr.ReadLine();
		}

		var numValues = 0L;
		foreach (var range in ranges)
		{
			numValues += (range.High - range.Low) + 1;
		}

		Console.WriteLine(numValues); 
	}

	private void DeleteAllSubRanges(IntervalTree<long, Range> ranges, Range r)
	{
		var rangesToDelete = new List<Range>();
		var e = ranges.FindLeastFollowing(r.Low, true);
		if (e == null)
			return;
		e.MoveNext();
		while (e.IsCurrentValid())
		{
			if (e.Current.Low >= r.Low && e.Current.High <= r.High)
				rangesToDelete.Add(e.Current);
			if (e.Current.Low > r.High)
				break;
			e.MoveNext();
		}
		foreach (var rangeToDelete in rangesToDelete)
			ranges.Delete(rangeToDelete, rangeToDelete.Low, rangeToDelete.High);
	}
}
