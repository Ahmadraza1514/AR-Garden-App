using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantsData", menuName = "PlantsData/Plant Collection")]
public class PlantsDataHolder : ScriptableObject
{
    [Header("List of Plants")]
    [Tooltip("Add the plant data here.")]
    public List<PlantData> plants = new List<PlantData>();
}

[System.Serializable]
public class PlantData
{
    public int id;

    [Tooltip("Image representing the plant.")]
    public Texture plantImage;

    [Tooltip("Name of the plant.")]
    public string plantName;

    [Tooltip("Description of the plant.")]
    [TextArea]
    public string description;
}
