using UnityEngine;

public sealed partial class Cuboid : MonoBehaviour
{
    // Const Private String

    private const string CuboidSpriteGroupName = "Cuboid Face";

    // Const Private Structures

    private const float InitialCuboidBodyLength = 2.0f;
    private const float CuboidFacePoint = 1.5f;
    private const float SmileThresholdDistance = 4.0f;
    private const float LengthenSpeed = 8.0f;

    private const int CuboidExpressionlessFaceSpriteIndex = 0;
    private const int CuboidSmiledFaceSpriteIndex = 1;

    // Private Components

    private Transform _cuboid;                      // Parent
    private Transform _cuboidBody;                  // Child(0)
    private Transform _cuboidFace;                  // Child(0).Child(0)
    private Transform _cuboidIndicator;             // Child(1)
    private Transform _cuboidCoarseIndicator;       // Child(1).Child(0)
    private Transform _cuboidFineIndicator;         // Child(1).Child(1)
    private Transform _cuboidFaceIndicator;         // Child(1).Child(2)

    private Instruction _cuboidInstruction;

    private Transition _cuboidTransition;

    private Indication _cuboidIndication;

    private SpriteRenderer _cuboidBodySpriteRenderer;
    private SpriteRenderer _cuboidFaceSpriteRenderer;
    private SpriteRenderer _cuboidIndicatorSpriteRenderer;
    private SpriteRenderer _cuboidCoarseIndicatorSpriteRenderer;
    private SpriteRenderer _cuboidFineIndicatorSpriteRenderer;
    private SpriteRenderer _cuboidFaceIndicatorSpriteRenderer;

    private LineRenderer _cuboidIndicatorLineRenderer;

    private BoxCollider2D _cuboidBodyBoxCollider2D;

    // Private Class Array

    [SerializeField] private Sprite[] _cuboidSprites;

    // Private Structures

    [SerializeField] private float _indicateThresholdDistance;
    [SerializeField] private float _cuboidDegree;
    [SerializeField] private float _cuboidLength;
}
public sealed partial class Cuboid : MonoBehaviour
{
    // Private Messages of MonoBehaviour Class

    private void Awake()
    {
        Get();

        Initialize();
    }
    private void OnEnable()
    {
        Decode();

        Conduct();
    }
    private void FixedUpdate()
    {
        _cuboidTransition.PerformState();
    }
}
public sealed partial class Cuboid : MonoBehaviour
{
    // Private Defined Methods Called at Awake Message

    private void Get()
    {
        _cuboid = this.transform;
        _cuboidBody = _cuboid.GetChild(0);
        _cuboidFace = _cuboidBody.GetChild(0);
        _cuboidIndicator = _cuboid.GetChild(1);
        _cuboidCoarseIndicator = _cuboidIndicator.GetChild(0);
        _cuboidFineIndicator = _cuboidIndicator.GetChild(1);
        _cuboidFaceIndicator = _cuboidIndicator.GetChild(2);

        _cuboidInstruction = _cuboid.GetComponent<Instruction>();

        _cuboidTransition = _cuboid.GetComponent<Transition>();

        _cuboidIndication = _cuboid.GetComponent<Indication>();

        _cuboidBodySpriteRenderer = _cuboidBody.GetComponent<SpriteRenderer>();
        _cuboidFaceSpriteRenderer = _cuboidFace.GetComponent<SpriteRenderer>();
        _cuboidIndicatorSpriteRenderer = _cuboidIndicator.GetComponent<SpriteRenderer>();
        _cuboidCoarseIndicatorSpriteRenderer = _cuboidCoarseIndicator.GetComponent<SpriteRenderer>();
        _cuboidFineIndicatorSpriteRenderer = _cuboidFineIndicator.GetComponent<SpriteRenderer>();
        _cuboidFaceIndicatorSpriteRenderer = _cuboidFaceIndicator.GetComponent<SpriteRenderer>();

        _cuboidIndicatorLineRenderer = _cuboidIndicator.GetComponent<LineRenderer>();

        _cuboidBodyBoxCollider2D = _cuboidBody.GetComponent<BoxCollider2D>();
    }
    private void Initialize()
    {
        _cuboidSprites = GameManager.One.LoadedSpriteGroup(CuboidSpriteGroupName);

        _cuboidBodySpriteRenderer.color = GameManager.One.SkinHue;

        _cuboidCoarseIndicatorSpriteRenderer.color = GameManager.One.LineHue;

        SetIndicatorHue(_cuboidCoarseIndicatorSpriteRenderer.color);
    }

