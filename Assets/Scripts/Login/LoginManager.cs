using UnityEngine;
using TMPro;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;

    public void OnLoginClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (username == "admin" && password == "1234")
        {
            Debug.Log("Login successful!");
            // Load next scene or enable gameplay UI
        }
        else
        {
            Debug.Log("Login failed. Invalid credentials.");
        }
    }
}