using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GalleryController : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuButton;
    [SerializeField] private GameObject _backToShelfButton;

    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Animator _cameraAnimator;

    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;

    [SerializeField] private GameObject _downloadingPrefab;

    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private Transform _tablePosition;

    [SerializeField] private List<GalleryNamePlate> _namePlates;

    private List<DownloadedProject> _downloadProjects = new List<DownloadedProject>();

    private List<Project> _projectList;
    private int _currentPage = 0;
    private int _totalPages = 0;

    private DownloadedProject _onTableProject;

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

    public void BackOnePage()
    {
        if (_currentPage > 0)
        {
            _currentPage--;
            DownloadSubmissions();
        }
        SetLeftRightButtonInteractable();
    }

    public void ForwardOnePage()
    {
        if (_currentPage < _totalPages - 1)
        {
            _currentPage++;
            DownloadSubmissions();
        }
        SetLeftRightButtonInteractable();
    }

    public void ToTable(int shelfID)
    {
        _cameraAnimator.SetTrigger("MoveCamera");
        _onTableProject = _downloadProjects[shelfID];
        _onTableProject.MoveToPoint(_tablePosition.position);

        // UI stuff
        foreach (var namePlate in _namePlates)
        {
            namePlate.setInteractive(false);
        }

        _mainMenuButton.SetActive(false);
        _backToShelfButton.SetActive(true);

        _leftButton.gameObject.SetActive(false);
        _rightButton.gameObject.SetActive(false);
    }

    public void BackToShelf()
    {
        _cameraAnimator.SetTrigger("MoveCamera");
        _onTableProject.ReturnToPoint();
        
        foreach (var namePlate in _namePlates)
        {
            namePlate.setInteractive(true);
        }

        _mainMenuButton.SetActive(true);
        _backToShelfButton.SetActive(false);

        _leftButton.gameObject.SetActive(true);
        _rightButton.gameObject.SetActive(true);
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
        int startEntry = _currentPage * _spawnPoints.Count;

        foreach (var spawn in _spawnPoints)
        {
            spawn.gameObject.SetActive(false);
        }

        for (int i = 0; i + startEntry < _projectList.Count && i < _spawnPoints.Count; i++)
        {
            _spawnPoints[i].gameObject.SetActive(true);
            var project = _projectList[i + startEntry];
            _downloadProjects[i].Init(project.ProjectID);
            _namePlates[i].Init(project, i);
        }
    }

    private void OnProjectsRecieved(List<Project> projects)
    {
        if (projects.Count > 0)
        {
            ShuffleProjects(ref projects);
            _totalPages = (projects.Count / 8 + 1);
            _currentPage = 0;
        }

        _projectList = projects;

        DownloadSubmissions();
        SetLeftRightButtonInteractable();

        _loadingScreen.SetActive(false);
    }

    private void OnGetProjectsFailed(string message)
    {
        Debug.Log(message);
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

    private void SetLeftRightButtonInteractable()
    {
        _leftButton.interactable = true;
        _rightButton.interactable = true;

        if (_currentPage == 0)
        {
            _leftButton.interactable = false;
        }

        if (_currentPage == _totalPages - 1)
        {
            _rightButton.interactable = false;
        }
    }
}
