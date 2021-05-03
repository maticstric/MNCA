using UnityEngine;

public class LargerThanLife : MonoBehaviour {
    [Header("Objects")]
    [SerializeField] private ComputeShader computeShader;

    [Header("Params")]
    [SerializeField] [Range(0.000011f, 1f)] private float timeBetweenSteps;
    [SerializeField] [Range(0f, 1f)] private float density;
    [Space]
    [SerializeField] private int range;
    [SerializeField] private int b1;
    [SerializeField] private int b2;
    [SerializeField] private int a1;
    [SerializeField] private int a2;

    private SimulationSettings _settings;

    private RenderTexture _currTexture;
    private RenderTexture _prevTexture;

    private int _initId;
    private int _mainId;

    /* * * * */

    private void Awake() {
        _settings = gameObject.GetComponentInParent<SimulationSettings>();

        _initId = computeShader.FindKernel("Init");
        _mainId = computeShader.FindKernel("Main");

        _settings.CreateRenderTexture(ref _currTexture);
        _settings.CreateRenderTexture(ref _prevTexture);

        _settings.quad.GetComponent<MeshRenderer>().material.mainTexture = _currTexture;
    }

    private void Start() {
        computeShader.SetFloat("Density", density);

        computeShader.SetInt("Range", range);
        computeShader.SetInt("B1", b1);
        computeShader.SetInt("B2", b2);
        computeShader.SetInt("A1", a1);
        computeShader.SetInt("A2", a2);

        computeShader.SetTexture(_initId, "CurrTexture", _currTexture);
        _settings.Dispatch(_initId, computeShader);

        InvokeRepeating("SimulationStep", 0f, timeBetweenSteps);
    }

    /* * * * */

    private void SimulationStep() {
        RenderTexture tmp = _prevTexture;
        _prevTexture = _currTexture;
        _currTexture = tmp;

        computeShader.SetTexture(_mainId, "PrevTexture", _prevTexture);
        computeShader.SetTexture(_mainId, "CurrTexture", _currTexture);

        _settings.Dispatch(_mainId, computeShader);
    }
}