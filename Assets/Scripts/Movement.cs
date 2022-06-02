using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Movement : MonoBehaviour
{
    #region SerizializeFields
    [SerializeField] private WinCanvas winCanvas;
    [SerializeField] private Slider slider;
    [SerializeField] private float speed;
    [Header("Texts")] 
    [SerializeField] private TextMeshProUGUI stolen;
    [SerializeField] private TextMeshProUGUI loseEarned; 
    [SerializeField] private TextMeshProUGUI winEarned;
    [Header("Meshes")]
    [SerializeField] private List<MeshRenderer> characterParts = new List<MeshRenderer>();
    [SerializeField] private GameObject body;
    [SerializeField] private MeshRenderer boxMesh;
    [Header("AudioSources")] 
    [SerializeField] private AudioSource walk;
    [SerializeField] private AudioSource close;
    [Header("CanvasItems")]
    [SerializeField] private GameObject  lose, win, coinShown;
    #endregion
    #region PrivateFields
    private float _inputHorizontal, _inputVertical;
    private int _coin, _total;
    private Vector2 _turn;
    #endregion
    #region Proporties
    private BoxCollider MyBoxCollider => GetComponent<BoxCollider>();
    private Animator MyAnimator => GetComponent<Animator>();
    #endregion
    
    private CharacterController ThisCharacterController => GetComponent<CharacterController>();

    private void Start()
    {
        coinShown.SetActive(true);
        lose.SetActive(false);
        win.SetActive(false);
    }
    private void Update()
    {
        slider.value = _total;
        if (PoliceController.Stopper)
        {
            if (Input.GetMouseButton(0)) MoveMode();
            else StopMode();
        }
        else LoseGame();
    }
    private void MoveMode()
    {
        MoveJoystick();
        LookJoystick();
        if (MyBoxCollider.enabled) return;
        MyBoxCollider.enabled = true;
        boxMesh.enabled = false;
        characterParts.ForEach(i=> i.enabled = true);
        body.GetComponent<SkinnedMeshRenderer>().enabled = true;
        ChangeAudio(isWalk:true);
    }
    private void StopMode()
    {
        if (!MyBoxCollider.enabled) return;
        MyBoxCollider.enabled = false;
        boxMesh.enabled = true;
        characterParts.ForEach(i=> i.enabled = false);
        body.GetComponent<SkinnedMeshRenderer>().enabled = false;
        ChangeAudio(isWalk:false);
    }
    
    #region JoystickFunctions
    void MoveJoystick()
    {
        _inputHorizontal = SimpleInput.GetAxis("Horizontal");
        _inputVertical = SimpleInput.GetAxis("Vertical");
        if (Mathf.Abs(_inputHorizontal) < 0.2f && Mathf.Abs(_inputVertical) < 0.2f) return;
        Vector3 moveVector = new Vector3(_inputHorizontal, 0, _inputVertical) * speed;
        ThisCharacterController.Move(moveVector * Time.deltaTime);
    }
    private void LookJoystick()
    {
        if (Mathf.Abs(_inputHorizontal) < 0.2f && Mathf.Abs(_inputVertical) < 0.2f) return;
        Vector3 direction = new Vector3(_inputHorizontal, 0, _inputVertical).normalized;
        Quaternion newRotation = Quaternion.LookRotation(direction);
        transform.GetChild(0).rotation = Quaternion.Lerp(transform.GetChild(0).rotation, newRotation, 5 * Time.deltaTime);
    }
    #endregion
    
    private void ChangeAudio(bool isWalk, bool isOff = false)
    {
        walk.enabled = isWalk && !isOff;
        close.enabled = !isWalk && !isOff;
    }

    private void LoseGame()
    {
        boxMesh.enabled = false;
        characterParts.ForEach(i=> i.enabled = true);
        body.GetComponent<SkinnedMeshRenderer>().enabled = true;
        ChangeAudio(false, true);
        MyAnimator.enabled = false;
        lose.SetActive(true);
        coinShown.SetActive(false);
        loseEarned.text = "You earned $" + _total;
        slider.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("steal"))
        {
            _coin = Random.Range(5, 10);
            _total += _coin;
            stolen.text = "$" + _total;
            Destroy(other.gameObject);
        }
        if (other.CompareTag("finishPoint"))
        {
            if (_total < 77) return;
            Destroy(this.gameObject);
            coinShown.SetActive(false);
            slider.gameObject.SetActive(false);
            win.SetActive(true);
            winEarned.text = "You earned $" + _total;
            winCanvas.UpdateStars(_total);
        }
    }
}
