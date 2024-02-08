using System.Collections;
using UnityEngine;
using Newtonsoft.Json;

public class AvatarAnimation : MonoBehaviour
{
    [SerializeField] private TextAsset _animationDataJsonFile;
    private CharacterAvatar _characterAvatar;
    private AnimationData _animationData;

    // Start is called before the first frame update
    private void Start()
    {
        _characterAvatar = GetComponent<CharacterAvatar>();
        ParseAnimationData();
        StartCoroutine(StartAnimation());
    }

    private void ParseAnimationData()
    {
        _animationData = JsonConvert.DeserializeObject<AnimationData>(_animationDataJsonFile.text);
    }

    private IEnumerator StartAnimation()
    {
        for (int i = 0; i < _animationData.data.Count; i++)
        {
            _characterAvatar.Load(_animationData.data[i]);
            yield return new WaitForEndOfFrame();
        }

        StartCoroutine(StartAnimation());
    }
}
