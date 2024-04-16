using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeColors : MonoBehaviour
{
    public Material[] cubeMaterials; // Assign this in the Unity editor
    public Dropdown childColorDropdown; // Assign this in the Unity editor
    public Dropdown grandChildColorDropdown; // Assign this in the Unity editor

    void Start()
    {
        childColorDropdown.onValueChanged.AddListener(SetChildMaterial);
        grandChildColorDropdown.onValueChanged.AddListener(SetGrandChildMaterial);
    }

    public void SetChildMaterial(int childMaterialIndex)
    {
        if (childMaterialIndex >= 0 && childMaterialIndex < cubeMaterials.Length)
        {
            foreach (Transform child in transform)
            {
                Renderer childRenderer = child.GetComponent<Renderer>();
                if (childRenderer != null)
                {
                    childRenderer.material = cubeMaterials[childMaterialIndex];
                }
            }
        }
    }

    public void SetGrandChildMaterial(int grandChildMaterialIndex)
    {
        if (grandChildMaterialIndex >= 0 && grandChildMaterialIndex < cubeMaterials.Length)
        {
            foreach (Transform child in transform)
            {
                foreach (Transform grandChild in child)
                {
                    Renderer grandChildRenderer = grandChild.GetComponent<Renderer>();
                    if (grandChildRenderer != null)
                    {
                        grandChildRenderer.material = cubeMaterials[grandChildMaterialIndex];
                    }
                }
            }
        }
    }
}