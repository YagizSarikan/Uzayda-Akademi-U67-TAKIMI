using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        var bootstrapPrefab = Resources.Load("--- Persistent Components ---");
        if (bootstrapPrefab == null) return;
        var bootstrapper = Instantiate(bootstrapPrefab);
        bootstrapper.name = "--- Persistent Components ---";
        DontDestroyOnLoad(bootstrapper);
    }
}
