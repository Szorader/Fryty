using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public ParticleSystem particles;
    public ParticleSystem particles1;

    public void TurnOn()
    {
        particles.Play();
        particles1.Play();
    }

    public void TurnOff()
    {
        particles.Stop();
        particles1.Stop();
    }
}