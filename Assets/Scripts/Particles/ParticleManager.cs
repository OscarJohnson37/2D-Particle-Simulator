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

    public int NumParticles;

    Vector2 gravity = new Vector2(0, -9.81f);

    public void AddParticle(Particle particle)
    {

        particles.Add(particle);
    }

    public void RemovePartice (int i)
    {
        particles.RemoveAt(i);
    }

    public List<Particle> GetParticles()
    {
        return particles;
    }

    public void Update()
    {
        float dt = Time.deltaTime;
        circleRenderer.BeginFrame();

        for (int i = 0; i < particles.Count; i++)
        {
            Particle particle = particles[i];

            if (particle.isActive)
            {
                if (EnableGravity)
                {
                    particle.ApplyForce(gravity);
                }

                particle.Update(dt);

                checkBoundaryCollision(particle);

                for (int k = i + 1; k < particles.Count; k++)
                {
                    Particle compParticle = particles[k];

                    checkParticleCollision(particle, compParticle);

                    Debug.Log("particle speed: " + particle.velocity.x);
                }

                if (particle.velocity.magnitude < 0.1f)
                {
                    particle.velocity = Vector2.zero;
                }

                circleRenderer.DrawCircle(particle.position, particle.radius, particle.color);

                checkNumParticles();
            }
        }

        circleRenderer.EndFrame();
    }


    public void initialiseParticles(int numInitialParticles)
    {
        for (int i = 0; i < numInitialParticles; i++)
        {
            Vector2 pos = Random.insideUnitCircle*5f;
            Vector2 vel = Random.insideUnitCircle*5f;
            float rad = Random.Range(0.2f, 2f);
            particles.Add(new Particle(pos, vel, rad, Random.ColorHSV(), 1, 0.8f));
        }

        NumParticles =  numInitialParticles;
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

    private void ElasticCollion(Particle particle1, Particle particle2)
    {
        Vector2 delta = particle1.position - particle2.position;
        float distance = delta.magnitude;

        if (distance == 0f) return; // Prevent division by zero

        Vector2 normal = delta.normalized;
        Vector2 relativeVelocity = particle1.velocity - particle2.velocity;

        float velocityAlongNormal = Vector2.Dot(relativeVelocity, normal);

        if (velocityAlongNormal > 0) return; // They are moving away from each other

        // Elastic collision: coefficient of restitution = 1
        float massSum = particle1.mass + particle2.mass;
        float impulseMagnitude = (2 * velocityAlongNormal) / massSum;

        // Apply impulse
        particle1.velocity -= (impulseMagnitude * particle2.mass) * normal * particle1.dampingFactor;
        particle2.velocity += (impulseMagnitude * particle1.mass) * normal * particle2.dampingFactor;
    }

    void checkParticleCollision(Particle particle1, Particle particle2)
    {
        if ((particle1.position - particle2.position).magnitude < (particle1.radius + particle2.radius))
        {

            correctPositioning(particle1, particle2);

            ElasticCollion(particle1, particle2);

        }
    }

    void correctPositioning(Particle particle1, Particle particle2)
    {
        Vector2 PX1PX2 = particle1.position - particle2.position;
        Vector2 PX2PX1 = particle2.position - particle1.position;

        float SeperationDistance = PX1PX2.magnitude;
        float RequiredDistance = particle1.radius + particle2.radius;

        float MovementDistance = ((RequiredDistance - SeperationDistance)/2);

        Vector2 NormalisedPX1PX2 = PX1PX2.normalized;
        Vector2 NormalisedPX2PX1 = PX2PX1.normalized;

        Vector2 Particle1Movement = NormalisedPX1PX2 * MovementDistance;
        Vector2 Particle2Movement = NormalisedPX2PX1 * MovementDistance;

        particle1.position += Particle1Movement;
        particle2.position += Particle2Movement;
    }

    void checkNumParticles()
    {
        if (NumParticles < 0)
        {
            NumParticles = 0;
        }
        int particlesDiff = NumParticles - particles.Count;
        if (particlesDiff > 0)
        {
            for (int i = 0; i < particlesDiff; i++)
            {
                Vector2 pos = Random.insideUnitCircle*5f;
                Vector2 vel = Random.insideUnitCircle*5f;
                float rad = Random.Range(0.2f, 2f);
                particles.Add(new Particle(pos, vel, rad, Random.ColorHSV(), 1, 0.8f));
            }
        } else if (particlesDiff < 0)
        {
            particles.RemoveRange(particles.Count + particlesDiff, -particlesDiff);
        }
    }

}

