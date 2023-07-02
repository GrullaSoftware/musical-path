using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class FirebaseLoginManager : MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    public InputField email;
    public InputField password;
    public TextMeshProUGUI ErrorMessage;
    private ValidatorLogin Validator;
    private void Start()
    {
        //Validator = GetComponent<ValidatorLogin>();
    }
    private void OnEnable()
    {
        Validator = GetComponent<ValidatorLogin>();
    }
    private void Awake()
    {

        CheckAndFixDependencies();
    }

    private void CheckAndFixDependencies()
    {
        if (auth == null)
        {
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                    InitializeFirebase();
                }
                else
                {
                    Debug.Log("Could not resolve all firebase depedencies " + dependencyStatus);
                }
            });
        }
    }
    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }
    private void AuthStateChanged(object sender, System.EventArgs eventArg)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed In " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed In " + user.UserId);
            }
        }
    }
    public void Login()
    {
        if (Validator.IsValid())
        {
            ErrorMessage.text = "";
            CheckAndFixDependencies();
            if (auth != null)
            {
                StartCoroutine(LoginAsync(email.text, password.text));
            }
        }
        else 
        {
            ErrorMessage.text = "";
            ErrorMessage.text = "Invalid data";
        } 
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        if (email.Trim() != "" && password.Trim() != "")
        {
            Debug.Log(auth);
            var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => loginTask.IsCompleted);
            if (loginTask.IsCanceled)
            {
                ErrorMessage.text = "";
                ErrorMessage.text = "SignInWithEmailAndPasswordAsync was canceled.";
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
            }
            else
            {
                if (loginTask.IsFaulted)
                {
                    Debug.Log(loginTask.Exception.Message);
                    FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;
                    string faileMessage = "Login Failed Because";
                    Debug.Log(faileMessage);

                }
                else
                {
                    user = loginTask.Result.User;
                    Debug.LogFormat("{0} You Are Successfully logged In", user.DisplayName);
                    Debug.LogFormat("{0} USER ID", user.UserId);
                    SceneManager.LoadSceneAsync(1);
                }
            }
        }
        else
        {
            Debug.Log("Erro enty");
        }

    }
}
