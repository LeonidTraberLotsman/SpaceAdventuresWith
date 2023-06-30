using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class tool 
{
    public static IEnumerator Move(Transform what, Vector3 from, Vector3 to,int frames)
    {
        Vector3 delta = (to - from) / frames;
        
        
        for (int i = 0; i < frames; i++)
        {
            what.position += delta;
            yield return null;
        }
        yield return null;
        what.position = to;
    }
}
