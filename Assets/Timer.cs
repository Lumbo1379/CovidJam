using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public int StartingTime;
    public BoxCollider ColliderTemplate;
    public LayerMask ObjectsInHouseMask;
    public Transform House;
    public CatalogueContents CatalogueContents;
    public GameObject ScorePanel;
    public TextMeshProUGUI FinalScoreText;
    [Range(0, 3)] public float DelayBetweenScoreDisplay;

    private int _time;
    private TextMeshProUGUI _text;
    private bool _started;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _time = StartingTime;
        _started = true;

        _text.text = _time.ToString();
        InvokeRepeating("DecreaseTime", 1f, 1f);
    }

    private void DecreaseTime()
    {
        _time--;

        if (_time > 0)
            _text.text = _time.ToString();
        else
            StartCoroutine(CalculateScore());
    }

    private IEnumerator CalculateScore()
    {
        CancelInvoke("DecreaseTime");
        _text.enabled = false;
        ScorePanel.SetActive(true);
        FinalScoreText.gameObject.SetActive(true);

        var contents = CatalogueContents.ObjectsInHouse;
        var scoreChart = new List<ContentChart>();
        var wait = new WaitForSeconds(DelayBetweenScoreDisplay);

        foreach (var obj in contents)
        {
            if (scoreChart.Count == 0)
            {
                var score = new ContentChart
                {
                    Name = Enum.GetName(typeof(HouseItems), obj.GetComponent<ObjectProperties>().HouseItem),
                    Amount = 1,
                };

                scoreChart.Add(score);
            }
            else
            {
                var item = scoreChart.Find(i => i.Name == Enum.GetName(typeof(HouseItems), obj.GetComponent<ObjectProperties>().HouseItem));

                if (item == null)
                {
                    var score = new ContentChart
                    {
                        Name = Enum.GetName(typeof(HouseItems), obj.GetComponent<ObjectProperties>().HouseItem),
                        Amount = 1,
                    };

                    scoreChart.Add(score);
                }
                else
                    item.Amount++;
            }
        }

        foreach (var score in scoreChart)
        {
            if (score.Name == "Table" || score.Name == "Cabinet")
                FinalScoreText.text += "<color=#EC7063>" + " " + score.Amount + " " + score.Name + "</color>";
            else if (score.Name == "Oven" || score.Name == "Fridge" || score.Name == "Microwave" || score.Name == "TV")
                FinalScoreText.text += "<color=#AAB7B8>" + " " + score.Amount + " " + score.Name + "</color>";
            else if (score.Name == "Trashcan" || score.Name == "Sofa" || score.Name == "Lamp" || score.Name == "Toilet" || score.Name == "Bath" || score.Name == "Sink")
                FinalScoreText.text += "<color=#ECF0F1>" + " " + score.Amount + " " + score.Name + "</color>";
            else if (score.Name == "Bed")
                FinalScoreText.text += "<color=#AED6F1>" + " " + score.Amount + " " + score.Name + "</color>";
            else
                FinalScoreText.text += "<color=#D2B4DE>" + " " + score.Amount + " " + score.Name + "</color>";

            yield return wait;
        }

        FinalScoreText.text += "\nFor a total of " + contents.Count + " items";
    }
}

class ContentChart
{
    public string Name { get; set; }
    public int Amount { get; set; }
}
