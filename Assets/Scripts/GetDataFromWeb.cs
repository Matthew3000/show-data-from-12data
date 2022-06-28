using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;


public class GetDataFromWeb : MonoBehaviour
{
    private const string twelveDataUrl = "https://api.twelvedata.com/time_series?";
    //hardcoding all attributes because not specified otherwise in a task
    private const string apiKey = "apikey=cb2ddf16991d4fd1bff11908d5514dd3";
    private string interval = "interval=1min";
    private string outputSize = "outputsize=5";
    private string format = "format=JSON";
    private string timezone = "timezone=Europe/Moscow";
    private string startDate;
    [SerializeField] private string[] tickers = new string[5];
    private string tickersTosend = "symbol=";
    private string request;
    [SerializeField] private GameObject CsvComposer;


    public void GetData()
    {
        ComposeUrl();
        StartCoroutine(getRequest(request));
    }

    void ComposeUrl()
    {
        //start date for request
        DateTime requestTime = DateTime.Now;
        startDate = requestTime.ToString("MM-dd-yyyy") + "%" + requestTime.ToString("HH") + ":" + (Convert.ToInt32(requestTime.ToString("mm")) - 1) + ":00";

        //concat tickers
        for (int i = 0; i < tickers.Length; i++)
        {
            if (i != 0 && !string.IsNullOrWhiteSpace(tickers[i]))
                tickersTosend += ",";
            if (!string.IsNullOrWhiteSpace(tickers[i]))
                tickersTosend += tickers[i];
        }

        request = twelveDataUrl + tickersTosend + "&" + apiKey + "&" + interval + "&" + outputSize + "&" + timezone + "&" + startDate + "&" + tickersTosend + "&" + format;
        Debug.Log(request);
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
            if (!stockData["code"]) {
                CsvComposer.GetComponent<CsvComposer>().WriteToCSV(stockData, tickers);
            }
        }
    }
}
