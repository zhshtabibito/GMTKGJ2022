using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance;
    private PanelManager panelManager;

    bool isReady;
    string sceneName;
    SceneInfo scene;



    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(scene == null)
        {
            scene = new StartScene();
            scene.OnEnter();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && panelManager.GetPeek().isExitIfClicked)
        {
            if (!panelManager.isMouseInPanel(panelManager.GetPeek()))
                panelManager.Pop();
        }
    }

    public void LoadScene(SceneInfo newScene, bool reload = true)
    {
        isReady = false;
        scene?.OnExit();
        scene = newScene;
        sceneName = newScene.SceneName;

        if (reload)
        {
            SceneManager.LoadScene(sceneName);
            SceneManager.sceneLoaded += SceneLoaded;
        }
        else
            scene?.OnEnter();
    }

    protected void SceneLoaded(Scene newScene, LoadSceneMode mode)
    {
        scene?.OnEnter();
        isReady = true;
        SceneManager.sceneLoaded -= SceneLoaded;
        Debug.Log($"{sceneName}场景加载完毕！");
    }

    /// <summary>
    /// 初始化面板管理器
    /// </summary>
    /// <param name="manager"></param>
    public void Initialize(PanelManager manager)
    {
        panelManager = manager;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
