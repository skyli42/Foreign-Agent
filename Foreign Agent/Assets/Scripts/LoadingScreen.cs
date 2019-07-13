using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{

	public TextAsset txtFile;
	public TextMeshProUGUI tutorialText;
	private readonly Dictionary<int, List<int>> loadTextIndex = new Dictionary<int, List<int>> // level index --> possible text
	{
		{ 0, new List<int>{0}}, // MainMenu
		{ 1, new List<int>{0, 1, 2, 3, 4, 5, 6}}, // Tutorial1
		{ 2, new List<int>{0, 1, 2, 3, 4, 5, 6} }, //MacrophageIntro1
		{ 3, new List<int>{0, 1, 2, 3, 4, 5, 6 } }, //MacrophageIntro2
		{ 4, new List<int>{7, 8, 9, 10, 11, 12, 13} }, //BCellTutorial
		{ 5, new List<int>{7, 8, 9, 10, 11, 12, 13} }, //BCellIntro
		{ 6, new List<int>{7, 8, 9, 10, 11, 12, 13} }, //BCellIntro2
		{ 7, new List<int>{14, 15, 16, 17} }, //TCellTutorial
		{ 8, new List<int>{14, 15, 16, 17} }, //TCellIntro1
		{ 9, new List<int>{14, 15, 16, 17} }, //TCellIntro2
		{ 10, new List<int>{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17} }, //1.2
		{ 11, new List<int>{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17} }, //1.3
		{ 12, new List<int>{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17} }, //1.4
		{ 13, new List<int>{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19} }, //2.1
		{ 14, new List<int>{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19} }, //2.2
		{ 15, new List<int>{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19} }, //2.3
		{ 16, new List<int>{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19} } //Vaccine?

	};
    // Make sure the loading screen shows for at least 1 second:
    private const float MIN_TIME_TO_SHOW = 5f;
    // The reference to the current loading operation running in the background:
    private AsyncOperation currentLoadingOperation;
    // A flag to tell whether a scene is being loaded or not:
    private bool isLoading;
    // The rect transform of the bar fill game object:
    [SerializeField]
    private RectTransform barFillRectTransform;
    // Initialize as the initial local scale of the bar fill game object. Used to cache the Y-value (just in case):
    private Vector3 barFillLocalScale;
    // The text that shows how much has been loaded:
    [SerializeField]
    private Text percentLoadedText;
    // The elapsed time since the new scene started loading:
    private float timeElapsed;
    // Set to true to hide the progress bar:
    [SerializeField]
    private bool hideProgressBar;
    // Set to true to hide the percentage text:
    [SerializeField]
    private bool hidePercentageText;
    // The animator of the loading screen:
    private Animator animator;
    // Flag whether the fade out animation was triggered.
    private bool didTriggerFadeOutAnimation;
    public GameObject prevMenu;
	private string[] loadingTexts;
    private void Awake()
    {
        // Singleton logic:

        DontDestroyOnLoad(gameObject);
        Configure();
        Hide();
		LoadText();
    }
	private void LoadText()
	{
		string[] texts = txtFile.text.Split('\n');
		
		int y = SceneManager.GetActiveScene().buildIndex;
		loadingTexts = new string[loadTextIndex[y].Count];
		for (int i=0; i<loadTextIndex[y].Count; i++)
		{
			loadingTexts[i] = texts[loadTextIndex[y][i]];
		}
		
		int index = (int)Random.Range(0, loadingTexts.Length);
		Debug.Log(loadingTexts[index]);
		tutorialText.text = loadingTexts[index];
		//TODO: add picture too
	}
	private void Configure()
    {
        // Save the bar fill's initial local scale:
        barFillLocalScale = barFillRectTransform.localScale;
        // Enable/disable the progress bar based on configuration:
        barFillRectTransform.transform.parent.gameObject.SetActive(!hideProgressBar);
        // Enable/disable the percentage text based on configuration:
        percentLoadedText.gameObject.SetActive(!hidePercentageText);
        // Cache the animator:
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (isLoading)
        {
            // Get the progress and update the UI. Goes from 0 (start) to 1 (end):
            SetProgress(currentLoadingOperation.progress);
            // If the loading is complete and the fade out animation has not been triggered yet, trigger it:
            if (currentLoadingOperation.isDone && !didTriggerFadeOutAnimation)
            {
                animator.SetTrigger("Hide");
                didTriggerFadeOutAnimation = true;
                Destroy(gameObject, 5f);
            }
            else
            {
                timeElapsed += Time.deltaTime;
                if (timeElapsed >= MIN_TIME_TO_SHOW)
                {
                    // The loading screen has been showing for the minimum time required.
                    // Allow the loading operation to formally finish:
                    currentLoadingOperation.allowSceneActivation = true;
                }
            }
        }
    }
    // Updates the UI based on the progress:
    private void SetProgress(float progress)
    {
        // Update the fill's scale based on how far the game has loaded:
        barFillLocalScale.x = progress;
        // Update the rect transform:
        barFillRectTransform.localScale = barFillLocalScale;
        // Set the percent loaded text:
        percentLoadedText.text = Mathf.CeilToInt(progress * 100).ToString() + "%";
    }
    // Call this to show the loading screen.
    // We can determine the loading's progress when needed from the AsyncOperation param:
    public void Show(AsyncOperation loadingOperation)
    {

        // Enable the loading screen:
        gameObject.SetActive(true);
        prevMenu.SetActive(false);
        // Store the reference:
        currentLoadingOperation = loadingOperation;
        // Stop the loading operation from finishing, even if it technically did:
        currentLoadingOperation.allowSceneActivation = false;
        // Reset the UI:
        SetProgress(0f);
        // Reset the time elapsed:
        timeElapsed = 0f;
        // Play the fade in animation:
        animator.SetTrigger("Show");
        // Reset the fade out animation flag:
        didTriggerFadeOutAnimation = false;
        isLoading = true;

		//TODO: Reset information / display stuff
    }
    // Call this to hide it:
    public void Hide()
    {
        // Disable the loading screen:
        gameObject.SetActive(false);
        currentLoadingOperation = null;
        isLoading = false;
    }
}