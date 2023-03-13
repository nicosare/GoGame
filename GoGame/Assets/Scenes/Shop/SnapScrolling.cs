using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour
{
    public Shop shop;
    public GameObject panPrefab;
    private int panCount;
    private GameObject[] instPans;
    public float panOffset;
    private Vector2[] panPos;
    private RectTransform contentRect;
    public int selectedPanID;
    private bool isScrolling;
    private Vector2 contentPos;
    public float snapSpeed;
    private Vector2[] panScale;
    public float scaleOffset;
    public float scaleSpeed;
    public ScrollRect scrollRect;
    public float scrollSpeed;

    void Start()
    {
        panCount = shop.skins.Count;
        panScale = new Vector2[panCount];
        contentRect = GetComponent<RectTransform>();
        instPans = new GameObject[panCount];
        panPos = new Vector2[panCount];
        for (int i = 0; i < panCount; i++)
        {
            instPans[i] = Instantiate(panPrefab, transform, false);
            instPans[i].transform.GetChild(0).GetComponent<Image>().sprite = shop.skins[i].sprite;
            instPans[i].transform.GetChild(1).GetComponent<Text>().text = shop.skins[i].modelName;

            if (i == 0) continue;
            instPans[i].transform.localPosition = new Vector2(instPans[i - 1].transform.localPosition.x
                                                            + panPrefab.GetComponent<RectTransform>().sizeDelta.x + panOffset
                                                            , instPans[i].transform.localPosition.y);
            panPos[i] = -instPans[i].transform.localPosition;
        }
    }

    private void FixedUpdate()
    {
        var nearestPos = float.MaxValue;
        for (int i = 0; i < panCount; i++)
        {
            var distance = Mathf.Abs(contentRect.anchoredPosition.x - panPos[i].x);
            if (distance < nearestPos)
            {
                nearestPos = distance;
                selectedPanID = i;
            }
            var scale = Mathf.Clamp(1 / (distance / panOffset) * scaleOffset, 0.5f, 1f);
            panScale[i].x = Mathf.SmoothStep(instPans[i].transform.localScale.x, scale, scaleSpeed * Time.fixedDeltaTime);
            panScale[i].y = Mathf.SmoothStep(instPans[i].transform.localScale.y, scale, scaleSpeed * Time.fixedDeltaTime);
            instPans[i].transform.localScale = panScale[i];
        }
        var scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if (scrollVelocity < scrollSpeed + 200 && !isScrolling)
            scrollRect.inertia = false;
        if (isScrolling && scrollVelocity >= scrollSpeed - 200) return;
        contentPos.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, panPos[selectedPanID].x, snapSpeed * Time.fixedDeltaTime);
        contentRect.anchoredPosition = contentPos;
    }

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
        if (isScrolling)
            scrollRect.inertia = true;
    }
}
