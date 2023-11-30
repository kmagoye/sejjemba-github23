using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class utility : MonoBehaviour
{
    /// <summary>
    /// selected scale -> which scale is on/relevant right now
    ///                -> 0,1,2,3, match to the order in the "scales" list within the scale manager
    /// strength -> the "strength" of that scale
    ///          -> 0,1,2,3,4, where 0 is blank and 4 is full
    ///          -> all other scales that are not the relevant scale are overriden to be blank/0
    /// enabled scales -> list of ints determining which scales are enables, 0,1,2,3 again match to list in scale manager       
    /// </summary> <summary>
    /// 
    /// </summary>
    
    [System.Serializable] public class scaleState
    {
        public int selectedScale;
        public int strength;
        public List<int> enabledScales = new List<int>();
    }

    [System.Serializable] public class song
    {
        public List<AudioClip> clips = new List<AudioClip>();
    }
}
