using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private GameObject gameCanvas;

    [Header("Player")]
    [SerializeField] private MonoBehaviour cameraLookScript;
    
    [Header("Audio")]
    [SerializeField] private EventReference bottle_squirt;
    // for the 3d audio to play like a 2d audio: 
    [SerializeField] private Transform playerTransform;

    private bool isPaused = false;
    
    public DeathScreenManager deathScreenManager;

    private void Start()
    {
        deathScreenManager = FindObjectOfType<DeathScreenManager>();
        pauseMenuCanvas.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !deathScreenManager.isShowing)
        {
            RuntimeManager.PlayOneShot(bottle_squirt, playerTransform.position);
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        //RuntimeManager.PlayOneShot(bottle_squirt);

        pauseMenuCanvas.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;

        Cursor.lockState = isPaused
            ? CursorLockMode.None
            : CursorLockMode.Locked;

        Cursor.visible = isPaused;

        if (cameraLookScript != null)
        {
            cameraLookScript.enabled = !isPaused;
        }

        if (gameCanvas != null)
        {
            gameCanvas.SetActive(!isPaused);
        }
    }
    public void ResumeGame()
    {
        isPaused = false;
    
        pauseMenuCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cameraLookScript != null)
        {
            cameraLookScript.enabled = true;
        }
    }
    
    public void LoadMainMenu(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}