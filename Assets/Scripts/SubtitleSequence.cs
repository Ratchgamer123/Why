using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sequence", menuName = "Whacky Stuff/Subtitles")]
public class SubtitleSequence : ScriptableObject
{
    public string sequenceName;

    public string content;
    public string audioName;
}
