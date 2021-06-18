using System.Collections.Generic;

namespace ScuffedWalls_GUI.Functions
{
    public class ModelToWall : SFunction
    {
        public string Path { get; set; }
        public string FullPath { get; set; }
        public bool? HasAnimation { get; set; }
        public float Duration { get; set; }
        public float DefiniteDurationInBeats { get; set; }
        public float DefiniteDurationInSeconds { get; set; }
        public float SpreadSpawnTime { get; set; }
        public bool? Normal { get; set; }
        public bool? CreateTracks { get; set; }
        public float ColorMult { get; set; }
        public bool? PreserveTime { get; set; }
        public bool? CameraToPlayer { get; set; }
        public bool? CreateNotes { get; set; }
        public bool? Spline { get; set; }
        public int Type { get; set; }
        public float Alpha { get; set; }
        public float Thicc { get; set; }
        public float[] DeltaPosition { get; set; } = new float[3];
        public float[] DeltaRotation { get; set; } = new float[3];
        public float DeltaScale { get; set; }
        public bool? SetDeltaPosition { get; set; }
        public bool? SetDeltaScale { get; set; }
        public int Repeat { get; set; }
        public float RepeatAddTime { get; set; }

        public ModelToWall(float Beat) => BeatNumber = Beat;

        public void ResolvePath(string Model)
        {
            if (Model.Contains("\\")) // Full Path
            {
                FullPath = Model;
                Path = null;
            }
            else
            {
                Path = Model;
                FullPath = null;
            }
        }

        public override List<string> ToScuffedFormat()
        {
            List<string> Formatted = new List<string> { BeatNumber.ToString() + ": ModelToWall" };
            if (Path != null) { Formatted.Add(TabSize + "path:" + Path); }
            if (FullPath != null) { Formatted.Add(TabSize + "fullpath:" + FullPath); }
            if (HasAnimation.HasValue) { Formatted.Add(TabSize + "hasAnimation:" + HasAnimation); }
            if (Duration != default) { Formatted.Add(TabSize + "duration:" + Duration); }
            if (DefiniteDurationInBeats != default) { Formatted.Add(TabSize + "definitedurationbeats:" + DefiniteDurationInBeats); }
            if (DefiniteDurationInSeconds != default) { Formatted.Add(TabSize + "definitedurationseconds:" + DefiniteDurationInSeconds); }
            if (SpreadSpawnTime != default) { Formatted.Add(TabSize + "spreadspawntime:" + SpreadSpawnTime); }
            if (Normal.HasValue) { Formatted.Add(TabSize + "normal:" + Normal); }
            if (CreateTracks.HasValue) { Formatted.Add(TabSize + "createtracks:" + CreateTracks); }
            if (ColorMult != default) { Formatted.Add(TabSize + "colormult:" + ColorMult); }
            if (PreserveTime.HasValue) { Formatted.Add(TabSize + "preservetime:" + PreserveTime); }
            if (CameraToPlayer.HasValue) { Formatted.Add(TabSize + "cameratoplayer:" + CameraToPlayer); }
            if (CreateNotes.HasValue) { Formatted.Add(TabSize + "createnotes:" + CreateNotes); }
            if (Spline.HasValue) { Formatted.Add(TabSize + "spline:" + Spline); }
            if (Type != default) { Formatted.Add(TabSize + "type:" + Type); }
            if (Alpha != default) { Formatted.Add(TabSize + "alpha:" + Alpha); }
            if (Thicc != default) { Formatted.Add(TabSize + "thicc:" + Thicc); }
            if (DeltaPosition[0] != default || DeltaPosition[1] != default || DeltaPosition[2] != default) { Formatted.Add(TabSize + "deltaposition:[" + DeltaPosition[0] + ", " + DeltaPosition[1] + ", " + DeltaPosition[2] + "]"); }
            if (DeltaRotation[0] != default || DeltaRotation[1] != default || DeltaRotation[2] != default) { Formatted.Add(TabSize + "deltarotaion:[" + DeltaRotation[0] + ", " + DeltaRotation[1] + ", " + DeltaRotation[2] + "]"); }
            if (DeltaScale != default) { Formatted.Add(TabSize + "deltascale:" + DeltaScale); }
            if (SetDeltaPosition.HasValue) { Formatted.Add(TabSize + "setdeltaposition:" + SetDeltaPosition); }
            if (SetDeltaScale.HasValue) { Formatted.Add(TabSize + "setdeltascale:" + SetDeltaScale); }
            if (Repeat > 0) { Formatted.Add(TabSize + "repeat:" + Repeat); }
            if (RepeatAddTime != default) { Formatted.Add(TabSize + "repeataddtime:" + RepeatAddTime); }

            return Formatted;
        }

        public void ReadFunctionBlock(List<string> FunctionBlock)
        {
            for (int i = 0; i < FunctionBlock.Count; i++)
            {
                if (FunctionBlock[i].ToLower().Contains("path:")) { ResolvePath(FunctionBlock[i].Split(':')[1]); } // Get Model Path
                if (FunctionBlock[i].ToLower().Contains("hasanimation:")) { HasAnimation = bool.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("duration:")) { Duration = float.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("definitedurationbeats:")) { DefiniteDurationInBeats = float.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("definitedurationseconds:")) { DefiniteDurationInSeconds = float.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("spreadspawntime:")) { SpreadSpawnTime = float.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("normal:")) { Normal = bool.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("createtracks:")) { CreateTracks = bool.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("colormult:")) { ColorMult = float.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("preservetime:")) { PreserveTime = bool.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("cameratoplayer:")) { CameraToPlayer = bool.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("createnotes:")) { CreateNotes = bool.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("spline:")) { Spline = bool.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("type:")) { Type = int.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("alpha:")) { Alpha = float.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("thicc:")) { Thicc = float.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("deltaposition:")) { DeltaPosition = ParseFloatArray(FunctionBlock[i].Split(':')[1], 3); }
                if (FunctionBlock[i].ToLower().Contains("deltarotaion:")) { DeltaRotation = ParseFloatArray(FunctionBlock[i].Split(':')[1], 3); }
                if (FunctionBlock[i].ToLower().Contains("deltascale:")) { DeltaScale = float.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("setdeltaposition:")) { SetDeltaPosition = bool.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("setdeltascale:")) { SetDeltaScale = bool.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("repeat:")) { Repeat = int.Parse(FunctionBlock[i].Split(':')[1]); }
                if (FunctionBlock[i].ToLower().Contains("repeataddtime:")) { RepeatAddTime = float.Parse(FunctionBlock[i].Split(':')[1]); }
            }
        }
    }
}
