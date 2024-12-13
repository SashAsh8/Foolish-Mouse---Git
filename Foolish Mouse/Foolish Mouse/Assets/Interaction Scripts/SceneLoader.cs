using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityFundamentals
{
    public class SceneLoader : MonoBehaviour
    {
    	public void LoadScene (int pSceneIndex)
    	{
    		SceneManager.LoadScene(pSceneIndex);
    	}

        public void LoadScene(string pSceneName)
        {
            SceneManager.LoadScene(pSceneName);
        }
    }
}
