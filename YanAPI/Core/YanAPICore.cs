using UnityEngine;
using YanAPI.Core.Cache;
using YanAPI.Modules.Hooks;
using YanAPI.Modules.UI.Scenes.MainMenu;
using YanAPI.Wrappers;

#nullable enable
namespace YanAPI.Core; 
internal class YanAPICore : MonoBehaviour {
    internal static YanAPICore Instance { get; private set; } = null!;

    private bool Initialized = false;
    private void Awake() {
        if (Initialized == true)
            return;

        Instance = this;

        HookManager.PatchAll();
        InitModules();

        Initialized = true;
    }

    private void InitModules() {
        if (Initialized == true)
            return;

        GameSceneWrapper.Init();
        MMUIBase.Init();
    }
}
