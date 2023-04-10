using UnityEngine;

public class Helpers : MonoBehaviour
{
    public static bool EvenFrame => Time.frameCount % 2 == 0;
}