    // Private Defined Methods Called at OnEnable Message

    private void Decode()
    {
        #region Local Variable Declaration in Decode Method

        string[] splitDetails;

        #endregion

        splitDetails = _cuboidInstruction.SplitDetails('t', 'd', 'l', '!');

        _indicateThresholdDistance = _cuboidInstruction.ParsedSignedDigits(splitDetails[0]);
        _cuboidDegree = _cuboidInstruction.ParsedTripleDigits(splitDetails[1]);
        _cuboidLength = _cuboidInstruction.ParsedSignedDigits(splitDetails[2]);
    }
    private void Conduct()
    {
        RotateCuboid();

        FormCuboid();

        SetCuboidFace(CuboidExpressionlessFaceSpriteIndex);

        _cuboidInstruction.VanishDeterminant = _cuboid.position.x + _cuboidLength + _cuboidLength;

        _cuboidTransition.NextStatePerformed = Wait;
    }

    // Private Defined Methods Used by Delegate

    private void Wait()
    {
        if (_cuboidIndication.IsTimeToIndicate(_cuboid.position.x, _indicateThresholdDistance))
        {
            _cuboidTransition.Transit(() => StartIndicating());

            _cuboidTransition.NextStatePerformed = Indicate;
        }
    }
    private void Indicate()
    {
        _cuboidIndication.PulseIndicator(_cuboidCoarseIndicator);

        if (_cuboidIndicator.position.x + SmileThresholdDistance >= _cuboid.position.x)
        {
            _cuboidTransition.Transit(() => { ChangeIndicatorMode(true); SetProgressOffset(); });

            _cuboidTransition.NextStatePerformed = Smile;
        }
    }
    private void Smile()
    {
        SetIndicatorHue(_cuboidIndication.BlinkedIndicatorHue(_cuboidIndicatorSpriteRenderer.color, _cuboidInstruction.ProgressOffset));

        if (_cuboidIndication.IsOverlappingPerfectly(_cuboidIndicator, _cuboid.position.x))
        {
            _cuboidTransition.Transit(() => { ChangeIndicatorMode(false); SetProgressOffset(); });

            _cuboidTransition.NextStatePerformed = Lengthen;
        }
    }
    private void Lengthen()
    {
        #region Local Variable Declaration in Lengthen Method

        float progressDomain;

        #endregion

        if ((progressDomain = LengthenSpeed * (GameManager.One.Progress - _cuboidInstruction.ProgressOffset)) < _cuboidLength)
        {
            FormCuboid(progressDomain);
        }
        else
        {
            _cuboidTransition.Transit(() => { FormCuboid(_cuboidLength); EndIndicating(); });

            _cuboidTransition.NextStatePerformed = Keep;
        }
    }
    private void Keep()
    {
        ProcessVanishing();
    }

    // Private Defined Methods Called at Other Defined Methods

