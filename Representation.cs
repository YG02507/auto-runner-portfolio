using System.Collections;
using UnityEngine;

public sealed partial class Representation : MonoBehaviour
{
    // Const Private Structures

    private const float RingScaleComplement = 1.0f;
    private const float RingOutSpeed = 0.2f;

    private const int MaximumRingAlphaLimit = 255;

    // Private Structure Tuple

    [SerializeField] private (float Maximum, float Minimum) _ringScale;
}
public sealed partial class Representation : MonoBehaviour
{
    // Public Defined Method

    public void SetRing(Transform ring, float scaleValue = 0.5f)
    {
        _ringScale.Minimum = scaleValue + RingScaleComplement;

        ring.localScale = new(_ringScale.Minimum, _ringScale.Minimum, 1.0f);
    }
}
public sealed partial class Representation : MonoBehaviour
{
    // Public Defined Coroutine

    public IEnumerator RingOut(Transform ring, SpriteRenderer ringSpriteRenderer, Color ringHue)
    {
        #region Local Variable Declaration in RingOut Method

        int ringAlpha;

        #endregion

        ring.gameObject.SetActive(true);

        while (ring.localScale.x < (_ringScale.Maximum = _ringScale.Minimum + RingScaleComplement))
        {
            ringAlpha = Mathf.RoundToInt((float)MaximumRingAlphaLimit * (_ringScale.Maximum - ring.localScale.x) / RingScaleComplement);

            ringSpriteRenderer.color = GameManager.One.HueWithAlpha(ringHue, ringAlpha);

            ring.localScale += RingOutSpeed * GameManager.One.AdvanceDelta * (Vector3.right + Vector3.up);

            yield return null;
        }

        ring.gameObject.SetActive(false);

        yield return null;
    }
}
