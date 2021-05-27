using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageRayCastTargetOff : MonoBehaviour
{
    [SerializeField] private IAbleEvent target;
    private Image myImage;

    // Start is called before the first frame update
    void Start()
    {
        myImage = GetComponent<Image>();
        target.enable += () => myImage.raycastTarget = false;
        target.disable += () => myImage.raycastTarget = true;
    }
}
