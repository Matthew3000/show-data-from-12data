using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

public class GetDataFromWeb : MonoBehaviour
{
    private const string twelveDataUrl = "https://api.twelvedata.com/time_series?";
    //hardcoding all the attributes because not specified otherwise in a task
    private const string apiKey = "apikey=cb2ddf16991d4fd1bff11908d5514dd3";
    private string interval = "interval=1min";
    private string outputSize = "outputsize=2";
    private string format = "format=JSON";
    private string timezone = "timezone=Europe/Moscow";
    private string startDate;
    [SerializeField] private string[] tickers = new string[5];
    private string tickersTosend = "symbol=";
    private string request;
    [SerializeField] private GameObject CsvComposer;
    [SerializeField] private GameObject Logger;

    public void GetData()
    {
        //GetData method starts the whole process
        ComposeUrl();
        StartCoroutine(getRequest(request));
    }

    void ComposeUrl()
    {
        //ComposeUrl method composes the request url
        //clearing previous request. The request is always made as a whole from scratch in order
        //to make possible following modication such as customizable in runtime attributes
        request = "";

        //start date for the request
        DateTime requestTime = DateTime.Now;
        startDate = "start_date=" + requestTime.ToString("yyyy-MM-dd HH") + ":" + (Convert.ToInt32(requestTime.ToString("mm")) - 3) + ":00";

        //concat tickers. Again starting from scratch
        tickersTosend = "symbol=";
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

        if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.DataProcessingError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            string text = "Error While Sending: " + uwr.error;
            Logger.GetComponent<Logger>().LogText(text, true);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            string text = "Succesfully recieved data";
            Logger.GetComponent<Logger>().LogText(text, true);
            //Parsing response and writing to csv
            JSONNode stockData = JSON.Parse(uwr.downloadHandler.text);
            CsvComposer.GetComponent<CsvComposer>().WriteToCSV(stockData, tickers);
        }
    }
}
