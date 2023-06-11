using Firebase.Auth;
using Firebase;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using static UnityEditor.PlayerSettings;
using System.Net.Mail;

public class FirebaseResetPasswordManager: MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;
    public InputField email;
    public TextMeshProUGUI ErrorMessage;
    public TextMeshProUGUI SuccesMessage;
    private ValidatorResetPassword Validator;
    private void Start()
    {
        Validator = GetComponent<ValidatorResetPassword>();
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
    public void ResetPassword()
    {
        if (Validator.IsValid())
        {
            ErrorMessage.text = "";
            CheckAndFixDependencies();
            if (auth != null)
            {
                StartCoroutine(ResetPasswordAsync(email.text));
            }
        }
        else
        {
            ErrorMessage.text = "";
            ErrorMessage.text = "Invalid data";
        }
    }

    private IEnumerator ResetPasswordAsync(string email)
    {
        if (email.Trim() != "")
        {
            Debug.Log(auth);
            var resetTask = auth.SendPasswordResetEmailAsync(email);
            yield return new WaitUntil(() => resetTask.IsCompleted);
            if (resetTask.IsCanceled)
            {
                ErrorMessage.text = "";
                ErrorMessage.text = "SendPasswordResetEmailAsync was canceled.";
                Debug.LogError("SendPasswordResetEmailAsync was canceled.");
            }
            else
            {
                if (resetTask.IsFaulted)
                {
                    Debug.Log(resetTask.Exception.Message);
                    FirebaseException firebaseException = resetTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;
                    string faileMessage = "Reset Password Failed Because";
                    ErrorMessage.text = "";
                    ErrorMessage.text = faileMessage;
                    Debug.Log(faileMessage);

                }
                else
                {
                    ErrorMessage.text = "";
                    SuccesMessage.text = "";
                    SuccesMessage.text = "A password reset link was sent to your email";
                }
            }
        }
        else
        {
            Debug.Log("Erro enty");
        }
    }
}
