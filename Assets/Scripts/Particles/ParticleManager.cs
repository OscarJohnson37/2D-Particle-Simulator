using UnityEngine;
using System.Collections.Generic;

public class ParticleManager : MonoBehaviour
{
    public List<Particle> particles = new List<Particle>();

    public CircleRenderer circleRenderer;

    public void AddParticle(Particle particle)
    {
        particles.Add(particle);
    }

    public List<Particle> GetParticles()
    {
        return particles;
    }

    public void Update()
    {
        float dt = Time.deltaTime;
        circleRenderer.BeginFrame();

        foreach (var particle in particles)
        {
            particle.Update(dt);
            if (particle.isActive)
            {
                circleRenderer.DrawCircle(particle.position, particle.radius, particle.color);
            }
        }

        circleRenderer.EndFrame();
    }


    public void initialiseParticles(int numInitialParticles)
    {
        for (int i = 0; i < numInitialParticles; i++)
        {
            Vector2 pos = Random.insideUnitCircle * 3f;
            Vector2 vel = Random.insideUnitCircle;
            particles.Add(new Particle(pos, vel, 0.5f, Random.ColorHSV()));
        }
    }
}

