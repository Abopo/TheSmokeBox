using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GalleryController : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreen;

    [SerializeField] private GameObject _downloadingPrefab;

    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private List<TMPro.TMP_Text> _labels;

    private List<DownloadedProject> _downloadProjects = new List<DownloadedProject>();

    private List<Project> _projectList;
    private const int _entriesPerPage = 8;
    private int _currentPage = 0;
    private int _totalPages = 0;

    // Start is called before the first frame update
    void Start()
    {
        InitializeGallery();
        SpawnSubmissions();
    }

    public void BackToTitleScreen()
    {
        GameManager.Instance.LoadScene("TitleScreen");
    }

    private void InitializeGallery()
    {
        _loadingScreen.SetActive(true);
        WebServiceProjectManager.Instance.GetProjects(OnProjectsRecieved, OnGetProjectsFailed);
    }

    private void SpawnSubmissions()
    {
        foreach (var spawn in _spawnPoints)
        {
            var obj = Instantiate(_downloadingPrefab, spawn);
            _downloadProjects.Add(obj.GetComponent<DownloadedProject>());
        }
    }

    private void DownloadSubmissions()
    {
        int startEntry = _currentPage * _entriesPerPage;

        foreach (var spawn in _spawnPoints)
        {
            spawn.gameObject.SetActive(false);
        }

        for (int i = 0; i + startEntry < _projectList.Count && i < _entriesPerPage; i++)
        {
            _spawnPoints[i].gameObject.SetActive(true);
            var project = _projectList[i + startEntry];
            _downloadProjects[i].Init(project.ProjectID);
            _labels[i].text = project.Name + "\nBy: " + project.OwnerName;
        }
    }

    private void OnProjectsRecieved(List<Project> projects)
    {
        if (projects.Count > 0)
        {
            ShuffleProjects(ref projects);
            int pages = projects.Count / 8;
            _currentPage = 0;
        }

        _projectList = projects;

        DownloadSubmissions();

        _loadingScreen.SetActive(false);
    }

    private void OnGetProjectsFailed(string message)
    {

    }

    private void ShuffleProjects(ref List<Project> projects)
    {
        int n = projects.Count;
        while (n > 1)
        {
            n--;
            int k = RandomNumberGenerator.GetInt32(0, n + 1);
            Project value = projects[k];
            projects[k] = projects[n];
            projects[n] = value;
        }
    }
}
