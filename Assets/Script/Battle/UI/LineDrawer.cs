using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    private Camera myMainCam;
    private RectTransform m_rect;

    void Awake()
    {
        myMainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        m_rect = gameObject.GetComponent<RectTransform>();
    }


    public void SetLine_Worlds(Transform start, Vector2 end)
    {
        Vector2 m_start = myMainCam.WorldToScreenPoint(start.position);
        //Debug.Log("S:" + m_start);
        Vector2 m_end = myMainCam.WorldToScreenPoint(end);
        //Debug.Log("E" + m_end);

        Vector2 m_vector = m_end - m_start;
        float rotateValue;
        if (m_start.x < m_end.x)
        {
             rotateValue = -Vector2.Angle(start.up, m_vector.normalized);
        }
        else
        {
            rotateValue = Vector2.Angle(start.up, m_vector.normalized);
        }
        //Debug.Log("V" + m_vector);
        Vector2 middlePoint = m_start + (m_vector / 2);


        float magnitude = m_vector.magnitude;
        

        m_rect.position = middlePoint;
        m_rect.rotation = Quaternion.Euler(new Vector3(0, 0, rotateValue));
        m_rect.localScale = new Vector2(1, magnitude);
    }

    public void SetLine_Canvas(Transform start, Vector2 end)
    {
        Vector2 m_start = start.position;
        //Debug.Log("S:" + m_start);
        Vector2 m_end = myMainCam.WorldToScreenPoint(end);
        //Debug.Log("E" + m_end);

        Vector2 m_vector = m_end - m_start;
        float rotateValue;
        if (m_start.x < m_end.x)
        {
            rotateValue = -Vector2.Angle(start.up, m_vector.normalized);
        }
        else
        {
            rotateValue = Vector2.Angle(start.up, m_vector.normalized);
        }
        //Debug.Log("V" + m_vector);
        Vector2 middlePoint = m_start + (m_vector / 2);


        float magnitude = m_vector.magnitude;


        m_rect.position = middlePoint;
        m_rect.rotation = Quaternion.Euler(new Vector3(0, 0, rotateValue));
        m_rect.localScale = new Vector2(1, magnitude);
    }
}
