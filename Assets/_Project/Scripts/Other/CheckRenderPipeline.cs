using UnityEngine;
using UnityEngine.Rendering;

public class CheckRenderPipeline : MonoBehaviour
{
    void Start()
    {
        var rpAsset = GraphicsSettings.renderPipelineAsset;
        if (rpAsset != null)
        {
            Debug.Log($"Render pipeline: {rpAsset.GetType().ToString()}");
        }
    }
}
