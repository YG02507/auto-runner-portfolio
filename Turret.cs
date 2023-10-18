public sealed partial class Turret : MonoBehaviour
{
    // Const Private Structures

    private const float RecoilScalar = 0.02f;

    private const int RecoilPower = 10;

    // Private Components

    private Transform _turret;              // Parent
    private Transform _turretBarrel;        // Child(0)
    private Transform _turretBarrelSpark;   // Child(0).Child(0)
    private Transform _cannonball;          // Child(1)
    private Transform _cannonballSphere;    // Child(1).Child(0)

    private ParticleSystem _turretBarrelSparkParticleSystem;
}
public sealed partial class Turret : MonoBehaviour
{
    // Private Defined Coroutine

    private IEnumerator Recoil()
    {
        _turretBarrelSparkParticleSystem.Play();

        for (int i = 0; i < RecoilPower; i++)
        {
            _turretBarrel.Translate(RecoilScalar * Vector3.down);

            yield return null;
        }
        for (int i = 0; i < RecoilPower; i++)
        {
            _turretBarrel.Translate(RecoilScalar * Vector3.up);

            yield return null;
        }

        _turretBarrel.localPosition = Vector3.zero;
    }
}
