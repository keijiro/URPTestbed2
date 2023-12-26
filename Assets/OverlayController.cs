using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public sealed partial class OverlayController : MonoBehaviour
{
    #region Public properties

    [field:SerializeField] public Texture2D Layer1 { get; set; }
    [field:SerializeField] public Texture2D Layer2 { get; set; }
    [field:SerializeField] public Texture2D Layer3 { get; set; }

    [field:SerializeField, HideInInspector] public Material Material { get; set; }

    public MaterialPropertyBlock Properties => UpdateProperties();

    public bool IsReady => enabled && Material != null && FirstLayer != null;

    #endregion

    #region MonoBehaviour implementation

    void Update() {} // Just for providing the component enable switch.

    #endregion

    #region Private methods

    Texture2D FirstLayer
      => Layer1 != null ? Layer1 : (Layer2 != null ? Layer2 : Layer3);

    (Vector2 scale, Vector2 offset) GetAspectRatioCompensation()
    {
        var source = FirstLayer;
        var target = GetComponent<Camera>();

        var aspect1 = (float)source.width / source.height;
        var aspect2 = (float)target.pixelWidth / target.pixelHeight;

        var scale = new Vector2(aspect2 / aspect1, aspect1 / aspect2);
        scale = Vector2.Min(Vector2.one, scale);

        var offset = (Vector2.one - scale) / 2;

        return (scale, offset);
    }

    #endregion

    #region Controller implementation

    MaterialPropertyBlock _props;

    MaterialPropertyBlock UpdateProperties()
    {
        if (_props == null) _props = new MaterialPropertyBlock();

        var (scale, offset) = GetAspectRatioCompensation();

        _props.Clear();

        if (Layer1 != null) _props.SetTexture("_Layer1", Layer1);
        if (Layer2 != null) _props.SetTexture("_Layer2", Layer2);
        if (Layer3 != null) _props.SetTexture("_Layer3", Layer3);

        _props.SetVector("_Scale", scale);
        _props.SetVector("_Offset", offset);

        return _props;
    }

    #endregion
}
