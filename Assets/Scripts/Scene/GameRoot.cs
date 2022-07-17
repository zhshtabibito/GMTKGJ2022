using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance;
    private PanelManager panelManager;

    private bool isReady;
    private string sceneName;
    public SceneInfo scene;



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
        if (Input.GetMouseButtonDown(0) && panelManager.GetPeek()!=null && panelManager.GetPeek().isExitIfClicked)
        {
            if (!panelManager.isMouseInPanel(panelManager.GetPeek()))
                panelManager.Pop();
        }
    }

    public void LoadScene(SceneInfo newScene, bool reload = true)
    {


        if (reload)
        {
            // SceneManager.LoadScene(sceneName);
            // SceneManager.sceneLoaded += SceneLoaded;
            StartCoroutine("ShowMaskAndLoadScene", newScene);
        }
        else
        {
            isReady = false;
            scene?.OnExit();
            scene = newScene;
            sceneName = newScene.SceneName;
            scene?.OnEnter();
        }
    }

    protected void SceneLoaded(Scene newScene, LoadSceneMode mode)
    {
        scene?.OnEnter();
        isReady = true;
        SceneManager.sceneLoaded -= SceneLoaded;
        Debug.Log($"{sceneName} loaded!");
    }

    /// <summary>
    /// ��ʼ����������
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

    private IEnumerator ShowMaskAndLoadScene(SceneInfo newScene)
    {
        Debug.Log(panelManager);
        Debug.Log(panelManager.BlackMaskCpn);
        Debug.Log(panelManager.BlackMaskCpn.color);


        panelManager.BlackMaskCpn.color = Color.clear;
        panelManager.BlackMaskCpn.gameObject.SetActive(true);

        panelManager.BlackMaskCpn.transform.SetAsLastSibling();
        for (float x = 0f; x < 1.0f; x += Time.deltaTime / 1f)
        {
            panelManager.BlackMaskCpn.color = Color.Lerp(Color.clear, Color.black, x);
            yield return null;
        }

        isReady = false;
        scene?.OnExit();
        scene = newScene;
        sceneName = newScene.SceneName;

        SceneManager.LoadScene(sceneName);
        SceneManager.sceneLoaded += SceneLoaded;

        //BlackMask.SetAsLastSibling();
        //for (float x = 1f; x > 0f; x -= Time.deltaTime / 2f)
        //{
        //    BlackMaskCpn.color = Color.Lerp(Color.clear, Color.black, x);
        //    yield return null;
        //}
        //BlackMask.gameObject.SetActive(false);
        //if (panelStack.Peek().GetType() == typeof(CutPanel))
        //{
        //    CutManager.Instance.OnFinishLoad();
        //}
    }


}
