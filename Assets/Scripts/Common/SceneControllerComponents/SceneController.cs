using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
namespace Common.SceneControllerComponents
{
  public class SceneController
  {
    public async UniTask<AsyncUnit> ChangeScene (string sceneName)
    {
      await SceneManager.LoadSceneAsync(sceneName);

      return AsyncUnit.Default;
    }

    public async UniTask<AsyncUnit> GoToGame()
    {
      await ChangeScene("GameScene");

      return AsyncUnit.Default;
    }
  }
}