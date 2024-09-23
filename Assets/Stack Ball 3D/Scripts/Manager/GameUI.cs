using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("InGame")]
    [SerializeField] private Image _levelSlider;
    [SerializeField] private Image _currentLevelImg;
    [SerializeField] private Image _nextLevelImg;

    private Material ballMaterial;

    #region UnityLifecycle
    void Awake()
    {
        ballMaterial = FindObjectOfType<Ball>().transform.GetChild(0).GetComponent<MeshRenderer>().material;

        _levelSlider.transform.parent.GetComponent<Image>().color = ballMaterial.color + Color.gray;
        _levelSlider.color = ballMaterial.color;
        _currentLevelImg.color = ballMaterial.color;
        _nextLevelImg.color = ballMaterial.color;
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region FunctionPublic
    public void LevelSliderFill(float _fillAmount)
    {
        _levelSlider.fillAmount = _fillAmount;
    }
    #endregion
}
