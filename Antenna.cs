using UnityEngine;

public sealed partial class Antenna : MonoBehaviour
{
    // Const Private Structures

    private const float KeepDistance = 9.0f;
    private const float MaximumRayPathWidth = 0.3f;
    private const float MinimumRayPathWidth = 0.05f;
    private const float RetreatSpeed = 0.24f;

    // Private Components

    private Transform _antenna;            // Parent
    private Transform _antennaBody;        // Child(0)
    private Transform _antennaEmitter;     // Child(1)
    private Transform _rayPath;            // Child(2)
    private Transform _rayStart;           // Child(3)

    private Instruction _antennaInstruction;

    private Transition _antennaTransition;

    private Amplification _antennaAmplification;

    private LineRenderer _antennaLineRenderer;
    private LineRenderer _rayPathLineRenderer;

    private BoxCollider2D _antennaBoxCollider2D;

    // Private Structure

    [SerializeField] private bool _isAntennaAbovePlayer;
}
public sealed partial class Antenna : MonoBehaviour
{
    // Private Messages of MonoBehaviour Class

    private void Awake()
    {
        Get();

        Initialize();
    }
    private void OnEnable()
    {
        Conduct();
    }
    private void FixedUpdate()
    {
        _antennaTransition.PerformState();
    }
}
public sealed partial class Antenna : MonoBehaviour
{
    // Private Defined Methods Called at Awake Message

    private void Get()
    {
        _antenna = this.transform;
        _antennaBody = _antenna.GetChild(0);
        _antennaEmitter = _antenna.GetChild(1);
        _rayPath = _antenna.GetChild(2);
        _rayStart = _antenna.GetChild(3);

        _antennaInstruction = _antenna.GetComponent<Instruction>();

        _antennaTransition = _antenna.GetComponent<Transition>();

        _antennaAmplification = _antenna.GetComponent<Amplification>();

        _antennaLineRenderer = _antenna.GetComponent<LineRenderer>();
        _rayPathLineRenderer = _rayPath.GetComponent<LineRenderer>();

        _antennaBoxCollider2D = _antenna.GetComponent<BoxCollider2D>();
    }
    private void Initialize()
    {
        _antennaBody.GetComponent<SpriteRenderer>().color = GameManager.One.SkinHue;
        _antennaEmitter.GetComponent<SpriteRenderer>().color = GameManager.One.SkinHue;

        _rayPathLineRenderer.endColor = _rayPathLineRenderer.startColor = GameManager.One.LineHue;
    }

    // Private Defined Method Called at OnEnable Message

    private void Conduct()
    {
        _antennaAmplification.SetBeamPeriod();

        SetRayPathWidth(MaximumRayPathWidth);

        _antennaTransition.NextStatePerformed = Approach;
    }

    // Private Defined Methods Used by Delegate

    private void Approach()
    {
        if (_antennaInstruction.IsTimeToPerform(_antenna.position.x, KeepDistance))
        {
            _antennaTransition.Transit(() => { FixToMainCamera(); SetRay(true, false); SetProgressOffset(); });

            _antennaTransition.NextStatePerformed = Charge;
        }
    }
    private void Charge()
    {
        #region Local Variable Declaration in Charge Method

        float rayPathWidth;

        #endregion

        rayPathWidth = (MaximumRayPathWidth - MinimumRayPathWidth) * (GameManager.One.Progress - _stingrayInstruction.ProgressOffset);
        rayPathWidth = MaximumRayPathWidth - rayPathWidth / (_keepDistance - GameManager.One.FocusOffset);

        SetRayPathWidth(rayPathWidth);

        if (_antennaInstruction.IsProgressExceeding(KeepDistance - GameManager.One.FocusOffset))
        {
            _antennaTransition.Transit(() => { CompareHeights(); ProcessRayDamageBox(); SetRay(false, true); SetProgressOffset(); });

            _antennaTransition.NextStatePerformed = Shoot;
        }
    }
    private void Shoot()
    {
        HandlePassingRay();

        AmplifyBeam();

        if (_antennaInstruction.IsProgressExceeding(KeepDistance - GameManager.One.FocusOffset))
        {
            _antennaTransition.Transit(() => SetRay(false, false));

            _antennaTransition.NextStatePerformed = Retreat;
        }
    }
    private void Retreat()
    {
        _antenna.Translate(RetreatSpeed * Vector3.right);

        ProcessVanishing();
    }

    // Private Defined Methods Called at Other Defined Methods

    private void SetRayPathWidth(float rayPathWidth)
    {
        _rayPathLineRenderer.endWidth = _rayPathLineRenderer.startWidth = rayPathWidth;
    }
    private void FixToMainCamera()
    {
        GameManager.One.FixToMainCamera(_antenna, (KeepDistance, _antenna.position.y));
    }
    private void SetRay(bool isPathEnabled, bool isBeamEnabled)
    {
        _rayPath.gameObject.SetActive(isPathEnabled);

        _rayStart.gameObject.SetActive(isBeamEnabled);

        _antennaLineRenderer.enabled = isBeamEnabled;

        _antennaBoxCollider2D.enabled = isBeamEnabled;
    }
    private void SetProgressOffset()
    {
        _antennaInstruction.SetProgressOffset();
    }
    private void CompareHeights()
    {
        _isAntennaAbovePlayer = _antenna.position.y <= GameManager.One.PlayerHeight;
    }
    private void ProcessRayDamageBox()
    {
        _antennaBoxCollider2D.offset = new(GameManager.One.FocusOffset - KeepDistance, _isAntennaAbovePlayer ? -10.0f : 10.0f);
    }
    private void HandlePassingRay()
    {
        if (_antenna.position.y <= GameManager.One.PlayerHeight)
        {
            if (!_isAntennaAbovePlayer)
            {
                _isAntennaAbovePlayer = true;

                Invoke(nameof(ProcessRayDamageBox), Time.fixedDeltaTime);
            }
        }
        else if (_isAntennaAbovePlayer)
        {
            _isAntennaAbovePlayer = false;

            Invoke(nameof(ProcessRayDamageBox), Time.fixedDeltaTime);
        }
    }
    private void AmplifyBeam()
    {
        _antennaAmplification.VibrateBeamVertexLocalScale();
        _antennaAmplification.VibrateBeamVertexLocalScale(_rayStart);

        _antennaLineRenderer.endWidth = _antennaLineRenderer.startWidth = _antennaAmplification.VibratedBeamWidth();

        _antennaAmplification.CountUpBeamPeriod();
    }
    private void ProcessVanishing()
    {
        _antennaInstruction.VanishDeterminant = _antenna.position.x;

        if (_antennaInstruction.IsTimeToVanish(false))
        {
            _antenna.parent = null;

            _antennaInstruction.Vanish();
        }
    }
}
