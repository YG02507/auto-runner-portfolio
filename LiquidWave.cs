using UnityEngine;

public sealed partial class LiquidWave : MonoBehaviour
{
    // Const Private Structures

    private const float RippleHeightLimit = 5.0f;
    private const float RipplePlacidness = 360.0f;

    private const int RipplePeriodLimit = 720;

    // Private Components

    private RectTransform _liquidWave;  // Parent

    private MeshFilter _liquidWaveMeshFilter;

    // Private Class

    [SerializeField] private Mesh _liquidWaveMesh;

    // Private Structures

    [SerializeField] private int _waveColumnCount;
    [SerializeField] private int _ripplePeriod;

    // Private Structure Arrays

    [SerializeField] private Vector3[] _waveMeshVertices;

    [SerializeField] private Color32[] _waveMeshTriangleColors;

    [SerializeField] private float[] _waveColumnLocalPositions;

    [SerializeField] private int[] _waveMeshTriangles;
}
public sealed partial class LiquidWave : MonoBehaviour
{
    // Private Messages of MonoBehaviour Class

    private void Awake()
    {
        Get();

        Initialize();
    }
    private void FixedUpdate()
    {
        Present();
    }
}
public sealed partial class LiquidWave : MonoBehaviour
{
    // Private Defined Methods Called at Awake Message

    private void Get()
    {
        _liquidWave = this.GetComponent<RectTransform>();

        _liquidWaveMeshFilter = _liquidWave.GetComponent<MeshFilter>();
    }
    private void Initialize()
    {
        #region Local Variable Declaration in Initialize Method

        float waveColumnWidth;

        #endregion

        _liquidWaveMesh = new();

        _waveColumnCount = _liquidWave.rect.width < 200.0f ? 9 : 17;

        _waveMeshVertices = new Vector3[2 * _waveColumnCount];

        _waveMeshTriangleColors = new Color32[_waveMeshVertices.Length];

        _waveColumnLocalPositions = new float[_waveColumnCount];

        _waveMeshTriangles = new int[6 * (_waveColumnCount - 1)];

        waveColumnWidth = _liquidWave.rect.width / (_waveColumnCount - 1);

        for (int i = 0; i < _waveColumnCount; i++)
        {
            _waveColumnLocalPositions[i] = (float)i * waveColumnWidth - 0.5f * _liquidWave.rect.width;
        }

        Connect();

        _ripplePeriod = 0;
    }

    // Private Defined Method Called at FixedUpdate Message

    private void Present()
    {
        #region Local Variable Declaration in Present Method

        int waveMeshVertexIndex;

        #endregion

        waveMeshVertexIndex = 0;

        for (int i = 0; i < _waveColumnCount; i++)
        {
            _waveMeshVertices[waveMeshVertexIndex + 1] = _waveColumnLocalPositions[i] * Vector3.right + _liquidWave.rect.height * Vector3.up;
            _waveMeshVertices[waveMeshVertexIndex + 1] += CalculatedRippleHeight(_waveColumnLocalPositions[i]) * Vector3.up;

            waveMeshVertexIndex += 2;
        }

        _liquidWaveMesh.vertices = _waveMeshVertices;
        _liquidWaveMesh.colors32 = _waveMeshTriangleColors;

        _liquidWaveMeshFilter.mesh = _liquidWaveMesh;
    }

    // Private Defined Methods Called at another Defined Methods

    private void Connect()
    {
        #region Local Variables Declaration in Connect Method

        int waveMeshVertexIndex;
        int waveMeshTriangleIndex;

        #endregion

        waveMeshVertexIndex = 0;
        waveMeshTriangleIndex = 0;

        for (int i = 0; i < _waveColumnCount; i++)
        {
            _waveMeshVertices[waveMeshVertexIndex] = _waveColumnLocalPositions[i] * Vector3.right;
            _waveMeshVertices[waveMeshVertexIndex + 1] = _waveColumnLocalPositions[i] * Vector3.right + _liquidWave.rect.height * Vector3.up;

            waveMeshVertexIndex += 2;
        }

        waveMeshVertexIndex = 0;

        for (int i = 0; i < _waveColumnCount - 1; i++)
        {
            _waveMeshTriangles[waveMeshTriangleIndex] = waveMeshVertexIndex;
            _waveMeshTriangles[waveMeshTriangleIndex + 1] = waveMeshVertexIndex + 1;
            _waveMeshTriangles[waveMeshTriangleIndex + 2] = waveMeshVertexIndex + 3;
            _waveMeshTriangles[waveMeshTriangleIndex + 3] = waveMeshVertexIndex;
            _waveMeshTriangles[waveMeshTriangleIndex + 4] = waveMeshVertexIndex + 2;
            _waveMeshTriangles[waveMeshTriangleIndex + 5] = waveMeshVertexIndex + 3;

            waveMeshVertexIndex += 2;
            waveMeshTriangleIndex += 6;
        }

        for (int i = 0; i < _waveMeshTriangleColors.Length; i++)
        {
            _waveMeshTriangleColors[i] = GameManager.One.HueWithAlpha(Color.black, 127);
        }

        _liquidWaveMesh.vertices = _waveMeshVertices;
        _liquidWaveMesh.triangles = _waveMeshTriangles;
        _liquidWaveMesh.colors32 = _waveMeshTriangleColors;
    }
    private float CalculatedRippleHeight(float waveColumnsLocalPosition)
    {
        _ripplePeriod = _ripplePeriod.Equals(RipplePeriodLimit) ? 0 : _ripplePeriod;

        return RippleHeightLimit * Mathf.Sin(Mathf.PI * (waveColumnsLocalPosition + (float)_ripplePeriod++) / RipplePlacidness);
    }
}
