using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Trexate/Level", order = 2)]
public class LevelData : ScriptableObject
{
    public string levelName;

    [Header("Visuals")]
    public int startYear;
    public int endYear;
    public string background;
}
