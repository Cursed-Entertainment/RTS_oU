using System.Collections;
using UnityEngine.Networking;
using UnityEngine;

[DisallowMultipleComponent]
public class Server : MonoBehaviour
{
    string ServerURL = "http://<host_ip>:5000";

    void Start()
    {
        StartCoroutine(SendMessageToServer("Hello from Unity!"));
    }

    IEnumerator SendMessageToServer(string message)
    {
        WWWForm form = new WWWForm();
        form.AddField("message", message);

        using (UnityWebRequest www = UnityWebRequest.Post(ServerURL + "/send_message", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                Debug.Log("Message sent successfully!");
            }
        }
    }
}
