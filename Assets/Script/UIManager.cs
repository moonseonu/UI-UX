using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    private bool isLogin = true;

    [SerializeField] private GameObject Instance;

    [SerializeField] private List<GameObject> InstanceObject = new List<GameObject>();
    [SerializeField] private GameObject NewReview;
    [SerializeField] private GameObject NewPlan;
    [SerializeField] private GameObject WriteWindow;

    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject PrivacyPanel;

    [SerializeField] private GameObject PlanPanel;
    [SerializeField] private GameObject PlanningPanel;
    [SerializeField] private GameObject PlanLayout;

    [SerializeField] private GameObject AddButton;
    [SerializeField] private GameObject SearchPanel;


    [SerializeField] private List<GameObject> MyPlan = new List<GameObject>();
    [SerializeField] private GameObject NavigationBar;
    [SerializeField] private List<GameObject> PanelStack = new List<GameObject>();

    private void PanelSetActive(string name)
    {
        switch (name)
        {
            case "home":
                DestroyList();
                foreach (GameObject go in PanelStack)
                {
                    InstanceObject.Clear();
                    go.SetActive(false);
                }
                MainPanel.SetActive(true);
                PanelStack.Clear();
                break;

            case "plan":
                DestroyList();
                foreach (GameObject go in PanelStack)
                {
                    InstanceObject.Clear();
                    go.SetActive(false);
                }
                PlanPanel.SetActive(true);
                LoadPanel("Plan");
                PanelStack.Clear();
                AddPanelStack(PlanPanel);
                break;

            case "privacy":
                DestroyList();
                foreach (GameObject go in PanelStack)
                {
                    InstanceObject.Clear();
                    go.SetActive(false);
                }
                PrivacyPanel.SetActive(true);
                PanelStack.Clear();
                AddPanelStack(PrivacyPanel);
                break;
        }
    }

    private void AddPanelStack(GameObject panel)
    {
        PanelStack.Add(panel);
    }

    private void BackPanelStack(GameObject panel)
    {
        PanelStack.Remove(panel);
    }

    [SerializeField] private GameObject ReviewPanel;
    [SerializeField] private GameObject SavePanel;

    [SerializeField] private GameObject MotelListPanel;
    [SerializeField] private GameObject MotelLayout;
    [SerializeField] private GameObject MotelInfoPage;

    [SerializeField] private List<GameObject> MotelList = new List<GameObject>();
    [SerializeField] private List<GameObject> MotelPageList = new List<GameObject>();

    [SerializeField] private List<GameObject> ReviewList = new List<GameObject>();
    [SerializeField] private List<GameObject> SavedList = new List<GameObject>();

    [SerializeField] private List<GameObject> RecentlyPage = new List<GameObject>();

    private void InitRecentlyPage()
    {
        if(RecentlyPage.Count > 10)
        {
            RecentlyPage.Remove(RecentlyPage[9]);
        }
    }
    public void AddSavedList(GameObject list)
    {
        SavedList.Add(list);
    }
    public void DeleteSavedList(GameObject list)
    {
        SavedList.Remove(list);
    }
    public bool IsLogin
    {
        get { return isLogin; }
        set { isLogin = value; }
    }
    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InitRecentlyPage();
    }

    public void ButtonEvent(string name)
    {
        if (isLogin)
        {
            switch (name)
            {
                case "review":
                    ReviewPanel.gameObject.SetActive(true);
                    LoadPanel("Review");
                    PrivacyPanel.gameObject.SetActive(false);
                    AddPanelStack(ReviewPanel);
                    break;

                case "save":
                    SavePanel.SetActive(true);
                    LoadPanel("Save");
                    PrivacyPanel.gameObject.SetActive(false);
                    AddPanelStack(SavePanel);
                    break;

                case "setting":
                    break;

                case "back":
                    if (ReviewPanel.activeSelf)
                    {
                        DestroyList();
                        ReviewPanel.SetActive(false);
                        PrivacyPanel.SetActive(true);
                        NavigationBar.SetActive(true);
                        BackPanelStack(ReviewPanel);
                    }

                    else if (SavePanel.activeSelf)
                    {
                        DestroyList();
                        SavePanel.SetActive(false);
                        PrivacyPanel.SetActive(true);
                        NavigationBar.SetActive(true);
                        BackPanelStack(SavePanel);
                    }

                    else if (MotelListPanel.activeSelf)
                    {
                        if (MotelInfoPage.activeSelf)
                        {
                            DestroyList();
                            if (PanelStack.Contains(MotelInfoPage))
                            {
                                MotelLayout.SetActive(true);
                                MotelInfoPage.SetActive(false);
                                LoadPanel("Motel");
                                NavigationBar.SetActive(true);
                                BackPanelStack(MotelInfoPage);
                            }

                            else if (PanelStack.Contains(SavePanel))
                            {
                                MotelListPanel.SetActive(false);
                                SavePanel.SetActive(true);
                                LoadPanel("Save");
                                BackPanelStack(MotelInfoPage);
                            }
                        }
                        else if (MotelLayout.activeSelf)
                        {
                            DestroyList();
                            MotelListPanel.SetActive(false);
                            MainPanel.SetActive(true);
                            PanelStack.Clear();
                        }
                    }

                    else if (PlanningPanel.activeSelf)
                    {
                        DestroyList();
                        PlanPanel.SetActive(true);
                        PlanningPanel.SetActive(false);
                        NavigationBar.SetActive (true);
                        LoadPanel("Plan");
                        BackPanelStack(PlanningPanel);
                    }

                    else if (SearchPanel.activeSelf)
                    {
                        DestroyList();
                        if (PanelStack.Contains(PlanningPanel))
                        {
                            PlanningPanel.SetActive(true);
                            BackPanelStack(SearchPanel);
                            Instance = Instantiate(AddButton);
                            InstanceObject.Add(Instance);
                            Instance.transform.SetParent(GameObject.Find("Add Place content").transform, false);

                            Button nbutton = Instance.GetComponent<Button>();
                            if (nbutton != null)
                            {
                                nbutton.onClick.AddListener(() => LoadPanel("Search"));
                            }
                            SearchPanel.SetActive(false);
                        }

                        else
                        {
                            PanelSetActive("home");
                        }
                    }

                    break;

                case "newreview":
                    LoadPanel("WriteReview");
                    break;

                case "newplan":
                    PlanningPanel.SetActive(true);
                    PlanPanel.SetActive(false);
                    NavigationBar.SetActive(false);

                    AddPanelStack(PlanningPanel);

                    Instance = Instantiate(AddButton);
                    InstanceObject.Add(Instance);
                    Instance.transform.SetParent(GameObject.Find("Add Place content").transform, false);

                    Button button = Instance.GetComponent<Button>();
                    if (button != null)
                    {
                        button.onClick.AddListener(() => LoadPanel("Search"));
                    }
                    break;

                case "saving":

                    break;

                case "privacy":
                    MainPanel.SetActive(false);
                    PrivacyPanel.SetActive(true);
                    PlanPanel.SetActive(false);
                    PanelSetActive("privacy");
                    break;

                case "home":
                    MainPanel.SetActive(true);
                    PrivacyPanel.SetActive(false);
                    PlanPanel.SetActive(false);
                    PanelSetActive("home");
                    break;

                case "plan":
                    PrivacyPanel.SetActive(false);
                    MainPanel.SetActive(false);
                    PlanPanel.SetActive(true);
                    PanelSetActive("plan");
                    break;

                case "motel":
                    MotelListPanel.SetActive(true);
                    LoadPanel("Motel");
                    MainPanel.SetActive(false);
                    AddPanelStack(MotelListPanel);
                    break;

                case "search":
                    SearchPanel.SetActive(true);
                    LoadPanel("Search");
                    MainPanel.SetActive(false);
                    AddPanelStack(SearchPanel);
                    break;

            }
        }
    }

    private void LoadPanel(string name)
    {
        switch (name)
        {
            case "Review":
                GameObject Reviewtemp;
                for (int i = 0; i <= ReviewList.Count; i++)
                {
                    if (i == ReviewList.Count || ReviewList.Count == 0)
                    {
                        GameObject NewReviewButton = Instantiate(NewReview);
                        NewReviewButton.transform.SetParent(GameObject.Find("Review content").transform, false);
                        InstanceObject.Add(NewReviewButton);
                        Button button = NewReviewButton.GetComponent<Button>();
                        if (button != null)
                        {
                            button.onClick.AddListener(() => ButtonEvent("newreview"));
                        }
                    }

                    else
                    {
                        Reviewtemp = Instantiate(ReviewList[i]);
                        InstanceObject.Add(Reviewtemp);
                        Reviewtemp.transform.SetParent(GameObject.Find("Review content").transform, false);
                    }
                }
                break;

            case "Plan":
                GameObject Plantemp;
                for (int i = 0; i <= MyPlan.Count; i++)
                {
                    if (i == MyPlan.Count || MyPlan.Count == 0)
                    {
                        GameObject NewPlanButton = Instantiate(NewPlan);
                        NewPlanButton.transform.SetParent(GameObject.Find("Plan content").transform, false);
                        InstanceObject.Add(NewPlanButton);
                        Button button = NewPlanButton.GetComponent<Button>();
                        if (button != null)
                        {
                            button.onClick.AddListener(() => ButtonEvent("newplan"));
                        }
                    }
                    else
                    {
                        Plantemp = Instantiate(MyPlan[i]);
                        InstanceObject.Add(Plantemp);
                        Plantemp.transform.SetParent(GameObject.Find("Plan content").transform, false);
                    }
                }
                break;

            case "Save":
                GameObject Savetemp;
                for (int i = 0; i < SavedList.Count; i++)
                {
                    Savetemp = Instantiate(SavedList[i]);
                    InstanceObject.Add(Savetemp);
                    Savetemp.transform.SetParent(GameObject.Find("Save List content").transform, false);

                    int index = i;
                    Button button = Savetemp.GetComponent<Button>();
                    button.onClick.AddListener(() => MotelInfo(index, "save"));
                }
                break;

            case "WriteReview":
                GameObject temp = Instantiate(WriteWindow);
                InstanceObject.Add(temp);
                temp.transform.SetParent(GameObject.Find("WriteReviewPanel").transform, false);
                break;

            //모텔 정보는 개발자가 직접 추가하는 형식이므로 각 리스트 인덱스마다 어떤 장소인지 정해져있다 보면 됨.
            case "Motel":
                for (int i = 0; i < MotelList.Count; i++)
                {
                    GameObject motel = Instantiate(MotelList[i]);
                    InstanceObject.Add(motel);
                    motel.transform.SetParent(GameObject.Find("Motel List content").transform, false);

                    int index = i;
                    Button button = motel.GetComponent<Button>();
                    button.onClick.AddListener(() => MotelInfo(index, "motel"));
                }
                break;

            case "Search":
                if (PlanningPanel.activeSelf)
                {
                    AddPanelStack(SearchPanel);
                    SearchPanel.SetActive(true);
                    for (int i = 0; i < RecentlyPage.Count; i++)
                    {
                        Instance = Instantiate(RecentlyPage[i]);
                        InstanceObject.Add(Instance);
                        Instance.transform.SetParent(GameObject.Find("Recently content").transform, false);

                        int index = i;

                        Button button = Instance.GetComponent<Button>();

                        button.onClick.AddListener(() => AddPlan(RecentlyPage[index]));
                    }
                    PlanningPanel.SetActive(false);
                }
                break;

        }
    }

    public void MotelInfo(int i, string panel)
    {
        switch (panel)
        {
            case "save":
                MotelListPanel.SetActive(true);
                MotelLayout.SetActive(false);
                MotelInfoPage.SetActive(true);

                GameObject saveInfo = Instantiate(MotelPageList[i]);
                InstanceObject.Add(saveInfo);
                AddRecentlyPage(i, "motel");
                saveInfo.transform.SetParent(GameObject.Find("Info content").transform, false);

                Button savebutton = saveInfo.transform.Find("Saving Button").GetComponent<Button>();
                savebutton.onClick.AddListener(() => AddSaveList(i, "motel"));

                SavePanel.SetActive(false);
                break;

            case "motel":
                MotelLayout.SetActive(false);
                MotelInfoPage.SetActive(true);
                NavigationBar.SetActive(false);

                AddPanelStack(MotelInfoPage);
                Button motelbutton = null;

                GameObject Info = Instantiate(MotelPageList[i]);
                InstanceObject.Add(Info);
                Info.transform.SetParent(GameObject.Find("Info content").transform, false);

                motelbutton = Info.transform.Find("Saving Button").GetComponent<Button>();

                motelbutton.onClick.AddListener(() => AddSaveList(i, "motel"));
                AddRecentlyPage(i, "motel");


                break;
        }
    }
    public void AddPlan(GameObject go)
    {
        MyPlan.Add(go);
        ButtonEvent("back");
    }
    private void AddSaveList(int i, string name)
    {
        switch (name)
        {
            case "motel":
                if (!SavedList.Contains(MotelList[i]))
                    SavedList.Add(MotelList[i]);
                else
                    SavedList.Remove(MotelList[i]);
                break;
        }
    }

    private void AddRecentlyPage(int i, string name)
    {
        switch (name)
        {
            case "motel":
                if (!RecentlyPage.Contains(MotelList[i]))
                {
                    RecentlyPage.Add(MotelList[i]);
                }
                break;
        }
    }

    private void DestroyList()
    {
        foreach(GameObject go in InstanceObject)
        {
            Destroy(go);
        }
        InstanceObject.Clear();
    }
}
