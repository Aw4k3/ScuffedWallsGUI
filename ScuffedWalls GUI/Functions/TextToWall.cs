using System.Collections.Generic;

namespace ScuffedWalls_GUI.Functions
{
    public class TextToWall : SFunction
    {
        public string Path { get; set; }
        public string FullPath { get; set; }
        public List<string> Lines { get; set; } = new List<string>();
        public float Letting { get; set; }
        public float Leading { get; set; }
        public float Size { get; set; }
        public float Thicc { get; set; }
        public float Duration { get; set; }
        public float DefiniteDurationInBeats { get; set; }
        public float DefiniteDurationInSeconds { get; set; }
        public float DefiniteTime { get; set; }
        public float[] Position { get; set; } = new float[2];

        public TextToWall(float Beat) => BeatNumber = Beat;

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
            List<string> Formatted = new List<string> { BeatNumber.ToString() + ": TextToWall" };
            if (Path != null) { Formatted.Add(TabSize + "path:" + Path); }
            if (FullPath != null) { Formatted.Add(TabSize + "fullpath:" + FullPath); }
            if (Lines.Count > 0)
            {
                for (int i = 0; i < Lines.Count; i++)
                {
                    Formatted.Add(TabSize + "line:" + Lines[i]);
                }
            }
            if (Letting != default) { Formatted.Add(TabSize + "letting:" + Letting); }
            if (Leading != default) { Formatted.Add(TabSize + "leading:" + Leading); }
            if (Size != default) { Formatted.Add(TabSize + "size:" + Size); }
            if (Thicc != default) { Formatted.Add(TabSize + "thicc:" + Thicc); }
            if (Duration != default) { Formatted.Add(TabSize + "duration:" + Duration); }
            if (DefiniteDurationInBeats != default) { Formatted.Add(TabSize + "definitedurationbeats:" + DefiniteDurationInBeats); }
            if (DefiniteDurationInSeconds != default) { Formatted.Add(TabSize + "definitedurationseconds:" + DefiniteDurationInSeconds); }
            if (DefiniteTime != default) { Formatted.Add(TabSize + "definitetime:" + DefiniteTime); }
            if (Position[0] != default || Position[1] != default) { Formatted.Add(TabSize + "position:[" + Position[0] + ", " + Position[1] + "]"); }

            return Formatted;
        }

        public void ReadFunctionBlock(List<string> FunctionBlock)
        {
            for (int i = 0; i < FunctionBlock.Count; i++)
            {
                if (FunctionBlock[i].ToLower().Contains("path:")) { ResolvePath(FunctionBlock[i].Split(':')[1]); } // Get font Path
                if (FunctionBlock[i].ToLower().Contains("line:")) { Lines.Add(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("letting:")) { Letting = float.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("leading:")) { Leading = float.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("size:")) { Size = float.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("thicc:")) { Thicc = float.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("duration:")) { Duration = float.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("definitedurationbeats:")) { DefiniteDurationInBeats = float.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("definitedurationseconds:")) { DefiniteDurationInSeconds = float.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("definitetime:")) { DefiniteTime = float.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("position:")) { Position = ParseFloatArray(FunctionBlock[i].Split(':')[1], 2); }
            }
        }
    }
}
