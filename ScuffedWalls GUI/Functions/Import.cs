using System.Collections.Generic;

namespace ScuffedWalls_GUI
{
    public class Import : SFunction
    {
        public string Path { get; set; }
        public string FullPath { get; set; }
        public List<int> Type { get; set; } = new List<int>();
        public float AddTime { get; set; }
        public float ToBeat { get; set; }

        public Import(float Beat) => BeatNumber = Beat;

        public void ResolvePath(string FontFilePath)
        {
            if (FontFilePath.Contains("\\")) // Full Path
            {
                FullPath = FontFilePath;
                Path = null;
            }
            else
            {
                Path = FontFilePath;
                FullPath = null;
            }
        }

        public override List<string> ToScuffedFormat()
        {
            List<string> Formatted = new List<string> { BeatNumber.ToString() + ": Import" };
            if (Path != null) { Formatted.Add(TabSize + "path:" + Path); }
            if (Type.Count > 0) { Formatted.Add(TabSize + "type:" + Type.ToArray()); }
            if (AddTime != default) { Formatted.Add(TabSize + "addtime:" + AddTime); }
            if (ToBeat != default) { Formatted.Add(TabSize + "addtime:" + ToBeat); }

            return Formatted;
        }
    }
}
