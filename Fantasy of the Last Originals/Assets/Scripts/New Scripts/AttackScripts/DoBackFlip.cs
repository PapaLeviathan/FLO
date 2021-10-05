public class DoBackFlip : HandleFallingAnimation
{
    protected override void PlayFallAnimation()
    {
        _animator.SetBool("DoBackFlip", true);
    }

    protected override void StopFallAnimation()
    {
        _animator.SetBool("DoBackFlip", false);
    }
}