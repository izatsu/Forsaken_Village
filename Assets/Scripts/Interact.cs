using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private void Start()
    {
        keys = new List<Key>();
        books = new List<Book>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickUpKey();
            PickUpBook();
            OpenDoor();
            PutUpBook();
            OpenChest();
        }
    }

    private void PickUpKey()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, _distance))
        {
            if (hitInfo.transform.tag == "Key")
            {
                Key newKey = new Key();
                newKey.id = hitInfo.transform.GetComponent<PickUpItemID>().id; 
                keys.Add(newKey);
                Destroy(hitInfo.transform.gameObject);
            }
        }
    }

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
                Destroy(hitInfo.transform.gameObject);
            }
        }
    }

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
                            prayComponent.countAcitveBook++;
                            matchingBook.gameObject.SetActive(true);
                            books.Remove(book);
                            break;
                        }
                    }
                }
            }
        }
    }
}
