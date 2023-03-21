using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextFin : MonoBehaviour
{
    public void GoToMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Destroy(MapManager.Instance.gameObject);
        Destroy(GameManager.Instance.gameObject);
        Destroy(PlayerController.Instance.gameObject);
        Destroy(TimeManager.Instance.gameObject);
        Destroy(DialogManager.Instance.gameObject);
        Destroy(CinematicManager.Instance.gameObject);
        Destroy(RoomBehaviour.Instance.gameObject);
        CinematicManager.seen.Clear();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Menu");
    }
}
