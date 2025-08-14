using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Input = UnityEngine.Windows.Input;


[System.Serializable]
public class PlayerData
{
    public int id;
    public string username;
    public bool state;
    public List<int> deck;
}
public class HttpTest : MonoBehaviour
{
    private string APIUrl = "https://my-json-server.typicode.com/SebastianEsco/Json/users/";
    private string RicKandMortyUrl = "https://rickandmortyapi.com/api/character/";

    [SerializeField]
    private RawImage[] rawImage;
    [SerializeField] private TextMeshProUGUI id;

    void Start()
    {
        
    }

    public void NuevoUsuario()
    {
        string usuario = id.text.Trim();

        if (int.TryParse(usuario, out int userId))
        {
            StartCoroutine(GetUser(userId)); 
        }
        else
        {
            Debug.LogError("El valor ingresado no es un ID válido: '" + usuario + "'");
        }
    }

    IEnumerator GetUser(int userId)
    {
        UnityWebRequest request = UnityWebRequest.Get(APIUrl+userId);
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
                Debug.Log(json);
                
                PlayerData player = JsonUtility.FromJson<PlayerData>(json);
                
                List<int> deckList = player.deck;
                
                int idUsuario = player.id;
                
                Debug.Log(idUsuario);

                int i = 0;
                foreach (int cardId in deckList)
                {
                    Debug.Log("Carta: " + cardId);
                    StartCoroutine(GetCharacter(cardId, i));
                    i++;
                }
                
                

            }
            else
            {
                string mensaje = "status:" + request.responseCode;
                mensaje += "\nError: " + request.error;
                Debug.Log(mensaje);
            }
           
        }
    }
    IEnumerator GetCharacter(int characterId, int imagePosition)
    {
        UnityWebRequest request = UnityWebRequest.Get(RicKandMortyUrl+characterId);
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
                StartCoroutine(GetImage(character.image, imagePosition));
            }
            else
            {
                string mensaje = "status:" + request.responseCode;
                mensaje += "\nError: " + request.error;
                Debug.Log(mensaje);
            }
           
        }
    }

    IEnumerator GetImage(string imageUrl, int imagePosition)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            if (request.responseCode == 200)
            {
                // Show results as texture
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Debug.Log("Image downloaded successfully");
                rawImage[imagePosition].texture = texture; 
            }
            else
            {
                string mensaje = "status:" + request.responseCode;
                mensaje += "\nError: " + request.error;
                Debug.Log(mensaje);
            }
        }
    }
    
}

public class Character
{
    public int id;
    public string name;
    public string species;
    public string image;
}

 