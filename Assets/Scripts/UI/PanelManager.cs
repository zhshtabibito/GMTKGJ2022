using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelManager : MonoBehaviour
{
    public static PanelManager Instance;

    private Dictionary<string, GameObject> dictUI;
    private Dictionary<string, BasePanel> dictPanel;
    private Stack<BasePanel> panelStack;

    public Transform BlackMask;
    private RawImage BlackMaskCpn;

    private void Awake()
    {
        Instance = this;

        Screen.SetResolution(444, 960, false);

        dictUI = new Dictionary<string, GameObject>();
        dictPanel = new Dictionary<string, BasePanel>();
        panelStack = new Stack<BasePanel>();

        BlackMaskCpn = BlackMask.GetComponent<RawImage>();
    }

    private void Start()
    {
        Push(new StartPanel());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && panelStack.Peek().isExitIfClicked)
        {
            if (!isMouseInPanel(panelStack.Peek()))
                Pop();
        }

    }

    /*
    private Vector3 GetMousePos()
    {
        Vector3 res = Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.WorldToScreenPoint(transform.position).z))
            + new Vector3(0, -0.16f, 0) - MainRoot.position;
        // Debug.Log(res);
        return res;
    }
    */

    public GameObject GetSingleUI(PanelInfo panel)
    {
        if (dictUI.ContainsKey(panel.Path))
        {
            return dictUI[panel.Path];
        }
        GameObject obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(panel.Path), transform);
        obj.name = panel.Name;
        dictUI.Add(panel.Path, obj);
        return obj;
    }

    public void DestroyUI(PanelInfo panel, bool isDestroy = false)
    {
        if (dictUI.ContainsKey(panel.Path))
        {
            if (isDestroy)
            {
                GameObject.Destroy(dictUI[panel.Path]);
                dictUI.Remove(panel.Path);
                dictPanel.Remove(panel.Path);
            }
            else
            {
                dictUI[panel.Path].transform.PanelAppearance(false);
            }
        }
    }

    public BasePanel Push(BasePanel newPanel)
    {
        if (panelStack.Count > 0)
            panelStack.Peek().OnPause();
        // ά���ֵ�
        if (!dictPanel.ContainsKey(newPanel.Info.Path))
        {
            dictPanel.Add(newPanel.Info.Path, newPanel);
            GameObject obj = GetSingleUI(newPanel.Info);
            newPanel.ActivePanel = obj.transform;
            newPanel.Initialize(this);
        }
        else
        {
            // ˢ���ֵ��е�panel����add new panal
            BasePanel p = dictPanel[newPanel.Info.Path];
            p.OnChange(newPanel);
            newPanel = p;
        }
        newPanel.OnEnter();
        // Push
        if (panelStack.Count > 0)
        {
            // ��ֹ���������ظ������
            if (panelStack.Peek().Info.Path != newPanel.Info.Path)
                panelStack.Push(newPanel);
            else
                return panelStack.Peek();
        }
        else
            panelStack.Push(newPanel);

        return newPanel;
    }

    public void Pop()
    {
        if (panelStack.Count > 0)
            panelStack.Pop().OnExit();
        if (panelStack.Count > 0)
            panelStack.Peek().OnResume();
    }

    public void PopAll()
    {
        var values = new List<BasePanel>(dictPanel.Values);
        while (values.Count > 0)
        {
            values[0].OnExit(true);
            values.RemoveAt(0);
        }
    }

    private bool isMouseInPanel(BasePanel panel)
    {
        // https://blog.csdn.net/kun1234567/article/details/78684104
        RectTransform rect = panel.ActivePanel.GetComponent<RectTransform>();
        Rect spaceRect = rect.rect;
        spaceRect.x = spaceRect.x * rect.lossyScale.x + rect.position.x;
        spaceRect.y = spaceRect.y * rect.lossyScale.y + rect.position.y;
        spaceRect.width = spaceRect.width * rect.lossyScale.x;
        spaceRect.height = spaceRect.height * rect.lossyScale.y;
        if (spaceRect.Contains(Input.mousePosition))
            return true;
        else
            return false;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void PushWithCutscene(BasePanel newPanel)
    {
        StartCoroutine("MaskAndPush", newPanel);
    }

    private IEnumerator MaskAndPush(BasePanel newPanel)
    {
        BlackMask.gameObject.SetActive(true);
        BlackMask.SetAsLastSibling();
        for (float x = 0f; x < 1.0f; x += Time.deltaTime / 2f)
        {
            BlackMaskCpn.color = Color.Lerp(Color.clear, Color.black, x);
            yield return null;
        }
        Pop();
        Push(newPanel);
        BlackMask.SetAsLastSibling();
        for (float x = 1f; x > 0f; x -= Time.deltaTime / 2f)
        {
            BlackMaskCpn.color = Color.Lerp(Color.clear, Color.black, x);
            yield return null;
        }
        BlackMask.gameObject.SetActive(false);
    //    if(panelStack.Peek().GetType() == typeof(CutPanel))
    //    {
    //        CutManager.Instance.OnFinishLoad();
    //    }
    }
}
