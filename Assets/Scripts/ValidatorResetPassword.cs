using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValidatorResetPassword: MonoBehaviour
{
    private bool EmailIsValid = false;
    public InputField EmailInputField;
    public TextMeshProUGUI ErrorMessage;
    public void Start()
    {
        //Adds a listener to the main input field and invokes a method when the value changes.
        EmailInputField.onValueChanged.AddListener(ValidateEmail);
    }

    public void ValidateEmail(string value)
    {
        if (value != null)
        {
            if (value.Trim() == "")
            {
                EmailIsValid = false;
                ErrorMessage.text = "Empty email";
                return;
            }
            Regex regexValidateEmail = new Regex("^\\S+@\\S+\\.\\S+$");
            if (!regexValidateEmail.IsMatch(value))
            {
                EmailIsValid = false;
                ErrorMessage.text = "Invalid format email";
                return;
            }
            EmailIsValid = true;
            ErrorMessage.text = "";
        }

    }
    public bool IsValid()
    {
        return EmailIsValid;
    }

    private void OnDisable()
    {
        EmailInputField.onValueChanged.RemoveListener(ValidateEmail);
    }
}
