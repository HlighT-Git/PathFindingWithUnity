using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStatus : MonoBehaviour
{
    public static Color UNMOVABLE = Color.black;
    public static Color NORMAL = Color.white;
    public static Color HOVER = Color.blue;
    public static Color SEEN = new(1, 1, 128 / 255f);
    public static Color VISITED = new(0, 128 / 255f, 0);
    public static Color ENTRY = Color.magenta;
    public static Color START = Color.green;
    public static Color PATH = new(1, 140 / 255f, 0);
    public static Color END = Color.red;
    
    public static bool Match(Color status, params Color[] colors)
    {
        foreach(Color color in colors)
        {
            if (status == color)
            {
                return true;
            }
        }
        return false;
    }
}
