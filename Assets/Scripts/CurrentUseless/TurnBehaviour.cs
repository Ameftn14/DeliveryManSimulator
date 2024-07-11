using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBehaviour : MonoBehaviour
{
    public GameObject startVertex = null;
    public GameObject endVertex = null;

    // Start is called before the first frame update
    void Start()
    {
        // SetVertices(startVertex, endVertex);
    }

    void SetVertices(GameObject start, GameObject end)
    {
        // add a turn
        startVertex = start;
        endVertex = end;
        int startVid = startVertex.GetComponent<VertexBehaviour>().vid;
        int endVid = endVertex.GetComponent<VertexBehaviour>().vid;
        Vector3 startPosition = startVertex.transform.position;
        Vector3 endPosition = endVertex.transform.position;
        Vector3 direction1 = endPosition - transform.position;
        Vector3 direction2 = transform.position - startPosition;
        float angle = Vector3.Angle(direction1, direction2);
        float theta = Mathf.PI * angle / 180;
        if (angle <= 90)
        {
            transform.Find("BlackSquare").gameObject.SetActive(false);
            transform.Find("WhiteSquare").gameObject.SetActive(false);
            transform.Find("StartExtra").gameObject.SetActive(true);
            transform.Find("EndExtra").gameObject.SetActive(true);
            Vector3 localScale;
            GameObject startWhite = transform.Find("StartExtra").Find("WhiteSquare").gameObject;
            startWhite.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction1);
            localScale = startWhite.transform.localScale;
            localScale.y = localScale.x * Mathf.Tan(theta / 2);
            startWhite.transform.localScale = localScale;
            GameObject endWhite = transform.Find("EndExtra").Find("WhiteSquare").gameObject;
            endWhite.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction2);
            localScale = endWhite.transform.localScale;
            localScale.y = localScale.x * Mathf.Tan(theta / 2);
            endWhite.transform.localScale = localScale;
            GameObject startBlack = transform.Find("StartExtra").Find("BlackSquare").gameObject;
            startBlack.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction1);
            localScale = startBlack.transform.localScale;
            localScale.y = localScale.x * Mathf.Tan(theta / 2);
            startBlack.transform.localScale = localScale;
            GameObject endBlack = transform.Find("EndExtra").Find("BlackSquare").gameObject;
            endBlack.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction2);
            localScale = endBlack.transform.localScale;
            localScale.y = localScale.x * Mathf.Tan(theta / 2);
            endBlack.transform.localScale = localScale;
        }
        else
        {
            transform.Find("BlackSquare").gameObject.SetActive(true);
            transform.Find("WhiteSquare").gameObject.SetActive(true);
            transform.Find("StartExtra").gameObject.SetActive(false);
            transform.Find("EndExtra").gameObject.SetActive(false);
            float a1 = 1 / Mathf.Sin(theta);
            float scaleX = Mathf.Sqrt(2) * a1 * Mathf.Sin(theta / 2);
            float scaleY = Mathf.Sqrt(2) * a1 * Mathf.Cos(theta / 2);
            Vector3 newDirection = direction1 / direction1.magnitude + direction2 / direction2.magnitude;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, newDirection);
            transform.localScale = new Vector3(scaleX, scaleY, 1);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
