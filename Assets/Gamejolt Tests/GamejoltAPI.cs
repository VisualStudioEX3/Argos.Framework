using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Argos.Framework;
using Argos.Framework.Helpers;

public static class GamejoltAPI
{
    #region Constants
    const string GAMEJOLT_API_VERSION = "v1_2/";
    const string GAMEJOLT_API_URL = "https://api.gamejolt.com/api/game/" + GamejoltAPI.GAMEJOLT_API_VERSION;

    public const string GAMEJOLT_USERS_API_URL = GamejoltAPI.GAMEJOLT_API_URL + "users/";
    public const string GAMEJOLT_SESSIONS_API_URL = GamejoltAPI.GAMEJOLT_API_URL + "sessions/";
    #endregion

    #region Structs
    [Serializable]
    public class GamejoltBaseResponseData
    {
        public bool success;
        public string message;
    }

    [Serializable]
    public class GamejoltBaseResponse
    {
        public GamejoltBaseResponseData response;
    }
    #endregion

    #region Properties
    public static string GameID { get; set; }
    public static string PrivateKey { get; set; }
    #endregion
}

public sealed class GamejoltAPIWebRequest : IDisposable
{
    #region Enums
    public enum GamejoltAPIRequestTypes
    {
        GET,
        POST
    }

    public enum GamejoltAPIRequestErrorTypes
    {
        NetworkError,
        HttpError
    } 
    #endregion

    #region Internal vars
    string _apiURL;
    UnityWebRequest _request;
    #endregion

    #region Properties
    public Dictionary<string, string> Params { get; private set; }
    public string Response { get; private set; }
    public string URL { get; private set; }
    public bool IsDone { get; private set; }
    public bool IsNetworkError { get; private set; }
    public string Error { get; private set; }
    public bool IsHTTPError { get; private set; }
    public int ResponseCode { get; private set; }
    #endregion

    #region Delegates
    public delegate void GamejoltAPIRequestOnIsDoneHandler(string response);
    public delegate void GamejoltAPIRequestOnErrorHandler(GamejoltAPIRequestErrorTypes errorType, string errorMessage, int responseCode);
    #endregion

    #region Events
    public GamejoltAPIRequestOnIsDoneHandler OnRequestIsDone;
    public GamejoltAPIRequestOnErrorHandler OnRequestError;
    public Action OnRequestAbort;
    #endregion

    #region Constructors & Destructors
    public GamejoltAPIWebRequest()
    {
        this.Params = new Dictionary<string, string>();
    }

    public GamejoltAPIWebRequest(string apiUrl) : this()
    {
        this._apiURL = apiUrl;
    }

    public GamejoltAPIWebRequest(string apiURL, Dictionary<string, string> paramList)
    {
        this._apiURL = apiURL;
        this.Params = new Dictionary<string, string>(paramList);
    }

    public void Dispose()
    {
        this._request?.Dispose();
        this.Params.Clear();
        this.Params = null;
        this._apiURL = this.Response = this.Error = null;
    }
    #endregion

    #region Events
    void OnCompleted(AsyncOperation asyncOp)
    {
        this.IsDone = this._request.isDone;
        this.IsNetworkError = this._request.isNetworkError;
        this.Error = this._request.error;
        this.IsHTTPError = this._request.isHttpError;
        this.ResponseCode = (int)this._request.responseCode;
        this.Response = this._request.downloadHandler.text;

        if (this.IsDone)
        {
            this.OnRequestIsDone?.Invoke(this.Response);
        }
        else
        {
            this.OnRequestError?.Invoke(this.IsNetworkError ? GamejoltAPIRequestErrorTypes.NetworkError : GamejoltAPIRequestErrorTypes.HttpError, this.Error, this.ResponseCode);
        }
    } 
    #endregion

    #region Methods & Functions
    public void AddParam(string name, string value)
    {
        if (!this.Params.ContainsKey(name))
        {
            this.Params.Add(name, value);
        }
        else
        {
            throw new ArgumentException($"A parameter named \"{name}\" already exists in the request.");
        }
    }

    public UnityWebRequestAsyncOperation Send(GamejoltAPIRequestTypes requestType = GamejoltAPIRequestTypes.GET)
    {
        if (requestType == GamejoltAPIRequestTypes.GET)
        {
            this.URL = this.ComposeGETURL();
            this._request = UnityWebRequest.Get(this.URL);
        }
        else
        {
            this.URL = this.ComposePOSTURL();
            this._request = UnityWebRequest.Post(this.URL, this.Params);
        }

        UnityWebRequestAsyncOperation asyncOp = this._request.SendWebRequest();

        asyncOp.completed -= this.OnCompleted;
        asyncOp.completed += this.OnCompleted;

        return asyncOp;
    }

    public void Abort()
    {
        this._request?.Abort();
        this.OnRequestAbort?.Invoke();
    }

    string ComposeGETURL()
    {
        var url = new StringBuilder();

        url.Append($"{this._apiURL}?game_id={GamejoltAPI.GameID}");

        foreach (var param in this.Params)
        {
            url.Append($"&{param.Key}={param.Value}");
        }

        string urlPrivateKey = $"{url.ToString()}{GamejoltAPI.PrivateKey}";
        url.Append($"&signature={ArgosHelper.CalculateMD5Hash(urlPrivateKey)}");

        return url.ToString(); ;
    }

    string ComposePOSTURL()
    {
        string url = $"{this._apiURL}?game_id={GamejoltAPI.GameID}";
        var paramList = new StringBuilder();
        Dictionary<string, string> sortedParams = this.Params.OrderBy(e => e.Key).ToDictionary(e => e.Key, e => e.Value);

        foreach (var param in sortedParams)
        {
            paramList.Append($"{param.Key}{param.Value}");
        }

        paramList.Append(GamejoltAPI.PrivateKey);

        string urlPrivateKey = $"{url}{paramList.ToString()}";
        string signature = $"&signature={ArgosHelper.CalculateMD5Hash(urlPrivateKey)}";

        return $"{url}{signature}";
    }
    #endregion
}
