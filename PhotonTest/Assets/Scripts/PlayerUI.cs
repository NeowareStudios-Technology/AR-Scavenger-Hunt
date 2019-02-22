using UnityEngine;
using UnityEngine.UI;


using System.Collections;



public class PlayerUI: MonoBehaviour
{


    #region Public Properties


    [Tooltip("UI Text to display Player's Name")]
    public Text PlayerNameText;


    [Tooltip("UI Slider to display Player's Health")]
    public Slider PlayerHealthSlider;


    #endregion


    #region Private Properties

    PlayerManager _target;

    #endregion


    #region MonoBehaviour Messages

    void Update()
    {
        // Reflect the Player Health
        if (PlayerHealthSlider != null)
        {
            PlayerHealthSlider.value = _target.Health;
        }
        if (_target == null)
        {
            Destroy(this.gameObject);
            return;
        }

    }
void Awake()
{
    this.GetComponent<Transform>().SetParent (GameObject.Find("Canvas").GetComponent<Transform>());
}

    #endregion


    #region Public Methods

    public void SetTarget(PlayerManager target)
{
    if (target == null)
    {
        Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.",this);
        return;
    }
    // Cache references for efficiency
    _target = target;
    if (PlayerNameText != null)
    {
       // PlayerNameText.text = _target.photonView.owner.name;
    }
}

    #endregion


}
