using UnityEngine;
using System.Collections.Generic;

public class CircleRenderer : MonoBehaviour
{
    public GameObject circlePrefab; // Assign in Inspector
    private List<GameObject> circlePool = new List<GameObject>();
    private int nextAvailableIndex = 0;

    public void BeginFrame()
    {
        nextAvailableIndex = 0; // Reset for new frame
    }

    public void DrawCircle(Vector2 position, float radius, Color color)
    {
        GameObject circleObj;

        // Reuse or create new
        if (nextAvailableIndex < circlePool.Count)
        {
            circleObj = circlePool[nextAvailableIndex];
        }
        else
        {
            circleObj = Instantiate(circlePrefab);
            circlePool.Add(circleObj);
        }

        nextAvailableIndex++;

        circleObj.SetActive(true);
        circleObj.transform.position = position;
        circleObj.transform.localScale = Vector3.one * radius * 2f; // Scale assumes sprite has diameter 1
        circleObj.GetComponent<SpriteRenderer>().color = color;
    }

    public void EndFrame()
    {
        // Hide unused circles
        for (int i = nextAvailableIndex; i < circlePool.Count; i++)
        {
            circlePool[i].SetActive(false);
        }
    }
}
