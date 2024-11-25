using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
        });
    }

    public void SignInWithEmailPassword(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
                // Get the authenticated user from the AuthResult
                user = task.Result.User;
                Debug.Log("User signed in: " + user.UserId);
            }
            else
            {
                Debug.LogError("Sign-in failed: " + task.Exception);
            }
        });
    }
}

