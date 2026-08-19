using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject playerObject;
    private string currentScene;
    private string saveFilePath = Application.dataPath + "/SaveData/SaveData.json";

    private void Start()
    {
        DontDestroyOnLoad(this);

        // Used before the main menu was implemented, which spawns the player on starting a save
        //if (player == null)
        //    player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void LoadScene(string scene, Vector2 location)
    {
        player.blackScreen.GetComponent<Animator>().Play("Fade");
        SceneManager.LoadScene(scene);
        player.spawnLocation = location;
        player.transform.position = location;
        currentScene = scene;

        SaveGame();
    }

    public void NewGame()
    {
        SceneManager.LoadScene("Intro");
    }

    public void FinishGame()
    {
        StartCoroutine(Finish());
    }

    private IEnumerator Finish()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Outro");
    }

    public void StartNewGame()
    {
        GameObject p = Instantiate(playerObject);
        player = p.GetComponent<Player>();

        LoadScene("TutorialArea1", Vector2.zero);
    }

    private void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            scene = currentScene,
            playerPosition = player.spawnLocation,
            hasDoubleJump = player.hasDoubleJump,
            hasDash = player.hasDash,
            hasSprint = player.hasSprint,
            hasWallSlide = player.hasWallSlide
        };

        GameUtilities.Save<SaveData>(saveData, saveFilePath);
    }

    public void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogError("Save file not found!");
            return;
        }

        SaveData saveData = GameUtilities.Load<SaveData>(saveFilePath);

        GameObject p = Instantiate(playerObject);
        player = p.GetComponent<Player>();

        LoadScene(saveData.scene, saveData.playerPosition);
        player.hasDoubleJump = saveData.hasDoubleJump;
        player.hasDash = saveData.hasDash;
        player.hasSprint = saveData.hasSprint;
        player.hasWallSlide = saveData.hasWallSlide;
    }
}
