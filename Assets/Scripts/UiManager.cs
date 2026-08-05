using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject[] menus;
    private GameObject currentScreen;

    void Start()
    {
        foreach (GameObject menu in menus)
        {
            menu.SetActive(false);
        }
        menus[0].SetActive(true);
        currentScreen = menus[0];
    }

    public void ChangeScreen(int menuIndex)
    {
        currentScreen.SetActive(false);
        menus[menuIndex].SetActive(true);
        currentScreen = menus[menuIndex];
    }

    public void Quit()
    {
        Application.Quit();
    }
}