    private void SetIndicatorHue(Color indicatorHue)
    {
        _cuboidFaceIndicatorSpriteRenderer.color = _cuboidFineIndicatorSpriteRenderer.color = _cuboidIndicatorSpriteRenderer.color = indicatorHue;

        _cuboidIndicatorLineRenderer.endColor = _cuboidIndicatorLineRenderer.startColor = indicatorHue;
    }
    private void RotateCuboid()
    {
        _cuboid.rotation = Quaternion.Euler(0.0f, 0.0f, _cuboidDegree);

        _cuboidFace.localRotation = Quaternion.Euler(0.0f, 0.0f, -_cuboidDegree);
    }
    private void FormCuboid(float lengthenValue = 0.0f)
    {
        _cuboidBodySpriteRenderer.size = (InitialCuboidBodyLength + lengthenValue) * Vector2.right + Vector2.up;

        _cuboidFace.localPosition = (CuboidFacePoint + lengthenValue) * Vector2.right;

        _cuboidBodyBoxCollider2D.offset = (CuboidFacePoint + (lengthenValue - 1.0f) / 2.0f) * Vector2.right;

        _cuboidBodyBoxCollider2D.size = (InitialCuboidBodyLength + lengthenValue) * Vector2.right + Vector2.up;
    }
    private void SetCuboidFace(int cuboidFaceSpriteIndex)
    {
        _cuboidFaceIndicatorSpriteRenderer.sprite = _cuboidFaceSpriteRenderer.sprite = _cuboidSprites[cuboidFaceSpriteIndex];
    }
    private void StartIndicating()
    {
        _cuboidIndication.FixPoint = (_indicateThresholdDistance, _cuboid.position.y);

        _cuboidIndication.FixIndicator(_cuboidIndicator, _cuboidCoarseIndicator, _cuboidFineIndicator);

        _cuboidIndicator.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        _cuboidCoarseIndicator.localRotation = Quaternion.Euler(0.0f, 0.0f, _cuboidDegree);
        _cuboidFineIndicator.localRotation = Quaternion.Euler(0.0f, 0.0f, _cuboidDegree);
        _cuboidFaceIndicator.localRotation = Quaternion.Euler(0.0f, 0.0f, _cuboidDegree);

        _cuboidCoarseIndicator.Translate(_cuboidLength * Vector3.right, Space.Self);
        _cuboidFineIndicator.Translate((_cuboidLength + CuboidFacePoint - 0.5f) * Vector3.right, Space.Self);
        _cuboidFaceIndicator.Translate((_cuboidLength + CuboidFacePoint - 0.5f) * Vector3.right, Space.Self);

        _cuboidFaceIndicator.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        _cuboidIndicatorLineRenderer.SetPosition(1, _cuboidCoarseIndicator.localPosition);

        _cuboidCoarseIndicator.Translate(Vector3.right, Space.Self);

        _cuboidIndicatorSpriteRenderer.enabled = true;

        _cuboidIndicatorLineRenderer.enabled = true;

        _cuboidIndication.ActivateIndicator(_cuboidIndicator, _cuboidCoarseIndicator);
    }
    private void ChangeIndicatorMode(bool isIndicatorFixedToMainCamera)
    {
        if (isIndicatorFixedToMainCamera)
        {
            _cuboidIndication.BeReadyToIndicateFinely(_cuboidCoarseIndicator);

            _cuboidFineIndicator.gameObject.SetActive(isIndicatorFixedToMainCamera);

            _cuboidFaceIndicatorSpriteRenderer.sprite = _cuboidFaceSpriteRenderer.sprite = _cuboidSprites[CuboidSmiledFaceSpriteIndex];
        }
        else
        {
            SetIndicatorHue(GameManager.One.HueWithAlpha(_cuboidIndicatorSpriteRenderer.color));

            _cuboidIndicatorSpriteRenderer.enabled = isIndicatorFixedToMainCamera;

            _cuboidIndicatorLineRenderer.enabled = isIndicatorFixedToMainCamera;

            GameManager.One.BecomeChild(_cuboidIndicator, _cuboid);

            _cuboidIndicator.localPosition = Vector3.zero;
        }
    }
    private void SetProgressOffset()
    {
        _cuboidInstruction.SetProgressOffset();
    }
    private void EndIndicating()
    {
        _cuboidIndicator.gameObject.SetActive(false);

        _cuboidIndicator.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        _cuboidCoarseIndicator.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0.0f, 0.0f, 0.0f));
        _cuboidFineIndicator.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0.0f, 0.0f, 0.0f));
        _cuboidFaceIndicator.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0.0f, 0.0f, 0.0f));
    }
    private void ProcessVanishing()
    {
        if (_cuboidInstruction.IsTimeToVanish())
        {
            _cuboidInstruction.Vanish();
        }
    }
}
