using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Argos.Framework;

public class GamejoltUserManager : MonoBehaviour
{
    [Serializable]
    public class GameJoltUserFetchResponse
    {
        public GamejoltUserFecthResponseData response;
    }

    [Serializable]
    public class GamejoltUserFecthResponseData : GamejoltAPI.GamejoltBaseResponseData
    {
        public GamejoltUserData[] users;
    }

    [Serializable]
    public class GamejoltUserData
    {
        public int id;                          // The ID of the user.
        public string type;                     // The type of user.Can be User, Developer, Moderator, or Administrator.
        public string username;                 // The user's username. 
        public string avatar_url;               // The URL of the user's avatar. 
        public string signed_up;                // How long ago the user signed up. 
        public int signed_up_timestamp;         // The timestamp (in seconds) of when the user signed up.
        public string last_logged_in;           // How long ago the user was last logged in. Will be Online Now if the user is currently online. 
        public int last_logged_in_timestamp;    // The timestamp (in seconds) of when the user was last logged in. 
        public string status;                   // Active if the user is still a member of the site.Banned if they've been banned. 
        public string developer_name;           // The user's display name.
        public string developer_website;        // The user's website (or empty string if not specified) 
        public string developer_description;    // The user's profile markdown description. 
    }

    [SerializeField, Button("Login", GUIButtonSize.Normal, GUIButtonDisableEvents.EditorMode)]
    string _loginButton = "LoginUser";
    [SerializeField, Button("Logout", GUIButtonSize.Normal, GUIButtonDisableEvents.EditorMode)]
    string _logoutButton = "LogoutUser";

    public GamejoltAPIWebRequest.GamejoltAPIRequestTypes RequestType;

    [Header("Params")]
    public string gameId;
    public string userName;
    public string userToken;
    public string privateKey;

    [Header("Response")]
    public GamejoltAPI.GamejoltBaseResponse authResponse;
    public GameJoltUserFetchResponse fetchResponse;
    [TexturePreview]
    public Texture2D userAvatar;
    public GamejoltAPI.GamejoltBaseResponse loginResponse;
    [Space]
    public GamejoltAPI.GamejoltBaseResponse logoutResponse;

    void Awake()
    {
        GamejoltAPI.GameID = this.gameId;
        GamejoltAPI.PrivateKey = this.privateKey;
    }

    void OnApplicationQuit()
    {
        this.LogoutUser();
    }

    public void LoginUser()
    {
        StartCoroutine(this.LoginUserCoroutine());
    }

    public void LogoutUser()
    {
        this.CleanResponses();
        StartCoroutine(this.CloseUserSessionCoroutine());
    }

    void CleanResponses()
    {
        this.authResponse = new GamejoltAPI.GamejoltBaseResponse();
        this.fetchResponse = new GameJoltUserFetchResponse();
        this.loginResponse = new GamejoltAPI.GamejoltBaseResponse();
        this.logoutResponse = new GamejoltAPI.GamejoltBaseResponse();
        this.userAvatar = null;
    }

    #region Coroutines
    IEnumerator LoginUserCoroutine()
    {
        this.CleanResponses();

        yield return this.AuthUserCoroutine();

        if (this.authResponse.response.success)
        {
            yield return this.GetUserDataCoroutine();

            if (this.fetchResponse.response.success)
            {
                yield return this.GetAvatarTextureCoroutine(this.fetchResponse.response.users[0].avatar_url);
                yield return this.OpenUserSessionCoroutine();

                if (this.loginResponse.response.success)
                {
                    print("User login!");
                }
            }
        }
    }

    IEnumerator RequestCoroutine(string url, GamejoltAPIWebRequest.GamejoltAPIRequestOnIsDoneHandler onSuccess, GamejoltAPIWebRequest.GamejoltAPIRequestOnErrorHandler onError, params string[] paramList)
    {
        using (var request = new GamejoltAPIWebRequest(url))
        {
            for (int i = 0; i < paramList.Length; i += 2)
            {
                request.AddParam(paramList[i], paramList[i + 1]);
            }

            request.OnRequestIsDone += onSuccess;
            request.OnRequestError += onError;

            UnityWebRequestAsyncOperation asyncOperation = request.Send(this.RequestType);

            yield return new WaitUntil(() => asyncOperation.isDone);

            request.OnRequestIsDone -= onSuccess;
            request.OnRequestError -= onError;
        }
    }

