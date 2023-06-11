using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValidatorRegister: MonoBehaviour
{
    private bool EmailIsValid = false;
    private bool PasswordIsValid = false;
    private bool NameIsValid = false;
    private bool ConfirmPasswordIsValid = false;
    public InputField EmailInputField;
    public InputField NameInputField;
    public InputField PasswordInputField;
    public InputField ConfirmPasswordInputField;
    public TextMeshProUGUI ErrorMessage;
    public void Start()
    {
        //Adds a listener to the main input field and invokes a method when the value changes.
        EmailInputField.onValueChanged.AddListener(ValidateEmail);
        PasswordInputField.onValueChanged.AddListener(ValidatePassword);
        NameInputField.onValueChanged.AddListener(ValidateName);
        ConfirmPasswordInputField.onValueChanged.AddListener(ValidateConfirmPassword);
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
    public void ValidatePassword(string value)
    {
        if (value != null)
        {
            if (value.Trim() == "")
            {
                PasswordIsValid = false;
                ErrorMessage.text = "Empty password";
                return;
            }
            if (value.Length < 7)
            {
                PasswordIsValid = false;
                ErrorMessage.text = "Password minimum 8 characters";
                return;
            }
            PasswordIsValid = true;
            ErrorMessage.text = "";
        }
    }
    public void ValidateName(string value)
    {
        if (value != null)
        {
            if (value.Trim() == "")
            {
                NameIsValid = false;
                ErrorMessage.text = "Empty name";
                return;
            }
            NameIsValid = true;
            ErrorMessage.text = "";
        }
    }
    public void ValidateConfirmPassword(string value)
    {
        if (value != null)
        {
            if (value.Trim() == "")
            {
                ConfirmPasswordIsValid = false;
                ErrorMessage.text = "Empty confirm password";
                return;
            }
            if (PasswordInputField.text != value.Trim())
            {
                ConfirmPasswordIsValid = false;
                ErrorMessage.text = "Password does not match";
                return;
            }
            ConfirmPasswordIsValid = true;
            ErrorMessage.text = "";
        }
    }
    public bool IsValid()
    {
        return PasswordIsValid && EmailIsValid && ConfirmPasswordIsValid && NameIsValid;
    }
    private void OnDisable()
    {
        EmailInputField.onValueChanged.RemoveListener(ValidateEmail);
        PasswordInputField.onValueChanged.RemoveListener(ValidatePassword);
        NameInputField.onValueChanged.RemoveListener(ValidateName);
        ConfirmPasswordInputField.onValueChanged.RemoveListener(ValidateConfirmPassword);
    }
}
