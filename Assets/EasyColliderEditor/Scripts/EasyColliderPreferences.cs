//Easy Collider Editor created by Patrick Murphy.
//Please contact pmurph.software@gmail.com for any questions, comments, and support. 
//Please check include docuementation .pdf in the EasyColliderEditor folder for common problems users have encountered.
//If you have any ideas for improvements, or have specific use cases that you wish implemented, just e-mail me and I will see what I can do.
#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[System.Serializable]
public class EasyColliderPreferences : ScriptableObject
{

    //Colour used to "highlight" selected collider.
    [SerializeField] private Color _selectedColliderColour;
    public Color SelectedColliderColour { get { return _selectedColliderColour; } set { _selectedColliderColour = value; EditorUtility.SetDirty(this); } }

    //Key used to select vertices.
    [SerializeField] private KeyCode _vertSelectKeyCode;
    public KeyCode VertSelectKeyCode {get {return _vertSelectKeyCode;} set { _vertSelectKeyCode = value; } }

    //display vertices scaling size
    [SerializeField] private float _displayVerticesScaling;
    public float DisplayVerticesScaling { get { return _displayVerticesScaling;} set { _displayVerticesScaling = value;} }

    //display vertices colour
    [SerializeField] private Color _displayVerticesColour;
    public Color DisplayVerticesColour { get { return _displayVerticesColour; } set { _displayVerticesColour = value; } }

    //hover vertices scaling size
    [SerializeField] private float _hoverVertScaling;
    public float HoverVertScaling { get { return _hoverVertScaling; } set { _hoverVertScaling = value; } }

    //hover vertices scaling colour
    [SerializeField]  private Color _hoverVertColour;
    public Color HoverVertColour { get { return _hoverVertColour; } set { _hoverVertColour = value; EditorUtility.SetDirty(this); } }

    //selected vertice scaling size
    [SerializeField] private float _selectedVertScaling;
    public float SelectedVertScaling { get { return _selectedVertScaling; } set { _selectedVertScaling = value; } }

    //selected vertice scaling colour
    [SerializeField] private Color _selectedVertCol;
    public Color SelectedVertexColour { get { return _selectedVertCol; } set { _selectedVertCol = value; } }

    //overlapped selected vertice scale
    [SerializeField] private float _overlapSelectedVertScale;
    public float OverlapSelectedVertScale { get { return _overlapSelectedVertScale; } set { _overlapSelectedVertScale = value;} }

    //overlapped vertice scaling colour
    [SerializeField] private Color _overlapSelectedVertColour;
    public Color OverlapSelectedVertColour { get { return _overlapSelectedVertColour; } set {_overlapSelectedVertColour = value; } }

    [SerializeField] private int _rotatedColliderLayer;
    public int RotatedColliderLayer { get { return _rotatedColliderLayer; } set {_rotatedColliderLayer = value; } }
    
    /// <summary>
    /// Sets default values for editor on installation.
    /// </summary>
    public void SetDefaultValues()
    {
        DisplayVerticesScaling = 0.02F;
        DisplayVerticesColour = Color.blue;

        HoverVertScaling = 0.03F;
        HoverVertColour = Color.cyan;

        SelectedVertScaling = 0.04F;
        SelectedVertexColour = Color.green;

        OverlapSelectedVertScale = 0.035F;
        OverlapSelectedVertColour = Color.red;
        VertSelectKeyCode = KeyCode.V;

        SelectedColliderColour = Color.black;

        RotatedColliderLayer = 0;
    }

}
#endif