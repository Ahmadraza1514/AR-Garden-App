using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Data.Common;

public class PlantsManager : MonoBehaviour
{
    public PlantsDataHolder plantsDataHolder;
    public GameObject ParentObj;
    public PlantButtonManager plantPrefab;

    void Start()
    {
        InstantiatePlants();
    }

    void InstantiatePlants()
    {
        foreach (PlantData plant in plantsDataHolder.plants)
        {
            GameObject plantInstance = Instantiate(plantPrefab.gameObject, ParentObj.transform);
            PlantButtonManager plantButtonManager = plantInstance.GetComponent<PlantButtonManager>();
            plantButtonManager.id = plant.id;
            plantButtonManager.nameText.text = plant.plantName;
            plantButtonManager.image.texture = plant.plantImage;
            plantButtonManager.description.text = plant.description;
            Button button = plantInstance.GetComponentInChildren<Button>();
            if (button != null)
            {
                plantButtonManager.button = button;
            }
        }
    }
}
