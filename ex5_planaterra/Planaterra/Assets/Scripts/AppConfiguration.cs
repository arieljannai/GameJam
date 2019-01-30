using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppConfiguration /*: MonoBehaviour*/
{
    //public static AppConfiguration Instance;
    
    //void Awake()
    //{
    //    if (Instance == null) { Instance = this; }
    //    else if (Instance != this) { Destroy(gameObject); }
    //}
}

public static class Animators
{
    public static readonly int OPEN_SCREEN__FADE_IN = 0;
    public static readonly int OPEN_SCREEN__FADE_OUT = 1;

    public static readonly int INSTRUCTIONS__FADE_OFF = 0;
    public static readonly int INSTRUCTIONS__FADE_IN = 1;
    public static readonly int INSTRUCTIONS__FADE_OUT = 2;

    public static readonly int PLAYER_MARKER__FADE_IN = 0;
    public static readonly int PLAYER_MARKER__FADE_OUT = 1;

    public static readonly int SHIFT__FADE_IN = 1;
    public static readonly int SHIFT__TO_HOLD = 2;
    public static readonly int SHIFT__FADE_OUT = 3;

    public static readonly int TURTLE_DISC__FADE_IN = 1;
    public static readonly int TURTLE_DISC__FADE_OUT = 2;
    
    public static readonly int CONSTELLATION__FADE_OFF = 0;
    public static readonly int CONSTELLATION__FADE_IN = 1;
    public static readonly int CONSTELLATION__FAIL = 2;
    public static readonly int CONSTELLATION__FAIL_TO_CONSTANT = 3;
    public static readonly int CONSTELLATION__FADE_OUT = 4;
    public static readonly int CONSTELLATION__SHAPE_WIN = 5;

    public static readonly int LAND__OFF = 0;
    public static readonly int LAND__ON = 1;
}
