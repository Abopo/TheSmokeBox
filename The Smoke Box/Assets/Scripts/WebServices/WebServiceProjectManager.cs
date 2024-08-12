using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class WebServiceProjectManager : MonoBehaviour
{
    public static WebServiceProjectManager Instance;

    private readonly string _projectEndpoint = "/api/Projects";

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    public void UploadProjectFile(string ownerName, string name, string path, Action<Project> callback, Action<string> handleFailure)
    {
        if (!File.Exists(path))
        {
            handleFailure.Invoke("File path error.");
            return;
        }

        byte[] data = new byte[0];
        try
        {
            data = File.ReadAllBytes(Path.GetFullPath(path));
        }
        catch (Exception e)
        {
            handleFailure.Invoke(e.ToString());
            return;
        }

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("file", data, Path.GetFileName(path), "file"));

        string url = PlayerPrefs.GetString("apiPath") + _projectEndpoint + "?userName=" + ownerName + "&projectName=" + name;

        try
        {
            StartCoroutine(WebRequestUtil.PostRequestForm<Project>(url, formData, callback, handleFailure));
        }
        catch (Exception e)
        {
            handleFailure.Invoke(e.ToString());
        }
    }

    public void GetProjects(Action<List<Project>> handleProjectsResponse, Action<string> handleFailure)
    {
        string url = PlayerPrefs.GetString("apiPath") + _projectEndpoint;

        StartCoroutine(WebRequestUtil.GetRequest<List<Project>>(url, handleProjectsResponse, handleFailure));
    }

    // here im giving back a string, but you could probably change that
    // out for whatever data your serializing and get the class back
    public void GetProjectFile(int id, Action<string> handleSuccess, Action<string> handleFailure)
    {
        string url = PlayerPrefs.GetString("apiPath") + _projectEndpoint + "/" + id;

        StartCoroutine(WebRequestUtil.GetRequest<string>(url, handleSuccess, handleFailure));
    }
}
