using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WinCanvas : MonoBehaviour
{
    public GameObject first, middle, last;

    public void UpdateStars(int newValue)
    {
        bool oneStar = !(newValue > 77 && newValue <= 135);
        bool twoStar = !(newValue > 135 && newValue <= 174);
        first.SetActive(oneStar);
        last.SetActive(oneStar);
        middle.SetActive(twoStar);
    }
}
