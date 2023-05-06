using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    private CanvasGroup cg;
    public float fadeTime = 1f; // ���̵� Ÿ�� 
    float accumTime = 0f;
    private Coroutine fadeCor;
    [SerializeField] bool isButton;
    [SerializeField] string scenename = "none";

    private void Awake()
    {
        //������ Alpha ���� ����
        cg = GetComponent<CanvasGroup>(); // ĵ���� �׷�
        fadeCor = null;
        //StartFadeIn();
    }

    public void StartFadeIn() // ȣ�� �Լ� Fade In�� ����
    {
        if (fadeCor != null)
            StopCoroutine(fadeCor);

        fadeCor = StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn() // �ڷ�ƾ�� ���� ���̵� �� �ð� ����
    {
        while (accumTime < fadeTime)
        {
            cg.alpha = Mathf.Lerp(0f, 1f, accumTime / fadeTime);
            accumTime += Time.deltaTime;
            yield return null;
        }
        cg.alpha = 1f;
        accumTime = fadeTime;
    }

    public void StartFadeOut() // ȣ�� �Լ� Fadeout�� ����
    {
        if (fadeCor != null)
            StopCoroutine(fadeCor);

        fadeCor = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        while (0 < accumTime)
        {
            cg.alpha = Mathf.Lerp(0, 1f, accumTime / fadeTime);
            accumTime -= Time.deltaTime;
            yield return null;
        }
        cg.alpha = 0f;
        accumTime = 0;

        //SceneCheck();
    }

    void SceneCheck()
    {
        if(scenename != "none")
        {
            SceneChanger.SceneChange(scenename);
        }
    }
}