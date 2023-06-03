using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeFlag : IFlagModeStrategy
{
    public void Dash(FlagControl player)
    {
        Vector3 playerScale = player.transform.localScale;
        // 왼쪽 대쉬시 스케일을 통해 애니메이션 반전
        if (player.lastKeyPressed == KeyCode.A)
        {
            playerScale.x = -1;
            if (!player.transform.localScale.x.Equals(playerScale.x))
            {
                player.transform.localScale = playerScale;
                player.StartCoroutine(nameof(player.ResetScaleX_co));
            }
        }
        else
        {
            playerScale.x = 1;
            if (!player.transform.localScale.x.Equals(playerScale.x))
            {
                player.transform.localScale = playerScale;
            }
        }
        AudioManager.Instance.PlaySfx(Define.SFX.Dash);
        player.SetAnimaTrigger(player.hashDash);
        player.StopCoroutine(nameof(player.ReturnToNomalState_co));
        player.StartCoroutine(player.ReturnToNomalState_co(player.EnterDashAni_wait, player.ExitDashAni_wait));
    }
    public void Rotate(FlagControl player)
    {
    }

    #region 공격
    public void StrongAttack(FlagControl player)
    {
        AudioManager.Instance.PlaySfx(Define.SFX.FlagBarrier);
        player.SetAnimaTrigger(player.hashFlagStrongAttack);
    }
    public void WeakAttack(FlagControl player, bool isHorizontal)
    {
        AudioManager.Instance.PlaySfx(Define.SFX.FlagAttack);
        if (isHorizontal)
        {
            HorizontalWeakAttack(player);
        }
        else
        {
            VerticalWeakAttack(player);
        }
    }
    private void VerticalWeakAttack(FlagControl player)
    {
        player.SetAnimaTrigger(player.hashVerticalWeakAttack);
    }
    private void HorizontalWeakAttack(FlagControl player)
    {
        player.SetAnimaTrigger(player.hashHorizontalWeakAttack);
    }
    #endregion 공격
}
