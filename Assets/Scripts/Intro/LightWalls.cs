using System;
using UnityEngine;

public class LightWalls : MonoBehaviour
{
    [SerializeField] private float minPlayerPosition;
    [SerializeField] private float maxPlayerPosition;
    [SerializeField] private Transform player;
    [SerializeField] private Material progressiveLightMaterial;

    private void Update()
    {
        progressiveLightMaterial.SetFloat("_Value", Mathf.InverseLerp(minPlayerPosition, maxPlayerPosition, player.position.z));
    }
}