    IEnumerator AuthUserCoroutine()
    {
        yield return this.RequestCoroutine($"{GamejoltAPI.GAMEJOLT_USERS_API_URL}auth/",
                                           (string response) =>
                                           {
                                               this.authResponse = JsonUtility.FromJson<GamejoltAPI.GamejoltBaseResponse>(response);
                                           },
                                           (GamejoltAPIWebRequest.GamejoltAPIRequestErrorTypes errorType, string errorMessage, int responseCode) =>
                                           {
                                               Debug.LogError($"AuthUserCoroutine: Request {errorType} (Response code {responseCode}) - {errorMessage}");
                                           },
                                           "username", this.userName,
                                           "user_token", this.userToken);
    }

    IEnumerator GetUserDataCoroutine()
    {
        yield return this.RequestCoroutine($"{GamejoltAPI.GAMEJOLT_USERS_API_URL}",
                                           (string response) =>
                                           {
                                               this.fetchResponse = JsonUtility.FromJson<GameJoltUserFetchResponse>(response);
                                           },
                                           (GamejoltAPIWebRequest.GamejoltAPIRequestErrorTypes errorType, string errorMessage, int responseCode) =>
                                           {
                                               Debug.LogError($"GetUserDataCoroutine: Request {errorType} (Response code {responseCode}) - {errorMessage}");
                                           },
                                           "username", this.userName);
    }

    IEnumerator GetAvatarTextureCoroutine(string url)
    {
        using (var request = UnityWebRequestTexture.GetTexture(this.fetchResponse.response.users[0].avatar_url))
        {
            var asyncOperation = request.SendWebRequest();

            yield return new WaitUntil(() => asyncOperation.isDone);

            if (request.isDone)
            {
                yield return new WaitUntil(() => request.downloadHandler.isDone);

                this.userAvatar = DownloadHandlerTexture.GetContent(request);
            }
            else if (request.isHttpError)
            {
                this.userAvatar = Texture2D.blackTexture;
                Debug.LogError($"GetAvatarTextureCoroutine: web request error: HTTP response code = {request.responseCode}");
            }
            else if (request.isNetworkError)
            {
                this.userAvatar = Texture2D.blackTexture;
                Debug.LogError($"GetAvatarTextureCoroutine: web request error: {request.error}");
            }
        }
    }

    IEnumerator OpenUserSessionCoroutine()
    {
        yield return this.RequestCoroutine($"{GamejoltAPI.GAMEJOLT_SESSIONS_API_URL}open/",
                                           (string response) =>
                                           {
                                               this.loginResponse = JsonUtility.FromJson<GamejoltAPI.GamejoltBaseResponse>(response);
                                           },
                                           (GamejoltAPIWebRequest.GamejoltAPIRequestErrorTypes errorType, string errorMessage, int responseCode) =>
                                           {
                                               Debug.LogError($"LoginUserCoroutine: Request {errorType} (Response code {responseCode}) - {errorMessage}");
                                           },
                                           "username", this.userName,
                                           "user_token", this.userToken);
    }

    IEnumerator CloseUserSessionCoroutine()
    {
        yield return this.RequestCoroutine($"{GamejoltAPI.GAMEJOLT_SESSIONS_API_URL}close/",
                                           (string response) =>
                                           {
                                               this.logoutResponse = JsonUtility.FromJson<GamejoltAPI.GamejoltBaseResponse>(response);
                                               print("User logout!");
                                           },
                                           (GamejoltAPIWebRequest.GamejoltAPIRequestErrorTypes errorType, string errorMessage, int responseCode) =>
                                           {
                                               Debug.LogError($"LogoutUserCoroutine: Request {errorType} (Response code {responseCode}) - {errorMessage}");
                                           },
                                           "username", this.userName,
                                           "user_token", this.userToken);
    }
    #endregion
}
