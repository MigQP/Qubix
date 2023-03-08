using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator animator;
    private string sceneToLoad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void FadeToLevel(string levelName)
    {
        sceneToLoad = levelName;
        animator.SetTrigger("FadeOut");
        
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
