using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LvlSelector : MonoBehaviour
{
    // Start is called before the first frame update
    public void Switch()
    {
        SceneManager.LoadScene("LevelMenu");
    }
}
