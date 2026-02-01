using UnityEngine;

public class Surface : MonoBehaviour
{
    public enum SurfaceType
    {
        Default,
        Sand,
        Stone
    }

    public SurfaceType surfaceType = SurfaceType.Default;
}