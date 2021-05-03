using UnityEngine;

public class Simulation : MonoBehaviour {
    [Header("Objects")]
    [SerializeField] private ComputeShader computeShader;
    [SerializeField] private GameObject quad;

    [Header("Stats")]
    [SerializeField] private int width;
    [SerializeField] private int height;

    private RenderTexture _currTexture;
    private RenderTexture _prevTexture;

    private int _initId;
    private int _mainId;

    /* * * * */

    private void Awake() {
        _initId = computeShader.FindKernel("Init");
        _mainId = computeShader.FindKernel("Main");

        CreateRenderTexture(ref _currTexture);
        CreateRenderTexture(ref _prevTexture);

        quad.GetComponent<MeshRenderer>().material.mainTexture = _currTexture;
    }

    private void Start() {
        computeShader.SetTexture(_initId, "CurrTexture", _currTexture);
        computeShader.Dispatch(_initId, width / 8, height / 8, 1);

        InvokeRepeating("SimulationStep", 0f, 0.000011f);
    }

    /* * * * */

    private void SimulationStep() {
        RenderTexture tmp = _prevTexture;
        _prevTexture = _currTexture;
        _currTexture = tmp;

        computeShader.SetTexture(_mainId, "PrevTexture", _prevTexture);
        computeShader.SetTexture(_mainId, "CurrTexture", _currTexture);

        computeShader.Dispatch(_mainId, width / 8, height / 8, 1);
    }

    private void CreateRenderTexture(ref RenderTexture texture) {
        texture = new RenderTexture(width, height, 24);
        texture.autoGenerateMips = false;
        texture.filterMode = FilterMode.Point;
        texture.enableRandomWrite = true;
        texture.Create();
    }
}