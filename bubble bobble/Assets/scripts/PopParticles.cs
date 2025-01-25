using UnityEngine;

public class PopParticles : MonoBehaviour
{
    private ParticleSystem mParticleSystem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mParticleSystem = GetComponent<ParticleSystem>();
        float duration = mParticleSystem.main.duration + mParticleSystem.main.startLifetime.constantMax;
        Destroy(this.gameObject, duration);
    }
}
