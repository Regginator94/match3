using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchInfo {

    public List<GridItems> match;
    public int matchStartingX;
    public int matchEndX;
    public int matchStartingY;
    public int matchEndY;

    public bool validMatch
    {
        get
        {
            return match != null;
        }
    }
}
