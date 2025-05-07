using UnityEngine;

public class Particle
{
    public Vector2 position;
    public Vector2 velocity;
    public Color color;
    public float radius;
    public bool isActive;

    public Particle(Vector2 pos, Vector2 vel, float rad, Color col)
    {
        position = pos;
        velocity = vel;
        radius = rad;
        color = col;
        isActive = true;
    }

    public void Update(float deltaTime)
    {
        if (!isActive) return;

        position += velocity * deltaTime;
        
    }

    public void ReflectVertBound()
    {
        velocity.x = -velocity.x;
    }

    public void ReflectHoriBound()
    {
        velocity.y = -velocity.y;
    }
}
