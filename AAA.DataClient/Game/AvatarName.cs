using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAA.DataClient.Game
{
    public class AvatarName
    {
        public enum PatternStyle
        {
            Simp = 0,
            Soft = 1,
            Harsh = 2,
            Snake = 3,
        }

        private static readonly Dictionary<string, int[]> FreqsVowels = new()
            {
                { "a", new[] { 3, 3, 3, 2 } },
                { "e", new[] { 4, 6, 1, 6 } },
                { "i", new[] { 2, 1, 2, 1 } },
                { "o", new[] { 3, 3, 1, 1 } },
                { "u", new[] { 1, 1, 6, 2 } }
            };

        private static readonly Dictionary<string, int[]> FreqsCons = new()
            {
                { "b", new[] { 1, 1, 1, 1 } },
                { "c", new[] { 2, 1, 1, 1 } },
                { "d", new[] { 2, 2, 8, 1 } },
                { "f", new[] { 1, 1, 1, 4 } },
                { "g", new[] { 1, 2, 6, 1 } },
                { "h", new[] { 4, 1, 4, 8 } },
                { "j", new[] { 1, 1, 1, 1 } },
                { "k", new[] { 1, 2, 1, 1 } },
                { "l", new[] { 2, 10, 1, 3 } },
                { "m", new[] { 2, 2, 2, 1 } },
                { "n", new[] { 5, 8, 1, 1 } },
                { "p", new[] { 1, 1, 1, 1 } },
                { "q", new[] { 1, 1, 1, 1 } },
                { "r", new[] { 4, 1, 6, 1 } },
                { "s", new[] { 4, 2, 4, 8 } },
                { "t", new[] { 6, 2, 6, 2 } },
                { "v", new[] { 1, 1, 6, 1 } },
                { "x", new[] { 1, 1, 3, 1 } },
                { "z", new[] { 1, 1, 3, 3 } },
                { "w", new[] { 1, 3, 1, 1 } },
                { "y", new[] { 1, 1, 1, 1 } }
            };

        private readonly string[] ClustersStart =
        {
                "bl", "br", "ch", "cl", "cr", "dr", "fl", "fr", "gl", "gr",
                "pl", "pr", "sc", "sh", "sk", "sl", "sm", "sn", "sp", "st", "sw",
                "th", "tr", "tw", "wh", "wr"
            };

        private readonly string[] ClustersEnd =
        {
                "st", "sk", "sp", "nd", "nt", "nk", "mp", "rd", "ld", "lp", "rk", "lt", "lf", "pt", "ft", "ct"
            };

        public string Generate(int length, PatternStyle style, string starts, string ends, string fav)
        {
            List<string> vowels = CreateVowels((int)style);
            List<string> consonants = CreateConsonants((int)style);

            if (fav != "none" && FreqsVowels.ContainsKey(fav))
            {
                for (int i = 0; i < 50; i++)
                {
                    vowels.Add(fav);
                }
            }
            else if (fav != "none" && FreqsCons.ContainsKey(fav))
            {
                for (int i = 0; i < 50; i++)
                {
                    consonants.Add(fav);
                }
            }

            List<int> pattern = new List<int>();
            // Skeleton implementation should be replaced with the appropriate input method in C#
            // pattern = GetSkeletonPattern();

            // Replace with the actual method for generating a pattern in C#
            pattern = MakePattern(length, style);

            List<string> name = new();

            if (ends == "rnd")
            {
                if (starts != "rnd" && FreqsVowels.ContainsKey(starts))
                {
                    while (pattern[0] != 0)
                    {
                        pattern = MakePattern(length, style);
                    }
                    name.Add(starts);
                    pattern.RemoveAt(0);
                }

                if (starts != "rnd" && FreqsCons.ContainsKey(starts))
                {
                    while (pattern[0] != 1)
                    {
                        pattern = MakePattern(length, style);
                    }
                    name.Add(starts);
                    pattern.RemoveAt(0);
                }
            }

            if (starts == "rnd")
            {
                if (ends != "rnd" && FreqsVowels.ContainsKey(ends))
                {
                    while (pattern[0] != 0)
                    {
                        pattern = MakePattern(length, style);
                    }
                    pattern.RemoveAt(pattern.Count - 1);
                }

                if (ends != "rnd" && FreqsCons.ContainsKey(ends))
                {
                    while (pattern[0] != 1)
                    {
                        pattern = MakePattern(length, style);
                    }
                    pattern.RemoveAt(pattern.Count - 1);
                }
            }

            if (starts != "rnd" && ends != "rnd")
            {
                if (FreqsVowels.ContainsKey(starts) && FreqsVowels.ContainsKey(ends))
                {
                    while (pattern[0] != 0 && pattern[length - 1] != 0)
                    {
                        pattern = MakePattern(length, style);
                    }
                    name.Add(starts);
                    pattern.RemoveAt(0);
                    pattern.RemoveAt(pattern.Count - 1);
                }
                else if (FreqsCons.ContainsKey(starts) && FreqsCons.ContainsKey(ends))
                {
                    while (pattern[0] != 1 && pattern[length - 1] != 1)
                    {
                        pattern = MakePattern(length, style);
                    }
                    name.Add(starts);
                    pattern.RemoveAt(0);
                    pattern.RemoveAt(pattern.Count - 1);
                }
                else if (FreqsVowels.ContainsKey(starts) && FreqsCons.ContainsKey(ends))
                {
                    while (pattern[0] != 0 && pattern[length - 1] != 1)
                    {
                        pattern = MakePattern(length, style);
                    }
                    name.Add(starts);
                    pattern.RemoveAt(0);
                    pattern.RemoveAt(pattern.Count - 1);
                }
                else if (FreqsCons.ContainsKey(starts) && FreqsVowels.ContainsKey(ends))
                {
                    while (pattern[0] != 1 && pattern[length - 1] != 0)
                    {
                        pattern = MakePattern(length, style);
                    }
                    name.Add(starts);
                    pattern.RemoveAt(0);
                    pattern.RemoveAt(pattern.Count - 1);
                }
            }

            if (style == PatternStyle.Snake)
            {
                for (int i = 0; i < pattern.Count; i++)
                {
                    if (i > 0 && pattern[i] == 0 && pattern[i - 1] == 0)
                    {
                        name.Add(name[i - 1]);
                    }
                    else if (i > 0 && pattern[i] == 1 && pattern[i - 1] == 1)
                    {
                        name.Add("s");
                    }
                    if (pattern[i] == 0)
                    {
                        name.Add(vowels[new Random().Next(vowels.Count)]);
                    }
                    else
                    {
                        name.Add(consonants[new Random().Next(consonants.Count)]);
                    }
                }
            }
            else if (style == PatternStyle.Harsh)
            {
                for (int i = 0; i < pattern.Count; i++)
                {
                    if (i == 0 && pattern[i] == 1 && pattern[i + 1] == 1)
                    {
                        name.Add(ClustersStart[new Random().Next(ClustersStart.Length)]);
                        i++;
                    }
                    else if (i == pattern.Count - 2 && pattern[i] == 1 && pattern[i + 1] == 1)
                    {
                        name.Add(ClustersEnd[new Random().Next(ClustersEnd.Length)]);
                        i++;
                    }
                    else if (pattern[i] == 1 && name[i - 1] == "t")
                    {
                        name.Add("h");
                    }
                    else if (pattern[i] == 0)
                    {
                        name.Add(vowels[new Random().Next(vowels.Count)]);
                    }
                    else
                    {
                        name.Add(consonants[new Random().Next(consonants.Count)]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < pattern.Count; i++)
                {
                    if (i == 0 && pattern[i] == 1 && pattern[i + 1] == 1)
                    {
                        name.Add(ClustersStart[new Random().Next(ClustersStart.Length)]);
                        i++;
                    }
                    else if (i == pattern.Count - 2 && pattern[i] == 1 && pattern[i + 1] == 1)
                    {
                        name.Add(ClustersEnd[new Random().Next(ClustersEnd.Length)]);
                        i++;
                    }
                    else if (pattern[i] == 0)
                    {
                        name.Add(vowels[new Random().Next(vowels.Count)]);
                    }
                    else
                    {
                        name.Add(consonants[new Random().Next(consonants.Count)]);
                    }
                }
            }

            if (ends != "rnd")
            {
                name.Add(ends);
            }

            name[0] = char.ToUpper(name[0][0]) + name[0].Substring(1);

            return string.Join("", name);
        }

        // Add required properties here, e.g. FreqsVowels, FreqsCons
        public List<string> FreqArray(string letter, int frequency)
        {
            List<string> arr = new();
            for (int i = 0; i < frequency; i++)
            {
                arr.Add(letter);
            }
            return arr;
        }

        private static List<string> CreateVowels(int styleNr)
        {
            return FreqsVowels.SelectMany(f => Enumerable.Repeat(f.Key, f.Value[styleNr])).ToList();
        }

        private static List<string> CreateConsonants(int styleNr)
        {
            return FreqsCons.SelectMany(f => Enumerable.Repeat(f.Key, f.Value[styleNr])).ToList();
        }

        public static List<int> MakePattern(int length, PatternStyle style)
        {
            List<int> pattern = new List<int>(length);
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int newValue;

                if (style == PatternStyle.Simp && i > 0)
                {
                    newValue = pattern[i - 1] == 0 ? 1 : 0;
                }
                else if (style == PatternStyle.Soft && i > 1 && pattern[i - 1] == 0 && pattern[i - 2] == 0)
                {
                    newValue = 1;
                }
                else if (style == PatternStyle.Soft && i > 0 && pattern[i - 1] == 1)
                {
                    newValue = 0;
                }
                else if (style == PatternStyle.Harsh && i > 1 && pattern[i - 1] == 1 && pattern[i - 2] == 1)
                {
                    newValue = 0;
                }
                else if (style == PatternStyle.Harsh && i > 0 && pattern[i - 1] == 0)
                {
                    newValue = 1;
                }
                else
                {
                    newValue = random.Next(2);
                }

                pattern.Add(newValue);
            }

            return pattern;
        }
    }
}
