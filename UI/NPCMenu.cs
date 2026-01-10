using PixelCrushers.DialogueSystem;
using PixelCrushers.DialogueSystem.ChatMapper;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCMenu : MonoBehaviour
{
    [SerializeField] private GameObject page;

    [SerializeField] private GameObject subtitlePanel;
    [SerializeField] private GameObject menuPanel;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI subtitleText;

    [Header("Prefabs")]
    [SerializeField] private GameObject closeButton;
    [SerializeField] private GameObject conversationItem;
    [SerializeField] private GameObject shopOption;


    [Header("Central Refs")]
    [SerializeField] public PixelCrushers.DialogueSystem.Wrappers.DialogueSystemTrigger trigger;
    [SerializeField] public DialogueDatabase db;

    private Vector2 optionsOGPos;
    [SerializeField] private Vector2 optionsHidePos;

    private Vector2 mainPanelOGPos;
    [SerializeField] private Vector2 mainPanelHidePos;

    List<string> greetings;

    private const float animDelay = 0.1f;
    public bool isOpen => page.activeSelf;
    private string actorName;
    public static NPCMenu i { get; private set; }

    private void Awake()
    {
        i = this;

        optionsOGPos = menuPanel.GetComponent<RectTransform>().anchoredPosition;
        mainPanelOGPos = subtitlePanel.GetComponent<RectTransform>().anchoredPosition;
    }
    private void Start()
    {
        page.SetActive(false);
        LeanTween.move(menuPanel.GetComponent<RectTransform>(), optionsHidePos, 0f);
        LeanTween.move(subtitlePanel.GetComponent<RectTransform>(), mainPanelHidePos, 0f);
    }
    public void Open(string actorName, List<string> greetings, float voicePitch, List<NPCConversation> conversations, List<NPCQuest> quests, ShopData shopData)
    {
        if (page.activeSelf) return;

        Game.ModifyCursorUnlockList(true, this);

        string displayName = DialogueLua.GetLocalizedActorField(actorName, "Display Name").asString;
        nameText.text = $"<wave a=0.1 s=1.6>{displayName}</wave>";
        this.greetings = greetings;
        this.actorName = actorName;

        PopulateMenu(conversations, quests, shopData);
        SetVisible(true); // greeting is set here

        LockActionMap.i.LockMovement(true);
        CameraManager.i.SetCamera(actorName);
    }
    public void Close()
    {
        Action midway = () =>
        {
            Game.ModifyCursorUnlockList(false, this);
            SetVisible(false);
            LockActionMap.i.LockMovement(false);
            CameraManager.i.RestorePlayerCam();
            actorName = null;
        };
        ScreenFade.i.Fade(NPCConstants.NPCMenuFadeLength, LeanTweenType.linear, LeanTweenType.linear, midway);
    }

    void PopulateMenu(List<NPCConversation> conversations, List<NPCQuest> quests, ShopData shopData)
    {
        // clear transform
        int childCount = menuPanel.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            GameObject child = menuPanel.transform.GetChild(i).gameObject;
            Destroy(child);
        }

        // add shop option if applicable
        if (shopData != null)
        {
            GameObject shop = Instantiate(shopOption, menuPanel.transform);
            shop.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = shopData.dialogueOptionLabel; 
            shop.GetComponent<Button>().onClick.AddListener(() => OpenShopMethod(shopData)); 
        }


        // add conversation options
        foreach (NPCConversation convo in conversations)
        {
            GameObject convoInstance = Instantiate(conversationItem, menuPanel.transform);
            ConversationMenuItem convoItem = convoInstance.GetComponent<ConversationMenuItem>();
            convoItem.Initialize(convo, trigger, db);
        }

        GameObject go = Instantiate(closeButton, menuPanel.transform);
        Button go_btn = go.GetComponent<Button>();
        go_btn.onClick.AddListener(Close);

    }

    private void OpenShopMethod(ShopData shopData)
    {
        SetVisible(false);
        ShopMenu.i.SetMenu(true, shopData);
    }

    public void SetVisible(bool value)
    {
        if (value)
        {
            subtitleText.text = greetings[UnityEngine.Random.Range(0, greetings.Count)];

            // anim
            LeanTween.alphaCanvas(menuPanel.GetComponent<CanvasGroup>(), 1f, 0.08f);
            LeanTween.move(menuPanel.GetComponent<RectTransform>(), optionsOGPos, 0.15f);

            LeanTween.alphaCanvas(subtitlePanel.GetComponent<CanvasGroup>(), 1f, 0.08f).setDelay(animDelay);
            LeanTween.move(subtitlePanel.GetComponent<RectTransform>(), mainPanelOGPos, 0.15f).setDelay(animDelay);

            page.SetActive(true);
            if (actorName != null) CameraManager.i.SetCamera(actorName);
        }
        else
        {

            LeanTween.alphaCanvas(menuPanel.GetComponent<CanvasGroup>(), 0f, 0.08f);
            LeanTween.move(menuPanel.GetComponent<RectTransform>(), optionsHidePos, 0.15f)
                .setOnComplete(() =>
                {
                    page.SetActive(false);
                });

            LeanTween.alphaCanvas(subtitlePanel.GetComponent<CanvasGroup>(), 0f, 0.08f).setDelay(animDelay);
            LeanTween.move(subtitlePanel.GetComponent<RectTransform>(), mainPanelHidePos, 0.15f).setDelay(animDelay);
        }

    }
}
