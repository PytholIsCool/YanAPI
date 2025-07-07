using Object = UnityEngine.Object;

namespace YanAPI.Wrappers; 
public static class PlayerWrapper {

    #region Properties

    private static float Sanity_Internal { get; set; }

    public static float Sanity {
        get => Sanity_Internal;
        set {
            var lpm = GetLocalPlayerManager();
            Sanity_Internal = value;
            if (lpm != null)
                lpm.Sanity = Sanity_Internal;
        }
    }

    #endregion

    public static YandereScript GetLocalPlayerManager() => GameWrapper.IsInGame() ? Object.FindFirstObjectByType<YandereScript>() : null;
}
