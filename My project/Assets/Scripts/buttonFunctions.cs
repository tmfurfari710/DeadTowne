using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    // Start is called before the first frame update
   public void resume()
    {
        gameManager.instance.stateUnpaused();
    }

  public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateUnpaused();
    }

    public void quit()
    {
        Application.Quit();
    }

}
