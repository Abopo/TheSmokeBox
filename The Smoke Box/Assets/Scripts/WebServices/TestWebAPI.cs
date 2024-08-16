using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TestWebAPI : MonoBehaviour
{
    [Header("API Selection Buttons")]
    public UnityEngine.UI.Button getProjectsButton;
    public UnityEngine.UI.Button getProjectButton;
    public UnityEngine.UI.Button postProjectButton;

    [Header("Get All Projects Input")]
    public GameObject getAllProjectsPanel;
    public UnityEngine.UI.Button submitGetAllProjects;

    [Header("Get Project Input")]
    public GameObject getProjectPanel;
    public TMP_InputField getProjectIDField;
    public UnityEngine.UI.Button submitGetProject;

    [Header("Post Project Input")]
    public GameObject postProjectPanel;
    public TMP_InputField postProjectOwnerNameField;
    public TMP_InputField postProjectProjectNameField;
    public TMP_InputField postProjectFilePathField;
    public UnityEngine.UI.Button submitPostProject;

    [Header("Results")]
    public TMPro.TMP_Text results;

    public void Start()
    {
        PlayerPrefs.SetString("apiPath", "https://bughunigamejam2024.azurewebsites.net");

        getProjectsButton.onClick.AddListener(ClickGetProjectsButton);
        getProjectButton.onClick.AddListener(ClickGetProjectButton);
        postProjectButton.onClick.AddListener(ClickPostProjectButton);

        submitGetAllProjects.onClick.AddListener(ClickSubmitGetAllProjects);
        submitGetProject.onClick.AddListener(ClickSubmitGetProject);
        submitPostProject.onClick.AddListener(ClickSubmitPostProject);
    }

    private void ClickGetProjectsButton()
    {
        // hide other panels
        getProjectPanel.SetActive(false);
        postProjectPanel.SetActive(false);
        // show ours
        getAllProjectsPanel.SetActive(true);
    }

    private void ClickGetProjectButton()
    {
        // hide other panels
        getAllProjectsPanel.SetActive(false);
        postProjectPanel.SetActive(false);
        // show ours
        getProjectPanel.SetActive(true);
    }

    private void ClickPostProjectButton()
    {
        // hide other panels
        getAllProjectsPanel.SetActive(false);
        getProjectPanel.SetActive(false);
        // show ours
        postProjectPanel.SetActive(true);
    }

    private void ClickSubmitGetAllProjects()
    {
        WebServiceProjectManager.Instance.GetProjects(HandleGetAllProjects, HandleFailure);
    }

    private void ClickSubmitGetProject()
    {
        int id = Int32.Parse(getProjectIDField.text);
        WebServiceProjectManager.Instance.GetProjectFile(id, HandleGetProjectSuccess, HandleFailure);
    }

    private void ClickSubmitPostProject()
    {
        WebServiceProjectManager.Instance.UploadProjectFile(postProjectOwnerNameField.text, postProjectProjectNameField.text, postProjectFilePathField.text, HandlePostProjectSuccess, HandleFailure);
    }

    private void HandleGetAllProjects(List<Project> projects)
    {
        results.text = JsonConvert.SerializeObject(projects);
    }

    private void HandleGetProjectSuccess(SubmissionData response)
    {
        results.text = response.ToString();
    }

    private void HandlePostProjectSuccess(Project project)
    {
        results.text = project.ToString();
    }

    private void HandleFailure(string message)
    {
        Debug.Log(message);
    }
}
