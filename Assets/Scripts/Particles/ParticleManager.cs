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

    public bool EnableGravity = true;

    Vector2 gravity = new Vector2(0, -9.81f);

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

            if (particle.isActive)
            {
                if (EnableGravity)
                {
                    particle.ApplyForce(gravity);
                }

                particle.Update(dt);

                checkBoundaryCollision(particle);

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
            particles.Add(new Particle(pos, vel, 0.5f, Random.ColorHSV(), 1));
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

    private void ElasticCollion(Particle particle1, Particle particle2, Particle* p1, Particle* p2)
    {
        Vector2 PX1PX2 = particle1.position - particle2.position;
        Vector2 PX2PX1 = particle2.position - particle1.position;
        Vector2 PV1PV2 = particle1.velocity - particle2.velocity;
        Vector2 PV2PV1 = particle2.velocity - particle1.velocity;

        float DotPV1PX2PX1PX2 = (PV1PV2.x * PX1PX2.x) + (PV1PV2.y + PX1PX2.y);
        float DotPV2PX1PX2PX1 = (PV2PV1.x * PX2PX1.x) + (PV2PV1.y + PX2PX1.y);

        float PX1PX2SquareMag = PX1PX2.sqrMagnitude;

        Vector2 NewVelParticle1 = particle1.velocity - ((2 * particle2.mass)/(particle1.mass + particle2.mass)) * (DotPV1PX2PX1PX2/PX1PX2SquareMag)*PX1PX2;
        Vector2 NewVelParticle2 = particle2.velocity - ((2 * particle1.mass)/(particle2.mass + particle1.mass)) * (DotPV2PX1PX2PX1/PX1PX2SquareMag)*PX2PX1;

        particle1.velocity = NewVelParticle1;
        particle2.velocity = NewVelParticle2;
    }

}

