using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HttpTest : MonoBehaviour
{
    [SerializeField]
    private int characterId = 1;

    [SerializeField]
    private string url = "https://rickandmortyapi.com/api/character/";

    [SerializeField] private RawImage rawImage;
    void Start()
    {
        url = "https://rickandmortyapi.com/api/character/" + characterId;
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.responseCode == 200)
            {
                // Show results as text
                string json = request.downloadHandler.text;
                Character character = JsonUtility.FromJson<Character>(json);
                Debug.Log(character.id + ":" + character.name + " is an " + character.species);
            }
            else
            {
                string mensaje = "status:" + request.responseCode;
                mensaje += "\nError: " + request.error;
                Debug.Log(mensaje);
            }
           
        }
    }
    
    IEnumerator GetImage(string imageUrl)
    {
        UnityWebRequest request = UnityWebRequest.Get(imageUrl);
        
        
    }
}



public class Character
{
    public int id;
    public string name;
    public string species;
    public string image;
}