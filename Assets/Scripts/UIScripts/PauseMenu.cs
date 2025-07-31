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
        Debug.Log("[PauseMenu] Pause called.");
        pauseMenu.SetActive(true);

        timeManager.isPaused = true;
        timeManager.SaveTimeScaleBeforePause();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _isPaused = true;
        playerMovement.isPaused = true;
        mouseMovement.isPaused = true;
        weaponController.isPaused = true;

        Debug.Log($"[PauseMenu] Paused: isPaused = {timeManager.isPaused}, Time.timeScale = {Time.timeScale}");
    }

    public void Resume()
    {
        Debug.Log("[PauseMenu] Resume called.");
        pauseMenu.SetActive(false);

        timeManager.isPaused = false;
        timeManager.RestoreTimeScaleAfterResume();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _isPaused = false;
        playerMovement.isPaused = false;
        mouseMovement.isPaused = false;
        weaponController.isPaused = false;

        Debug.Log($"[PauseMenu] Resumed: isPaused = {timeManager.isPaused}, Time.timeScale = {Time.timeScale}");
    }


}
