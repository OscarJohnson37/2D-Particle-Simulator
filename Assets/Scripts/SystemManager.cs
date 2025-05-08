using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public ParticleManager particleManager;

    public Camera mainCamera;

    void Start()
    {
        Vector2 screenBounds = GetScreenBounds();
        particleManager.initialise(screenBounds);
        particleManager.initialiseParticles(10);
    }

    void Update()
    {
        particleManager.Update();
    }

    private Vector2 GetScreenBounds()
    {
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        return new Vector2(cameraWidth / 2f, cameraHeight / 2f); // Half extents
    }
}
