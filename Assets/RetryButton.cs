using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine; 




public class RetryButton : MonoBehaviour
{
    public void OnRetryButtonClicked()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
