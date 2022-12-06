using System.Text.RegularExpressions;

namespace AoC2022
{
    public static class Day6
    {
        public static string Part1(IEnumerable<string> lines)
        {
            var input = lines.First();

            char a = input[0];
            char b = input[1];
            char c = input[2];

            for(var i = 3; i < input.Length; i++)
            {
                var d = input[i];
                var uniqueInLastFour = (new List<char>() {a,b,c,d}).Distinct();
                if(uniqueInLastFour.Count() == 4)
                {
                    return (i+1).ToString();
                }
                a = b;
                b = c;
                c = d;
            }
            return "NOT FOUND";
        }

        public static string Part2(IEnumerable<string> lines)
        {
            var input = lines.First();

            char a = input[0];
            char b = input[1];
            char c = input[2];
            char d = input[3];
            char e = input[4];
            char f = input[5];
            char g = input[6];
            char h = input[7];
            char I = input[8];
            char j = input[9];
            char k = input[10];
            char l = input[11];
            char m = input[12];

            for(var i = 13; i < input.Length; i++)
            {
                var n = input[i];
                var uniqueInLastFour = (new List<char>() {a,b,c,d,e,f,g,h,I,j,k,l,m,n}).Distinct();
                if(uniqueInLastFour.Count() == 14)
                {
                    return (i+1).ToString();
                }
                a = b;
                b = c;
                c = d;
                d = e;
                e = f;
                f = g;
                g = h;
                h = I;
                I = j;
                j = k;
                k = l;
                l = m;
                m = n;
            }
            return "NOT FOUND";
        }
    }
}
