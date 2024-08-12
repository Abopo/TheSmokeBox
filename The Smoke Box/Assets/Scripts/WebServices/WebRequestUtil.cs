using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public static class WebRequestUtil
{
    private static UnityWebRequest CreateRequest(string url, string method, string contentType = "application/json")
    {
        UnityWebRequest wr = new UnityWebRequest(url, method);

        wr.SetRequestHeader("Content-Type", contentType);
        wr.downloadHandler = new DownloadHandlerBuffer();

        return wr;
    }

    private static UnityWebRequest CreateAuthedRequest(string url, string method, string authToken)
    {
        UnityWebRequest wr = new UnityWebRequest(url, method);

        wr.SetRequestHeader("Authorization", authToken);
        wr.SetRequestHeader("Content-Type", "application/json");
        //wr.chunkedTransfer = false;
        wr.downloadHandler = new DownloadHandlerBuffer();

        return wr;
    }

    private static UnityWebRequest CreateAuthedRequest(string url, string method, string authToken, WWWForm data)
    {
        UnityWebRequest wr = CreateAuthedRequest(url, method, authToken);

        wr.uploadHandler = new UploadHandlerRaw(data.data);

        foreach (KeyValuePair<string, string> header in data.headers)
        {
            wr.SetRequestHeader(header.Key, header.Value);
        }

        return wr;
    }

    private static UnityWebRequest CreateAuthedRequest(string url, string method, string authToken, object data)
    {
        UnityWebRequest wr = CreateAuthedRequest(url, method, authToken);

        string json = (data.GetType() == typeof(string) ? (string)data : JsonConvert.SerializeObject(data)) ?? "";
        //string json = JsonConvert.SerializeObject(data);
        byte[] bytes = Encoding.UTF8.GetBytes(json);

        wr.uploadHandler = new UploadHandlerRaw(bytes);
        //wr.chunkedTransfer = false;

        return wr;
    }

    private static UnityWebRequest CreateRequest(string url, string method, object data, string contentType = "application/json")
    {
        UnityWebRequest wr = CreateRequest(url, method, contentType);

        string json = data.GetType() == typeof(string) ? (string)data : JsonConvert.SerializeObject(data);
        //string json = JsonConvert.SerializeObject(data);
        byte[] bytes = Encoding.UTF8.GetBytes(json);

        wr.uploadHandler = new UploadHandlerRaw(bytes);
        //wr.chunkedTransfer = false;

        return wr;
    }

    private static UnityWebRequest CreateRequest(string url, string method, WWWForm data)
    {
        UnityWebRequest wr = CreateRequest(url, method);

        wr.uploadHandler = new UploadHandlerRaw(data.data);

        foreach (KeyValuePair<string, string> header in data.headers)
        {
            wr.SetRequestHeader(header.Key, header.Value);
        }

        return wr;
    }

    /// <summary>
    /// Creates a HTTP GET web request.
    /// </summary>
    /// <param name="url">The URL to make the request to.</param>
    /// <returns>Returns a <see cref="UnityWebRequest"/> with a GET request</returns>
    public static UnityWebRequest CreateGetRequest(string url)
    {
        return CreateRequest(url, UnityWebRequest.kHttpVerbGET);
    }

    /// <summary>
    /// Create a HTTP POST web request.
    /// </summary>
    /// <param name="url">The url to make the request to.</param>
    /// <param name="data">Data to be serialized as JSON.</param>
    /// <returns>Returns a <see cref="UnityWebRequest"/> with a POST request</returns>
    public static UnityWebRequest CreatePostRequest(string url, object data)
    {
        return CreateRequest(url, UnityWebRequest.kHttpVerbPOST, data);

    }

    /// <summary>
    /// Create a HTTP PUT web request.
    /// </summary>
    /// <param name="url">The url to make the request to.</param>
    /// <param name="data">Data to be serialized as JSON.</param>
    /// <returns>Returns a <see cref="UnityWebRequest"/> with a PUT request</returns>
    public static UnityWebRequest CreatePutRequest(string url, object data)
    {
        return CreateRequest(url, UnityWebRequest.kHttpVerbPUT, data);
    }

    /// <summary>
    /// Used to actual call the GET request.
    /// </summary>
    /// <typeparam name="T">The type that the JSON will be deserialized as.</typeparam>
    /// <param name="url">The url to make the request to.</param>
    /// <param name="handleData">An action to actually handle the deserialized data.</param>
    /// <param name="handleError">An optionally action to handle any errors. By default errors are logged to the debug console.</param>
    /// <returns>Returns an <see cref="IEnumerator"/> which can be used as a coroutine.</returns>
    public static IEnumerator GetRequest<T>(string url, Action<T> handleData, Action<string> handleError = null) where T : class
    {
        using (UnityWebRequest wr = CreateGetRequest(url))
        {
            yield return wr.SendWebRequest();

            HandleResponse<T>(wr, handleData, handleError);
        }
    }

    /// <summary>
    /// Used to actual call the GET request.
    /// </summary>
    /// <typeparam name="T">The type that the JSON will be deserialized as.</typeparam>
    /// <param name="url">The url to make the request to.</param>
    /// <param name="authToken">The auth token needed for an authorized request.</param>
    /// <param name="handleData">An action to actually handle the deserialized data.</param>
    /// <param name="handleError">An optionally action to handle any errors. By default errors are logged to the debug console.</param>
    /// <returns>Returns an <see cref="IEnumerator"/> which can be used as a coroutine.</returns>
    public static IEnumerator GetAuthedRequest<T>(string url, string authToken, Action<T> handleData, Action<string> handleError = null) where T : class
    {
        using (UnityWebRequest wr = CreateAuthedRequest(url, UnityWebRequest.kHttpVerbGET, authToken))
        {
            yield return wr.SendWebRequest();

            HandleResponse<T>(wr, handleData, handleError);
        }
    }

    /// <summary>
    /// Used to actual call the Post request.
    /// </summary>
    /// <typeparam name="T">The type that the JSON will be deserialized as.</typeparam>
    /// <param name="url">The url to make the request to.</param>
    /// <param name="handleData">An action to actually handle the deserialized data.</param>
    /// <param name="handleError">An optionally action to handle any errors. By default errors are logged to the debug console.</param>
    /// <returns>Returns an <see cref="IEnumerator"/> which can be used as a coroutine.</returns>
    public static IEnumerator PostRequest<T>(string url, WWWForm data, Action<T> handleData, Action<string> handleError = null) where T : class
    {
        using (UnityWebRequest wr = CreateRequest(url, UnityWebRequest.kHttpVerbPOST, data))
        {
            yield return wr.SendWebRequest();

            HandleResponse<T>(wr, handleData, handleError);
        }
    }



    /// <summary>
    /// Used to actual call the POST request.
    /// </summary>
    /// <typeparam name="T">The type that the JSON will be deserialized as.</typeparam>
    /// <param name="url">The url to make the request to.</param>
    /// <param name="data">Data to be serialized as JSON.</param>
    /// <param name="handleData">An action to actually handle the deserialized data.</param>
    /// <param name="handleError">An optionally action to handle any errors. By default errors are logged to the debug console.</param>
    /// <returns>Returns an <see cref="IEnumerator"/> which can be used as a coroutine.</returns>
    public static IEnumerator PostRequest<T>(string url, object data, bool json, Action<T> handleData, Action<string> handleError = null) where T : class
    {
        using (UnityWebRequest wr = CreateRequest(url, UnityWebRequest.kHttpVerbPOST, data, json ? "application/json" : "application/x-www-form-urlencoded"))
        {
            yield return wr.SendWebRequest();

            HandleResponse<T>(wr, handleData, handleError);
        }
    }

    /// <summary>
    /// Used to actual call the POST request.
    /// </summary>
    /// <typeparam name="T">The type that the JSON will be deserialized as.</typeparam>
    /// <param name="url">The url to make the request to.</param>
    /// <param name="data">Data to be serialized as JSON.</param>
    /// <param name="handleData">An action to actually handle the deserialized data.</param>
    /// <param name="handleError">An optionally action to handle any errors. By default errors are logged to the debug console.</param>
    /// <returns>Returns an <see cref="IEnumerator"/> which can be used as a coroutine.</returns>
    public static IEnumerator PostRequest<T>(string url, Action<T> handleData, Action<string> handleError = null) where T : class
    {
        using (UnityWebRequest wr = CreateRequest(url, UnityWebRequest.kHttpVerbPOST, "application/x-www-form-urlencoded"))
        {
            yield return wr.SendWebRequest();

            HandleResponse<T>(wr, handleData, handleError);
        }
    }

    public static IEnumerator PostRequestForm<T>(string url, List<IMultipartFormSection> data, Action<T> handleData, Action<string> handleError = null) where T : class
    {
        using (UnityWebRequest wr = UnityWebRequest.Post(url, data))
        {
            yield return wr.SendWebRequest();

            HandleResponse<T>(wr, handleData, handleError);
        }
    }

    /// <summary>
    /// Used to actual call the POST request.
    /// </summary>
    /// <typeparam name="T">The type that the JSON will be deserialized as.</typeparam>
    /// <param name="url">The url to make the request to.</param>
    /// <param name="authToken">The auth token needed for an authorized request.</param>
    /// <param name="data">Data to be serialized as JSON.</param>
    /// <param name="handleData">An action to actually handle the deserialized data.</param>
    /// <param name="handleError">An optionally action to handle any errors. By default errors are logged to the debug console.</param>
    /// <returns>Returns an <see cref="IEnumerator"/> which can be used as a coroutine.</returns>
    public static IEnumerator PostAuthedRequest<T>(string url, string authToken, object data, Action<T> handleData, Action<string> handleError = null) where T : class
    {
        using (UnityWebRequest wr = CreateAuthedRequest(url, UnityWebRequest.kHttpVerbPOST, authToken, data))
        {
            yield return wr.SendWebRequest();

            HandleResponse<T>(wr, handleData, handleError);
        }
    }

    public static IEnumerator PostAuthedRequestForm<T>(string url, string authToken, WWWForm data, Action<T> handleData, Action<string> handleError = null) where T : class
    {
        using (UnityWebRequest wr = CreateAuthedRequest(url, UnityWebRequest.kHttpVerbPOST, authToken, data))
        {
            yield return wr.SendWebRequest();

            HandleResponse<T>(wr, handleData, handleError);
        }
    }

    public static IEnumerator PostAuthedRequestForm<T>(string url, string authToken, List<IMultipartFormSection> data, Action<T> handleData, Action<string> handleError = null) where T : class
    {
        using (UnityWebRequest wr = UnityWebRequest.Post(url, data))
        {
            wr.SetRequestHeader("Authorization", authToken);

            yield return wr.SendWebRequest();

            HandleResponse<T>(wr, handleData, handleError);
        }
    }

    /// <summary>
    /// Used to actual call the POST request.
    /// </summary>
    /// <typeparam name="T">The type that the JSON will be deserialized as.</typeparam>
    /// <param name="url">The url to make the request to.</param>
    /// <param name="data">Data to be serialized as JSON.</param>
    /// <param name="handleData">An action to actually handle the deserialized data.</param>
    /// <param name="handleError">An optionally action to handle any errors. By default errors are logged to the debug console.</param>
    /// <returns>Returns an <see cref="IEnumerator"/> which can be used as a coroutine.</returns>
    public static IEnumerator PutRequest<T>(string url, object data, Action<T> handleData, Action<string> handleError = null) where T : class
    {
        using (UnityWebRequest wr = CreatePutRequest(url, data))
        {
            yield return wr.SendWebRequest();

            HandleResponse<T>(wr, handleData, handleError);
        }
    }

    /// <summary>
    /// Used to actual call the DELETE request.
    /// </summary>
    /// <typeparam name="T">The type that the JSON will be deserialized as.</typeparam>
    /// <param name="url">The url to make the request to.</param>
    /// <param name="authToken">The auth token needed for an authorized request.</param>
    /// <param name="handleData">An action to actually handle the deserialized data.</param>
    /// <param name="handleError">An optionally action to handle any errors. By default errors are logged to the debug console.</param>
    /// <returns>Returns an <see cref="IEnumerator"/> which can be used as a coroutine.</returns>
    public static IEnumerator DeleteAuthedRequest<T>(string url, string authToken, Action<T> handleData, Action<string> handleError = null) where T : class
    {
        using (UnityWebRequest wr = CreateAuthedRequest(url, UnityWebRequest.kHttpVerbDELETE, authToken))
        {
            yield return wr.SendWebRequest();

            HandleResponse<T>(wr, handleData, handleError);
        }
    }

    /// <summary>
    /// Used to actual call the POST request.
    /// </summary>
    /// <typeparam name="T">The type that the JSON will be deserialized as.</typeparam>
    /// <param name="url">The url to make the request to.</param>
    /// <param name="authToken">The auth token needed for an authorized request.</param>
    /// <param name="data">Data to be serialized as JSON.</param>
    /// <param name="handleData">An action to actually handle the deserialized data.</param>
    /// <param name="handleError">An optionally action to handle any errors. By default errors are logged to the debug console.</param>
    /// <returns>Returns an <see cref="IEnumerator"/> which can be used as a coroutine.</returns>
    public static IEnumerator PutAuthedRequest<T>(string url, string authToken, object data, Action<T> handleData, Action<string> handleError = null) where T : class
    {
        using (UnityWebRequest wr = CreateAuthedRequest(url, UnityWebRequest.kHttpVerbPUT, authToken, data))
        {
            yield return wr.SendWebRequest();

            HandleResponse<T>(wr, handleData, handleError);
        }
    }

    private static void HandleResponse<T>(UnityWebRequest wr, Action<T> handleData, Action<string> handleError = null) where T : class
    {
        if (wr.result == UnityWebRequest.Result.ConnectionError)
        {
            if (handleError != null)
            {
                handleError(wr.error);
            }
            else
            {
                // LogUtil.LogMessage($"URL: {wr.url}\n\nResponse Code: {wr.responseCode}\n\nError Message: {wr.error}\n\nText: {wr.downloadHandler.text}", LogUtil.LogLevel.Error);

                if (handleData != null)
                {
                    handleData(null);
                }
            }
        }
        else if (wr.result == UnityWebRequest.Result.ProtocolError)
        {
            try
            {
                T obj = JsonConvert.DeserializeObject<T>(wr.downloadHandler.text);
                handleData(obj);
            }
            catch (Exception e)
            {
                handleError(wr.downloadHandler.text);
            }
        }
        else
        {
            if (handleData != null)
            {
                T obj;
                if (typeof(T) == typeof(string))
                {
                    obj = wr.downloadHandler.text as T;
                }
                else
                {
                    obj = JsonConvert.DeserializeObject<T>(wr.downloadHandler.text);
                }

                if (obj == null)
                {
                    //LogUtil.LogMessage("Error Deserializing json: " + wr.downloadHandler.text, LogUtil.LogLevel.Error);

                    if (handleError != null)
                    {
                        handleError("Error Deserializing json: " + wr.downloadHandler.text);
                    }
                }

                handleData(obj);
            }
            else
            {
                // LogUtil.LogMessage("handleData is null", LogUtil.LogLevel.Error);
            }
        }
    }
}
