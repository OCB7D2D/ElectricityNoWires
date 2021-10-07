using DMT;
using HarmonyLib;
using UnityEngine;
using System.Reflection;

public class OcbNoWires
{
    public class OcbNoWires_Init : IHarmony
    {
        public void Start()
        {
            Debug.Log("Loading Patch: " + GetType().ToString());
            var harmony = new Harmony(GetType().ToString());
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    public static Color TripWireRelayColor = Color.magenta;
    public static Color ElectricWireRelayColor = (Color)
        new Color32((byte)0, (byte)97, byte.MaxValue, byte.MaxValue);

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
                                      ___pulseColor == TripWireRelayColor ||
                                      ___pulseColor == ElectricWireRelayColor;

            return false;
        }
    }

}
