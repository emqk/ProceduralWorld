using UnityEngine;
using UnityEngine.UI;

public class SettingsCanvas : MonoBehaviour
{
    [SerializeField] RectTransform settingsPanel;
    [SerializeField] Button toggleSettingsButton;

    private void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        #if UNITY_ANDROID
                toggleSettingsButton.gameObject.SetActive(true);
        #else
                toggleSettingsButton.gameObject.SetActive(false);
        #endif

        QualitySettings.SetQualityLevel(5, true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingsPanel();
        }
    }

    public void ToggleSettingsPanel()
    {
        settingsPanel.gameObject.SetActive(!settingsPanel.gameObject.activeSelf);
        if (settingsPanel.gameObject.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void SetQualitySettings(Dropdown dropdown)
    {
        QualitySettings.SetQualityLevel(dropdown.value, true);
    }
}
