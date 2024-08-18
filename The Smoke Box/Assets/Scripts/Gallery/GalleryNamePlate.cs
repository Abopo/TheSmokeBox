using TMPro;
using UnityEngine;

public class GalleryNamePlate : MonoBehaviour
{
    [SerializeField] private GalleryController _galleryController;

    [SerializeField] private bool _interactive = true;

    private int _shelfId = -1;
    private TMP_Text _textLabel;

    private void Awake()
    {
        _textLabel = GetComponent<TMP_Text>();
    }

    public void Init(Project project, int id)
    {
        _shelfId = id;
        _textLabel.text = project.Name + "/nBy: " + project.OwnerName;
    }

    public void setInteractive(bool interactive)
    {
        _interactive = interactive;
    }

    private void OnMouseDown()
    {
        if (_interactive)
        {
            _galleryController.ToTable(_shelfId);
        }
    }
}
