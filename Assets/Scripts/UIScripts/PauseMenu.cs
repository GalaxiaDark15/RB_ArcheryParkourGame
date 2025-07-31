using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private TimeManager timeManager;

    [SerializeField]
    private PlayerMovement playerMovement;

    [SerializeField]
    private MouseMovement mouseMovement;

    [SerializeField]
    private WeaponController weaponController;

    private static bool _isPaused = false;
    public static bool IsPaused => _isPaused;

    void Update()
    {
        if (!_isPaused && Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
        else if (_isPaused && Input.GetKeyDown(KeyCode.P))
        {
            Resume();
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _isPaused = true;
        playerMovement.isPaused = true;
        mouseMovement.isPaused = true;
        weaponController.isPaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _isPaused = false;
        playerMovement.isPaused = false;
        mouseMovement.isPaused = false;
        weaponController.isPaused = false;
    }
}
