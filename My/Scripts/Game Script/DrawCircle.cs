using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : MonoBehaviour
{
    public int numSegments = 100; // 원의 세그먼트(점) 수
    public float radius = 1f; // 원의 반지름
    public float lineWidth = 0.1f; // 선의 두께
    public Color lineColor = Color.red; // 선의 색상
    public GameObject cylinderPrefab;

    public LineRenderer lineRenderer;


    private void OnEnable()
    {
        StartCoroutine(FillCircleAnimation());
    }

    public void DrawCircleAtPosition(Vector3 position)
    {
        lineRenderer.positionCount = numSegments + 1;
        lineRenderer.useWorldSpace = true; // 월드 좌표계 사용
        lineRenderer.startWidth = lineWidth; // 선의 시작 부분 두께 설정
        lineRenderer.endWidth = lineWidth; // 선의 끝 부분 두께 설정

        Material material = new Material(Shader.Find("Standard"));
        material.color = lineColor;
        material.SetFloat("_Glossiness", 0f); // smoothness를 0으로 설정
        lineRenderer.material = material;
        
        lineRenderer.SetPositions(CalculateCirclePoints(position));
    }

    private Vector3[] CalculateCirclePoints(Vector3 position)
    {
        Vector3[] points = new Vector3[numSegments + 1];
        float deltaTheta = (2f * Mathf.PI) / numSegments;
        float theta = 0f;
        for (int i = 0; i < numSegments + 1; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);
            points[i] = new Vector3(x, 0f, z) + position;
            theta += deltaTheta;
        }
        return points;
    }

    private IEnumerator FillCircleAnimation()
    {
        Vector3 originalScale = cylinderPrefab.transform.localScale;
        Vector3 targetScale = new Vector3(radius*2, originalScale.y, radius*2);

        Material material = new Material(Shader.Find("Standard"));
        material.color = lineColor;
        material.SetFloat("_Glossiness", 0f); // smoothness를 0으로 설정

        cylinderPrefab.GetComponent<MeshRenderer>().material = material;

        float elapsedTime = 0f;

        while (elapsedTime < 3f)
        {
            float t = elapsedTime / 3f;
            cylinderPrefab.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cylinderPrefab.transform.localScale = originalScale;
        gameObject.SetActive(false);
    }
}
