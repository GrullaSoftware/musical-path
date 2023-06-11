using Firebase.Auth;
using Firebase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirebaseRegisterManager : MonoBehaviour
{

    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;
    //Registration Variables
    public InputField username;
    public InputField email;
    public InputField password;
    public InputField confirmPassword;
    private ValidatorRegister Validator;
    public TextMeshProUGUI ErrorMessage;
    private void Start()
    {
        Validator = GetComponent<ValidatorRegister>();
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
    public void Register()
    {
        if (Validator.IsValid())
        {
            ErrorMessage.text = "";
            CheckAndFixDependencies();
            if (auth != null)
            {
                StartCoroutine(RegisterAsync(username.text, email.text, password.text, confirmPassword.text));
            }
        }
        else
        {
            ErrorMessage.text = "";
            ErrorMessage.text = "Invalid data";
        }
    }
    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        if (true)
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            yield return new WaitUntil(() => registerTask.IsCompleted);
            if (registerTask.IsCanceled)
            {
                ErrorMessage.text = "";
                ErrorMessage.text = "CreateUserWithEmailAndPasswordAsync was canceled.";
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
            }
            else
            {
                if (registerTask.IsFaulted)
                {

                    //Debug.LogError(registerTask.Exception);

                    FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;
                    string failedMessage = "Registration Failed Because ";

                    ErrorMessage.text = "";
                    ErrorMessage.text = GetError(authError);
                    Debug.Log(GetError(authError));
                    Debug.Log(firebaseException.Message);
                }
                else
                {
                    //Get the User after Registration Success
                    user = registerTask.Result.User;
                    UserProfile userProfile = new UserProfile { DisplayName = name };
                    var updateProfileTask = user.UpdateUserProfileAsync(userProfile);
                    yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                    if (registerTask.IsFaulted)
                    {
                        //Delete the user if user update failed
                        user.DeleteAsync();
                        Debug.LogError(updateProfileTask.Exception);
                        FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError authError = (AuthError)firebaseException.ErrorCode;

                        string failedMessage = "Profile update Failed! Because ";
                        ErrorMessage.text = "";
                        ErrorMessage.text = GetError(authError);
                        Debug.Log(GetError(authError));
                    }
                    else
                    {
                        Debug.Log("Registration Sucessful Welcome " + user.DisplayName);
                    }

                }
            }
        }
    }
    private string GetError(AuthError authError)
    {
        string failedMessage = "";
        switch (authError)
        {
            case AuthError.InvalidEmail:
                failedMessage += "Email is invalid";
                break;
            case AuthError.WrongPassword:
                failedMessage += "Wrong password";
                break;
            case AuthError.MissingEmail:
                failedMessage += "Email is missing";
                break;
            case AuthError.MissingPassword:
                failedMessage += "Password is missing";
                break;
            default:
                failedMessage += "Registration Failed";
                break;
        }
        return failedMessage;
    }
}
