using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScreenScaler : MonoBehaviour
{
    [SerializeField] private Vector3 m_original_scale;
    private RectTransform m_canvas_rect;
    // Start is called before the first frame update
    void Start()
    {
        m_canvas_rect = GameObject.FindGameObjectWithTag("UIManager").GetComponent<RectTransform>();
        transform.localScale = new Vector3(m_original_scale.x * m_canvas_rect.localScale.x, m_original_scale.y * m_canvas_rect.localScale.y, m_original_scale.z * m_canvas_rect.localScale.z);
    }
}
