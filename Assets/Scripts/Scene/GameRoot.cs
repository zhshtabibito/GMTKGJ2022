using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance;
    private PanelManager panelManager;

    private bool isReady;
    private string sceneName;
    public SceneInfo scene;

    public RawImage BlackMaskCpn = null;

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
        BlackMaskCpn = GameObject.Find("CanvasMask").GetComponent<RawImage>();
        BlackMaskCpn.gameObject.SetActive(false);

        if (scene == null)
        {
            scene = new StartScene();
            scene.OnEnter();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && panelManager != null && panelManager.GetPeek()!=null && panelManager.GetPeek().isExitIfClicked)
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
        if (BlackMaskCpn != null)
        {
            BlackMaskCpn.color = Color.clear;
            BlackMaskCpn.gameObject.SetActive(true);

            BlackMaskCpn.transform.SetAsLastSibling();
            for (float x = 0f; x < 1.0f; x += Time.deltaTime / 1f)
            {
                BlackMaskCpn.color = Color.Lerp(Color.clear, Color.black, x);
                yield return null;
            }
        }
        isReady = false;
        scene?.OnExit();
        scene = newScene;
        sceneName = newScene.SceneName;

        SceneManager.LoadScene(sceneName);
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private IEnumerator HideMask()
    {
        BlackMaskCpn = GameObject.Find("CanvasMask").GetComponent<RawImage>();

        BlackMaskCpn.transform.SetAsLastSibling();
        for (float x = 0f; x < 1.0f; x += Time.deltaTime / 1f)
        {
            BlackMaskCpn.color = Color.Lerp(Color.black, Color.clear, x);
            yield return null;
        }
        BlackMaskCpn.gameObject.SetActive(false);
    }

}
