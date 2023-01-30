using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    [SerializeField] private GameObject _options_menu;
    [SerializeField] private GameObject _credits_menu;


    public void PlayGame()
    {
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
    }

    public void BackToMenu()
    {
        _options_menu.SetActive(false);
        _credits_menu.SetActive(false);
    }

    public void CreditsMenu()
    {
        _credits_menu.SetActive(true);
    }


}
