using UnityEngine;

public class PixellateEffect : MonoBehaviour
{
    public static PixellateEffect i;

    [SerializeField] private RenderTexture pixellateTexture;
    [SerializeField] private Camera renderingCamera;
    [SerializeField] public bool pixellateEnabled;
    [SerializeField] public int pixellateRatio = 40;

    private void Awake()
    {
        i = this;

        ResizeByRatio(pixellateRatio);
        SetEnabled(pixellateEnabled);
    }

    public void SetEnabled(bool enabled)
    {
        pixellateEnabled = enabled;

        if (enabled)
        {
            renderingCamera.targetTexture = pixellateTexture;
        }
        else
        {
            renderingCamera.targetTexture = null;
        }
    }

    public void ResizeByRatio(int ratio)
    {
        ResizeRenderTexture(16 * ratio, 9 * ratio);
    }

    public void ResizeRenderTexture(int newWidth, int newHeight)
    {
        if (pixellateTexture != null)
        {
            pixellateTexture.Release();

            pixellateTexture.width = newWidth;
            pixellateTexture.height = newHeight;
        }
    }
}