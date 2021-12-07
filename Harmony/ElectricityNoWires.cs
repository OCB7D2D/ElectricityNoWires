using HarmonyLib;
using UnityEngine;
using System.Reflection;
using System.Collections;

public class ElectricityNoWires : IModApi
{

    public void InitMod(Mod mod)
    {
        Log.Out(" Loading Patch: " + this.GetType().ToString());
        var harmony = new HarmonyLib.Harmony(GetType().ToString());
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    [HarmonyPatch(typeof(FastWireNode))]
    [HarmonyPatch("TogglePulse")]
    public class FastWireNode_TogglePulse
    {
        static bool Prefix(FastWireNode __instance, MeshRenderer ___meshRenderer,
            Color ___pulseColor, Color ___wireColor, ref bool isOn)
        {
            if (___meshRenderer.material == null)
                return false;
            ___meshRenderer.material.SetColor("_WireColor",
                isOn ? ___wireColor : ___wireColor);
            ___meshRenderer.material.SetColor("_PulseColor",
                isOn ? ___pulseColor : ___wireColor);

            // We don't know our left/right objects (could not get them
            // via start and end position for power dictionary). But
            // we can also just check for (hard-coded) pulse color.
            ___meshRenderer.enabled = WireManager.Instance.ShowPulse ||
                                      ___pulseColor != Color.yellow &&
                                      ___pulseColor != Color.gray;

            return false;
        }
    }

}
