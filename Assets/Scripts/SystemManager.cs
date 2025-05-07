using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public ParticleManager particleManager;

    void Start()
    {
        particleManager.initialiseParticles(5);
    }

    void Update()
    {
        particleManager.Update();
    }
}
