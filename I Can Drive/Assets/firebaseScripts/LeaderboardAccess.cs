using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions;

public class LeaderboardAccess : MonoBehaviour
{
    private DatabaseReference reference;

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    // Set the subscription status when player subscribes
    public void SetSubscriptionStatus(string userId, bool isSubscribed)
    {
        reference.Child("users").Child(userId).Child("isSubscribed").SetValueAsync(isSubscribed);
    }

    // Check if the player has access to the leaderboard
    public void CheckSubscriptionAccess(string userId)
    {
        reference.Child("users").Child(userId).Child("isSubscribed").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                bool hasAccess = task.Result.Value != null && (bool)task.Result.Value;
                if (hasAccess)
                {
                    // Show leaderboard
                    ShowLeaderboard();
                }
                else
                {
                    // Show prompt to subscribe
                    PromptSubscription();
                }
            }
        });
    }
}
