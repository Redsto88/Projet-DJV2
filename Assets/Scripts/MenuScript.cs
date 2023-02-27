using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{

    [SerializeField] private GameObject _options_menu;
    [SerializeField] private GameObject _credits_menu;
    [SerializeField] private GameObject _buttons;

    [SerializeField] private GameObject _options_button;
    [SerializeField] private GameObject _credits_button;


    public void PlayGame()
    {
        GameManager.Instance?.OnRestart();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void OptionsMenu()
    {
        _options_menu.SetActive(true);
        _buttons.SetActive(false);
        SetSelected();
    }

    public void BackToMenu()
    {
        _buttons.SetActive(true);
        if(_options_menu.activeInHierarchy)
        {
            _options_menu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(_options_button);
        }
        else if(_credits_menu.activeInHierarchy)
        {
            _credits_menu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(_credits_button);
        }
    }

    public void CreditsMenu()
    {
        _credits_menu.SetActive(true);
        _buttons.SetActive(false);
        SetSelected();
    }

    void Update()
    {
        Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (axis.magnitude > 0 && EventSystem.current.currentSelectedGameObject == null)
        {
            SetSelected();
        }
    }

    void SetSelected(){
        List<Button> buttonList = new List<Button>(GetComponentsInChildren<Button>());
        //get the first active button
        Button firstButton = buttonList.Find(x => x.gameObject.activeInHierarchy);
        EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
    }


}
