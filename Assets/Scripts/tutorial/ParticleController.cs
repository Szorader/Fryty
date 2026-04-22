using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public ParticleSystem particles;

    public void TurnOn()
    {
        particles.Play();
    }

    public void TurnOff()
    {
        particles.Stop();
    }
}