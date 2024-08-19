using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlantButtonManager : MonoBehaviour
{
    public int id; // Store the plant ID
    public TextMeshProUGUI nameText;
    public RawImage image;
    public TextMeshProUGUI description;
    public Button button; // Reference to the button component

    void Start()
    {
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    void OnButtonClick()
    {
        PlantsPlacementManager.instance.SetActivePlant(id);
    }
}
