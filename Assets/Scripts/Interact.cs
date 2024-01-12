using System.Collections;
using System.Collections.Generic;
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
}
