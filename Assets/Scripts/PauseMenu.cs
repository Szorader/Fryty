using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private GameObject gameCanvas;

    [Header("Player")]
    [SerializeField] private MonoBehaviour cameraLookScript;
    
    [Header("Background")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite[] backgroundSprites;
    
    [Header("Audio")]
    [SerializeField] private EventReference bottle_squirt;
    // for the 3d audio to play like a 2d audio: 
    [SerializeField] private Transform playerTransform;

    private bool isPaused = false;
    
    public DeathScreenManager deathScreenManager;
    public DayManager dayManager;

    private void Start()
    {
        //deathScreenManager = FindObjectOfType<DeathScreenManager>();
        pauseMenuCanvas.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (deathScreenManager.isShowing)
                //Debug.Log("deathscreenManager.isShowing" +  deathScreenManager.isShowing);
                return;
            if (dayManager.isShowing)
                //Debug.Log("dayManager.isShowing" +  dayManager.isShowing);
                return;
            RuntimeManager.PlayOneShot(bottle_squirt, playerTransform.position);
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
            SetRandomBackground();

        pauseMenuCanvas.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;

        SetCursorState(isPaused);

        if (cameraLookScript != null)
            cameraLookScript.enabled = !isPaused;

        if (gameCanvas != null)
            gameCanvas.SetActive(!isPaused);
    }
    public void ResumeGame()
    {
        isPaused = false;

        pauseMenuCanvas.SetActive(false);
        gameCanvas.SetActive(true);

        Time.timeScale = 1f;

        SetCursorState(false);

        if (cameraLookScript != null)
            cameraLookScript.enabled = true;
    }
    
    
    public void LoadMainMenu(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
    
    private void SetRandomBackground()
    {
        if (backgroundImage == null || backgroundSprites == null || backgroundSprites.Length == 0)
            return;

        int index = Random.Range(0, backgroundSprites.Length);
        backgroundImage.sprite = backgroundSprites[index];
    }
    
    private void SetCursorState(bool paused)
    {
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = paused;
    }
}