using UnityEngine;
using System.Collections.Generic;

public class ParticleManager : MonoBehaviour
{
    public List<Particle> particles = new List<Particle>();

    public CircleRenderer circleRenderer;

    private float upperBoundary;
    private float lowerBoundary;
    private float leftBoundary;
    private float rightBoundary;

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

            checkBoundaryCollision(particle);

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
            Vector2 pos = new Vector2(0, 0);
            Vector2 vel = Random.insideUnitCircle*5f;
            particles.Add(new Particle(pos, vel, 0.5f, Random.ColorHSV()));
        }
    }

    public void initialise(Vector2 screenBounds)
    {
        calculateBoundaries(screenBounds);
    }

    private void calculateBoundaries(Vector2 screenBounds)
    {
        upperBoundary = screenBounds.y;
        lowerBoundary = -screenBounds.y;
        leftBoundary = -screenBounds.x;
        rightBoundary = screenBounds.x;
    }

    private void checkBoundaryCollision(Particle particle)
    {
        if (particle.position.x < leftBoundary + particle.radius)
        {
            particle.ReflectVertBound();
            particle.position.x = leftBoundary + particle.radius;
        }
        else if (particle.position.x > rightBoundary - particle.radius)
        {
            particle.ReflectVertBound();
            particle.position.x = rightBoundary - particle.radius;
        }

        if (particle.position.y < lowerBoundary + particle.radius)
        {
            particle.ReflectHoriBound();
            particle.position.y = lowerBoundary + particle.radius;
        }
        else if (particle.position.y > upperBoundary - particle.radius)
        {
            particle.ReflectHoriBound();
            particle.position.y = upperBoundary - particle.radius;
        }
    }
}
