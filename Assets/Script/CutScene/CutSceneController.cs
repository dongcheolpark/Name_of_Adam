using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CutSceneController : MonoBehaviour
{
    [SerializeField] private CameraHandler _CameraHandler;
    public CameraHandler CameraHandler => _CameraHandler;

    CutSceneData CSData;
    GameObject Blur;

    public float ZoomTime = 1;
    public float CutSceneTime = 1;

    public float BlinkTime = 0.1f;
    public float ShakePower = 0.5f;
    public float ShakeTime = 0.1f;
    public float ShakeCount = 5;
    

    public void BattleCutScene(BattleUnit AttackUnit, List<BattleUnit> HitUnits)
    {
        if (HitUnits.Count == 0)
            return;

        BattleManager.Field.ClearAllColor();
        CSData = new CutSceneData(AttackUnit, HitUnits);
        
        CameraHandler.SetCutSceneCamera();
        UnitFlip();
        SetUnitRayer(5);
        
        StartCoroutine(ZoomIn());
    }

    private void UnitFlip()
    {
        if(CSData.AttackUnitDirection < 0)
        {
            CSData.AttackUnit.GetComponent<SpriteRenderer>().flipX = false;
            foreach(BattleUnit unit in CSData.HitUnits)
                unit.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (CSData.AttackUnitDirection > 0)
        {
            CSData.AttackUnit.GetComponent<SpriteRenderer>().flipX = true;
            foreach (BattleUnit unit in CSData.HitUnits)
                unit.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            bool attackDir = CSData.AttackUnit.GetComponent<SpriteRenderer>().flipX;
            foreach (BattleUnit unit in CSData.HitUnits)
                unit.GetComponent<SpriteRenderer>().flipX = !attackDir;
        }
    }

    public IEnumerator AfterAttack()
    {
        foreach (BattleUnit unit in CSData.HitUnits)
        {
            if (unit != null)
            {
                unit.GetComponent<Animator>().SetBool("isHit", true);
                StartCoroutine(UnitBlink(unit));
                StartCoroutine(UnitShake(unit));
            }
        }

        yield return StartCoroutine(AttackTilt());

        CSData.AttackUnit.GetComponent<Animator>().SetBool("isAttack", false);
        foreach (BattleUnit unit in CSData.HitUnits)
        {
            if (unit != null)
                unit.GetComponent<Animator>().SetBool("isHit", false);
        }

        yield return StartCoroutine(ZoomOut());

        SetUnitRayer(2);
        CameraHandler.SetMainCamera();
        BattleManager.Field.ClearAllColor();
        Destroy(Blur);
    }

    // 확대 후 컷씬
    public IEnumerator AttackTilt()
    {
        float time = 0;

        while (time <= CutSceneTime)
        {
            time += Time.deltaTime;
            float t = time / CutSceneTime;
            _CameraHandler.CameraLotate(new Vector3(0, 0, 0), new Vector3(0, 0, CSData.TiltPower), t);

            yield return null;
        }
    }

    // 컷씬 지점으로 이동
    IEnumerator ZoomIn()
    {
        Blur = GameManager.Resource.Instantiate("TestBlur");

        float time = 0;
        Vector3 moveVec = CSData.MovePosition;

        while (time <= ZoomTime)
        {
            time += Time.deltaTime;
            float t = time / ZoomTime;

            CSData.AttackUnit.transform.position = Vector3.Lerp(CSData.AttackPosition, CSData.MovePosition, t);
            _CameraHandler.ZoomIn(CSData, t);
            yield return null;
        }

        CSData.AttackUnit.GetComponent<Animator>().SetBool("isAttack", true);
    }

    // 원래 위치로 이동
    IEnumerator ZoomOut()
    {
        float time = 0;

        while (time <= ZoomTime)
        {
            time += Time.deltaTime;
            float t = time / ZoomTime;

            CSData.AttackUnit.transform.position = Vector3.Lerp(CSData.MovePosition, BattleManager.Field.GetTilePosition(CSData.AttackUnit.Location), t);
            _CameraHandler.CutSceneZoomOut(CSData, t);
            yield return null;
        }
    }

    private void SetUnitRayer(int rayer)
    {
        CSData.AttackUnit.GetComponent<SpriteRenderer>().sortingOrder = rayer;
        foreach (BattleUnit unit in CSData.HitUnits)
        {
            if(unit != null)
                unit.GetComponent<SpriteRenderer>().sortingOrder = rayer;
        }

    }

    private IEnumerator UnitBlink(BattleUnit unit)
    {
        SpriteRenderer sr = unit.GetComponent<SpriteRenderer>();

        sr.color = Color.black;

        yield return new WaitForSeconds(BlinkTime);

        sr.color = Color.white;
    }

    private IEnumerator UnitShake(BattleUnit unit)
    {
        Transform trans = unit.transform;
        Vector3 originPos = trans.position;
        
        float count = ShakeCount;
        float time = ShakeTime / ShakePower;
        bool min = true;
        trans.position += new Vector3(ShakePower / 2, 0, 0);

        while (count > 0)
        {
            yield return new WaitForSeconds(time / ShakeCount);

            if (trans == null)
                yield break;

            Vector3 vec = new Vector3(ShakePower / ShakeCount * count, 0, 0);
            if (min) vec *= -1;
            trans.position += vec;
            count--;
            min = !min;
        }

        trans.position = originPos;
    }
}