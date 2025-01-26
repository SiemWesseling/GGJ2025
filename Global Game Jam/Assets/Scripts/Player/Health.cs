using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private int numOfHearts;
    [SerializeField] private Image[] hearts;


    public void UpdateHealth()
    {
        numOfHearts--;
        if (numOfHearts == 0)
        {
            //Put here endgame logic
        }
    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
