using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public class Key
    {
        public int id;
    }
    
    public class Book
    {
        public int id; 
    }

    private float _distance = 6f;

    public List<Key> keys;
    public List<Book> books;

    [SerializeField] private int countKeys;
    [SerializeField] private int countBooks;

    private PhotonView _view;

    [Header("UI Count Key and Book")] 
    private TextMeshProUGUI _textCountKeys; 
    private TextMeshProUGUI _textCountBooks; 
    
    [Header("Sound VFX")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundPickKey;

    private void Start()
    {
        _view = GetComponent<PhotonView>();
        keys = new List<Key>();
        books = new List<Book>();

        _textCountKeys = GameObject.FindGameObjectWithTag("TextKey").GetComponent<TextMeshProUGUI>();
        _textCountBooks = GameObject.FindGameObjectWithTag("TextBook").GetComponent<TextMeshProUGUI>();
        
        _textCountKeys.text = countKeys.ToString();
        _textCountBooks.text = countBooks.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _view.IsMine)
        {
            _view.RPC(nameof(PickUpKey), RpcTarget.AllBuffered);
            _view.RPC(nameof(PickUpBook), RpcTarget.AllBuffered);
            _view.RPC(nameof(OpenDoor), RpcTarget.AllBuffered);
            _view.RPC(nameof(PutUpBook), RpcTarget.AllBuffered);
            _view.RPC(nameof(OpenChest), RpcTarget.AllBuffered);
            PickUpNote();
        }
    }

    private void CountKey()
    {
        countKeys = keys.Count;
        _textCountKeys.text = countKeys.ToString();
    }

    private void CountBook()
    {
        countBooks = books.Count;
        _textCountBooks.text = countBooks.ToString();
    }
    
    
    [PunRPC]
    private void PickUpKey()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, _distance))
        {
            if (hitInfo.transform.tag == "Key")
            {
                Key newKey = new Key();
                newKey.id = hitInfo.transform.GetComponent<PickUpItemID>().id;
                audioSource.clip = soundPickKey;
                audioSource.Play();
                keys.Add(newKey);
                CountKey();
                //Destroy(hitInfo.transform.gameObject);
                hitInfo.transform.GetComponent<PickUpItemID>().DestroyObj();
                //_view.RPC(nameof(DestroyObject), RpcTarget.OthersBuffered, hitInfo.transform.gameObject);
            }
        }
    }
    
    
    [PunRPC]
    private void OpenDoor()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, _distance))
        {
            if (hitInfo.transform.tag == "Door")
            {
                if (hitInfo.transform.GetComponent<Door>().isLock)
                {
                    int idDoor = hitInfo.transform.GetComponent<Door>().idDoor;
                    foreach (var key  in keys)
                    {
                        if (key.id == idDoor)
                        {
                            hitInfo.transform.GetComponent<Door>().isLock = false;
                            
                            keys.Remove(key);
                            CountKey();
                            break;
                        }
                    }
                    hitInfo.transform.GetComponent<Door>().inReach = true;
                }
                else
                {
                    hitInfo.transform.GetComponent<Door>().inReach = true;
                }
            }
        }
    }
    
    [PunRPC]
    private void OpenChest()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, _distance))
        {
            if (hitInfo.transform.tag == "Chest")
            {
                if (hitInfo.transform.GetComponent<Chest>().isLock)
                {
                    int idChest = hitInfo.transform.GetComponent<Chest>().idChest;
                    foreach (var key  in keys)
                    {
                        if (key.id == idChest)
                        {
                            hitInfo.transform.GetComponent<Chest>().isLock = false;
                            keys.Remove(key);
                            CountKey();
                            break;
                        }
                    }
                    hitInfo.transform.GetComponent<Chest>().inReach = true;
                }
                else
                {
                    hitInfo.transform.GetComponent<Door>().inReach = true;
                }
            }
        }
    }
    
    
    [PunRPC]
    private void PickUpBook()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, _distance))
        {
            if (hitInfo.transform.tag == "Book")
            {
                Book newBook = new Book();
                newBook.id = hitInfo.transform.GetComponent<PickUpItemID>().id; 
                books.Add(newBook);
                CountBook();
                //Destroy(hitInfo.transform.gameObject);
                hitInfo.transform.GetComponent<PickUpItemID>().DestroyObj();
            }
        }
    }

    [PunRPC]
    private void PutUpBook()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, _distance))
        {
            if (hitInfo.transform.CompareTag("Pray"))
            {
                Debug.Log("Tương tác Pray");
            
                Pray prayComponent = hitInfo.transform.GetComponent<Pray>();
                if (prayComponent != null)
                {
                    foreach (var book in books)
                    {
                        var matchingBook = prayComponent.books.FirstOrDefault(a => a.id == book.id);
                        if (matchingBook != null)
                        {
                            //prayComponent.countAcitveBook++;
                            //Pray.instance.AddBook();
                            //matchingBook.gameObject.SetActive(true);
                            prayComponent.SetActiveBook(matchingBook.id);
                            //prayComponent.AddBook();
                            books.Remove(book);
                            CountBook();
                            break;
                        } 
                    }
                }
            }
        }
    }

    private void PickUpNote()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, _distance))
        {
            if (hitInfo.transform.tag == "Note")
            {
                hitInfo.transform.GetComponent<NotePaper>().SetUIOn();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
