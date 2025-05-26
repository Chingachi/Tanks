using Common.SceneControllerComponents;
using UnityEngine;
using Zenject;
public class LaunchObject : MonoBehaviour
{

  private SceneController _controller;

  private void Start()
  {
    _controller?.GoToGame();
  }

  [Inject]
  public void Construct (SceneController controller)
  {
    _controller = controller;
  }
}