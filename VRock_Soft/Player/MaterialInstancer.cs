using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInstancer : MonoBehaviour
{
   private MeshRenderer m_Renderer;

    [SerializeField]
    private Color m_Color;
    void Start()
    {
        m_Renderer = GetComponent<MeshRenderer>();
        m_Renderer.material = Instantiate(m_Renderer.material);
        m_Renderer.material.SetColor("m_Color",m_Color);
    }

}
