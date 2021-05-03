using UnityEngine;

public class SimulationSettings : MonoBehaviour {
    [Header("Objects")]
    public GameObject quad;

    [Header("Stats")]
    public int width;
    public int height;
    [Space]
    public int threadGroupSizeX;
    public int threadGroupSizeY;


    public void CreateRenderTexture(ref RenderTexture texture) {
        texture = new RenderTexture(width, height, 24);
        texture.autoGenerateMips = false;
        texture.filterMode = FilterMode.Point;
        texture.enableRandomWrite = true;
        texture.Create();
    }

    public void Dispatch(int kernelId, ComputeShader computeShader) {
        computeShader.Dispatch(kernelId, width / threadGroupSizeX, height / threadGroupSizeY, 1);
    }
}