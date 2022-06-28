using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;


public class GetDataFromWeb : MonoBehaviour
{
    private const string twelveDataUrl = "https://api.twelvedata.com/time_series";
    private const string keys = "?symbol=AAPL,EUR/USD,ETH/BTC:Huobi,TRP:TSX&interval=1min&apikey=cb2ddf16991d4fd1bff11908d5514dd3&outputsize=5&start_date=2022-06-23%2016:40:00&format=JSON&end_date=2022-06-23%2016:40:00&timezone=Europe/Moscow";
    private string timezone;
    private string startDate;
    private string endDate;
    private string[] tickers = new string[5];


    public void GetData()
    {
        ComposeUrl();
        StartCoroutine(getRequest("https://api.twelvedata.com/time_series?symbol=AAPL,EUR/USD,ETH/BTC:Huobi,TRP:TSX&interval=1min&apikey=cb2ddf16991d4fd1bff11908d5514dd3&outputsize=5&start_date=2022-06-23%2016:40:00&format=JSON&end_date=2022-06-23%2016:40:00&timezone=Europe/Moscow"));
    }

    void ComposeUrl()
    {

    }
    IEnumerator getRequest(string url)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(url);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);

            JSONNode stockData = JSON.Parse(uwr.downloadHandler.text);

            Debug.Log(stockData["AAPL"]["meta"]["currency"]);
        }
    }
}
