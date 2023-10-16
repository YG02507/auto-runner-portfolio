using UnityEngine;

public sealed partial class Instruction : MonoBehaviour
{
    // Const Private String

    private const string LetterWrappingDetails = "_";

    // Const Private Structures

    private const float VanishGap = 6.0f;

    private const char ZeroDigit = '0';
    private const char NegativeSign = '-';

    // Private Strings

    [SerializeField] private string _category;
    [SerializeField] private string _details;

    // Private Structures

    [SerializeField] private float _vanishDeterminant;
    [SerializeField] private float _progressOffset;
}
public sealed partial class Instruction : MonoBehaviour
{
    // Public Delegates

    public delegate void PerformInstead();

    public PerformInstead Perform;
}
public sealed partial class Instruction : MonoBehaviour
{
    // Public Properties

    public float VanishDeterminant
    {
        set => _vanishDeterminant = value;
    }
    public float ProgressOffset
    {
        get => _progressOffset;
    }
}
public sealed partial class Instruction : MonoBehaviour
{
    // Public Defined Methods

    public void Write(string category, string details)
    {
        _category = category;
        _details = LetterWrappingDetails + details + LetterWrappingDetails;
    }
    public string[] SplitDetails(params char[] seperator)
    {
        #region Local Variable Declaration in SplitDetails Method

        string[] splitDetails;

        #endregion

        splitDetails = _details.Split(seperator);

        for (int i = 0; i < splitDetails.Length - 2; i++)
        {
            splitDetails[i] = splitDetails[i + 1];
        }

        return splitDetails[..^2];
    }
    public float ParsedSignedDigits(string signedDigits)
    {
        return (float)(IsNegativeSign(signedDigits[0]) ? -int.Parse(signedDigits[1..]) : int.Parse(signedDigits));
    }
    public float ParsedTripleDigits(string tripleDigits)
    {
        return (float)(100 * (tripleDigits[0] - ZeroDigit) + 10 * (tripleDigits[1] - ZeroDigit) + (tripleDigits[2] - ZeroDigit));
    }
    public bool IsNegativeSign(char sign)
    {
        return sign.Equals(NegativeSign);
    }
    public void SetProgressOffset()
    {
        _progressOffset = GameManager.One.Progress;
    }
    public bool IsTimeToVanish()
    {
        return GameManager.One.Progress - _vanishDeterminant > VanishGap;
    }
    public void Vanish()
    {
        GameManager.One.PushElement(_category, this.gameObject);

        this.gameObject.SetActive(false);
    }
}
