using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonHandler : MonoBehaviour
{
    [SerializeField] public Animator animator;
    public void playGame()
    {
        //animator.SetBool("isplaying", true);
        //Button button = GetComponent<Button>();
        SceneManager.LoadSceneAsync(1);

    }

    public void exitGame()
    {
        //animator.SetBool("isplaying", true);
        Application.Quit();
    }
}
