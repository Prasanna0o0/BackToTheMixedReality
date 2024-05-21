using UnityEngine;
using UnityEditor;
using System;

public class PrefabShaderUpdaterEditor : EditorWindow
{
    private GameObject prefab;
   

    string URP_Lit = "Universal Render Pipeline/Lit";
    string URP_ComplexLit = "Universal Render Pipeline/Complex Lit";
    string URP_SimpleLit = "Universal Render Pipeline/Simple Lit";
    string URP_Unlit = "Universal Render Pipeline/Unlit";


    string MetaDepthAPI_Lit = "Meta/Depth/URP/Occlusion Lit";
    string MetaDepthAPI_SimpleLit = "Meta/Depth/URP/Occlusion Simple Lit";
    string MetaDepthAPI_Unlit = "Meta/Depth/URP/Occlusion Unlit";


    [Serializable]
    public enum ShaderOptions
    {
        DefaultURPLit, // Corresponds to "Universal Render Pipeline/Lit"
        DefaultURPUnlit, // Corresponds to "Meta/Depth/URP/Occlusion Lit"
        DefaultURPSimplelit // Corresponds to "Meta/Depth/URP/Occlusion Lit"

    }

    [SerializeField]
    public ShaderOptions shaderOption;

    [MenuItem("Tools/Prefab Shader Updater")]
    public static void ShowWindow()
    {
        GetWindow<PrefabShaderUpdaterEditor>("Prefab Shader Updater");
    }

    private void OnGUI()
    {
        GUILayout.Label("Update Shader on Prefab", EditorStyles.boldLabel);

        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);
        
        if (GUILayout.Button("Enable Depth API Shaders"))
        {
            if (prefab != null)
            {
                UpdatePrefabShaders(prefab);
            }
            else
            {
                Debug.LogError("No prefab selected.");
            }
        }

        if (GUILayout.Button("Disable Depth API Shaders"))
        {
            if (prefab != null)
            {
                MakePrefabShadersDefault(prefab);
            }
            else
            {
                Debug.LogError("No prefab selected.");
            }
        }
    }

    private void UpdatePrefabShaders(GameObject prefab)
    {
        Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>(true);

        foreach (Renderer renderer in renderers)
        {
            foreach (Material material in renderer.sharedMaterials)
            {
                if (material.shader.name == URP_Lit)
                {
                    material.shader = Shader.Find(MetaDepthAPI_Lit);

                }
                else if (material.shader.name == URP_ComplexLit)
                {
                    material.shader = Shader.Find(MetaDepthAPI_Lit);

                }
                else if (material.shader.name == URP_Unlit)
                {
                    material.shader = Shader.Find(MetaDepthAPI_Unlit);

                }
                else if (material.shader.name == URP_SimpleLit)
                {
                    material.shader = Shader.Find(MetaDepthAPI_SimpleLit);

                }
            }
        }

        // Save changes to the prefab
        PrefabUtility.SavePrefabAsset(prefab);
        Debug.Log("Prefab shaders updated.");
    }

    private void MakePrefabShadersDefault(GameObject prefab)
    {
        Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>(true);

        foreach (Renderer renderer in renderers)
        {
            foreach (Material material in renderer.sharedMaterials)
            {
                if (material.shader.name == MetaDepthAPI_Lit)
                {
                    material.shader = Shader.Find(URP_Lit);

                }
                else if (material.shader.name == MetaDepthAPI_Unlit)
                {
                    material.shader = Shader.Find(URP_Unlit);

                }
                else if (material.shader.name == MetaDepthAPI_SimpleLit)
                {
                    material.shader = Shader.Find(URP_SimpleLit);

                }
              
            }
        }

        // Save changes to the prefab
        PrefabUtility.SavePrefabAsset(prefab);
        Debug.Log("Prefab shaders updated.");
    }
}
