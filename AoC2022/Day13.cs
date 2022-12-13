using System.Text.Json;

namespace AoC2022
{
    public static class Day13
    {
        public static string Part1(IEnumerable<string> lines)
        {
            var decoder = new Decoder(lines.ToList());

            return decoder.IndecesInRightOder.Sum().ToString();
        }

        public static string Part2(IEnumerable<string> lines)
        {
            var decoder = new Decoder(lines.ToList(), true);

            return decoder.SortAndFindDecoderKey().ToString();
        }

        internal class Decoder
        {
            private const string Divider1 = "[[2]]";
            private const string Divider2 = "[[6]]";
            private List<Packet> _packets = new List<Packet>();

            public IList<int> IndecesInRightOder { get; } = new List<int>();

            internal Decoder(IList<string> lines, bool sortAllPackets = false)
            {
                if (sortAllPackets)
                {
                    lines.Add(Divider1);
                    lines.Add(Divider2);

                    foreach (var line in lines)
                    {
                        if (line == string.Empty)
                        {
                            continue;
                        }
                        _packets.Add(new Packet(line));
                    }
                }
                else
                {
                    for (var i = 0; i < lines.Count; i = i + 3)
                    {
                        var leftString = lines[i];
                        var rightString = lines[i + 1];

                        var left = JsonDocument.Parse(leftString).RootElement;
                        var right = JsonDocument.Parse(rightString).RootElement;

                        var rightOrder = Compare(left, right);

                        if (rightOrder ?? true) // What if no decision was made at all? Seems to work ok like this (not sure if such cases though)
                        {
                            IndecesInRightOder.Add(i / 3 + 1);
                        }
                    }
                }
            }

            public int SortAndFindDecoderKey()
            {
                static int CompareV2(JsonElement a, JsonElement b)
                {
                    var result = Compare(a, b);
                    return result switch
                    {
                        null => 0,
                        true => -1,
                        false => 1
                    };
                }

                _packets.Sort((a, b) => CompareV2(a.JsonElement, b.JsonElement));
                var first = _packets.FindIndex(x => x.DisplayName == Divider1) + 1;
                var second = _packets.FindIndex(x => x.DisplayName == Divider2) + 1;

                return first * second;
            }

            private static bool? Compare(JsonElement left, JsonElement right)
            {
                if (left.ValueKind == JsonValueKind.Number && right.ValueKind == JsonValueKind.Number)
                {
                    var leftInt = left.GetInt32();
                    var rightInt = right.GetInt32();

                    if (leftInt == rightInt)
                    {
                        return null;
                    }
                    return leftInt < rightInt;
                }

                left = BoxInArrayIfNeeded(left);
                right = BoxInArrayIfNeeded(right);

                using var leftEnumerator = left.EnumerateArray();
                using var rightEnumerator = right.EnumerateArray();
                var leftLength = left.GetArrayLength();
                var rightLength = right.GetArrayLength();

                // _ = left.EnumerateArray().Zip(right.EnumerateArray(), (l, r) => Compare(l, r));
                for (var i = 0; i < leftLength; ++i)
                {
                    if (i >= rightLength)
                    {
                        return false;
                    }
                    leftEnumerator.MoveNext();
                    rightEnumerator.MoveNext();
                    var result = Compare(leftEnumerator.Current, rightEnumerator.Current);
                    if (result != null)
                    {
                        return result;
                    }
                }

                if (leftLength < rightLength)
                {
                    return true;
                }
                return null;
            }

            private static JsonElement BoxInArrayIfNeeded(JsonElement node)
            {
                if (node.ValueKind != JsonValueKind.Array)
                {
                    var value = node.GetInt32();
                    var rArrayString = $"[{value}]"; // This can't be too efficient, but here we go :D
                    return JsonDocument.Parse(rArrayString).RootElement;
                }
                return node;
            }

            internal class Packet
            {
                internal string DisplayName { get; }
                internal JsonElement JsonElement { get; }
                internal Packet(string displayName)
                {
                    DisplayName = displayName;
                    JsonElement = JsonDocument.Parse(displayName).RootElement;
                }
            }
        }
    }
}

