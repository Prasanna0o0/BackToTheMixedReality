using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionShaderUpdater : MonoBehaviour
{
    public string currentShaderName_Lit = "Universal Render Pipeline/Lit";
    public string currentShaderName_SimpleLit = "Universal Render Pipeline/Simple Lit";
    public string currentShaderName_Unlit = "Universal Render Pipeline/Unlit";


    public string targetShaderName_Lit = "Meta/Depth/URP/Occlusion Lit";
    public string targetShaderName_SimpleLit = "Meta/Depth/URP/Occlusion Simple Lit";
    public string targetShaderName_Unlit = "Meta/Depth/URP/Occlusion Unlit";

    private Shader newShader;

    [SerializeField] List<GameObject> occludedgameObjects = new List<GameObject>();
    void Start()
    {
        UpdateShaders();
    }



    private void UpdateShaders()
    {
        foreach (GameObject obj in occludedgameObjects)
        {
            obj.SetActive(false);
            // Get all renderers in this object and its children
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                // Iterate through each material in the renderer
                Material[] materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    // Check if the current material's shader is the one we want to change
                    if (materials[i].shader.name == currentShaderName_Lit)
                    {
                        // Replace the material's shader with the new shader
                        Shader newShader = Shader.Find(targetShaderName_Lit);
                        if (newShader != null)
                        {
                            materials[i].shader = newShader;
                           
                        }
                        else
                        {
                            Debug.LogError("Target shader not found: " + targetShaderName_Lit);
                        }
                    }
                }
            }
            obj.SetActive(true);

        }


    }
}
