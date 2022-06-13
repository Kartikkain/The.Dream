
using UnityEngine;

namespace Saving
{
    [System.Serializable]
    public class SerializedVector3
    {
        float x, y, z;
       public SerializedVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        public Vector3 ToVector()
        {
            return new Vector3(x, y, z);
        }
    }
}

