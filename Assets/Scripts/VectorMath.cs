using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorMath : MonoBehaviour
{
    /// <summary>
    /// Projects a vector onto the horizontal plane.
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static Vector3 NormalizeHorizontalProjection(Vector3 vector)
    {
        vector.y = 0;
        return vector.normalized;
    }
}
