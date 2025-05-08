using UnityEngine;

public class Particle
{
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration = new Vector2(0, 0);
    public Color color;
    public float radius;
    public bool isActive;
    public float mass;

    public Particle(Vector2 pos, Vector2 vel, float rad, Color col, float m)
    {
        position = pos;
        velocity = vel;
        radius = rad;
        color = col;
        isActive = true;
        mass = m;
    }

    public void Update(float deltaTime)
    {
        if (!isActive) return;

        position += velocity * deltaTime;

        velocity += acceleration * deltaTime;

        acceleration.x = 0;
        acceleration.y = 0;
        
    }

    public void ReflectVertBound()
    {
        velocity.x = -velocity.x;
    }

    public void ReflectHoriBound()
    {
        velocity.y = -velocity.y;
    }

    public void ApplyForce(Vector2 Force)
    {
        acceleration += Force;
    }
}
