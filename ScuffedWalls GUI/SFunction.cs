using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ScuffedWalls_GUI
{
    public abstract class SFunction
    {
        public float BeatNumber { get; set; }
        public string TabSize { get; } = "   ";
        public abstract List<string> ToScuffedFormat();

        public float[] ParseFloatArray(string Arr, int Size)
        {
            float[] parsed = new float[Size];
            Regex Delimiter = new Regex(@"(\[|\]| )+");
            Arr = Delimiter.Replace(Arr, "");
            string[] split = Arr.Split(',');
            for (int i = 0; i < Size; i++) { parsed[i] = float.Parse(split[i]); }
            return parsed;
        }
    }
}
