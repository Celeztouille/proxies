using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneTrigger : MonoBehaviour
{
    [SerializeField] private CanvasGroup sceneTransition;
    [SerializeField] private string nextScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[LoadSceneTrigger] Triggered");
            sceneTransition.DOFade(1f, 3f).OnComplete(() =>
            {
                SceneManager.LoadScene(nextScene);
            });
            gameObject.SetActive(false);
        }
    }
}
