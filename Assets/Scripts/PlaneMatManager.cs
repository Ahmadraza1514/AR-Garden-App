using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneMatManager : MonoBehaviour
{
    public Material _planeMat;
    public List<Button> planeTexButtons;

    void Start()
    {
        foreach (var item in planeTexButtons)
        {
            Texture texture = item.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture;
            item.onClick.AddListener(() => OnClickButton(texture));
        }

    }
    private void OnClickButton(Texture texture)
    {
        _planeMat.mainTexture = texture;
    }

}
