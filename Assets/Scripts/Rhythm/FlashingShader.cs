using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingShader : MonoBehaviour
{

    Material mat;
    Color goalColor, initColor;

    // Start is called before the first frame update
    void Start()
    {
        initColor = mat.GetColor(Shader.PropertyToID("Shadow Color"));
    }

    // Update is called once per frame
    void Update()
    {
        
        mat.SetColor(Shader.PropertyToID("Shadow Color"), goalColor);
    }
}
