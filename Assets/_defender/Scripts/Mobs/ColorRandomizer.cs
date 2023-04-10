using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ColorRandomizer : MonoBehaviour
{
    [SerializeField] Renderer _renderer;

    void Update()
    {
        _renderer.material.color = Random.ColorHSV();
    }
}
