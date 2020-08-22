//Easy Collider Editor created by Patrick Murphy.
//Please contact pmurph.software@gmail.com for any questions, comments, and support. 
//Please check include docuementation .pdf in the EasyColliderEditor folder for common problems users have encountered.
//If you have any ideas for improvements, or have specific use cases that you wish implemented, just e-mail me and I will see what I can do.


using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEditorInternal;


public class EasyColliderEditorWindow : EditorWindow
{
  enum AttachmentEnum
  {
    SelectedGameObject,
    ColliderHolders,
  }
  AttachmentEnum attachmentStyle;
  //ECE Objects.
  private GameObject _easyColliderGameObject;
  private EasyColliderEditor _easyColliderEditor;

  public GameObject SelectedGameObject;
  public Transform TransformToAttachCollidersTo;

  public List<MeshTransform> MeshTransformList;

  //Component Lists.
  public List<Component> AddedComponents; //Contains a list of components added to the selected gameobject for functionality. (Currently only mesh colliders are added)
  public List<Component> DisabledComponents; //List of components already on the selected gameobject that are temporarily disabled for functionality. (Disables non-mesh colliders, and sets rigidbodies isKinematic to true)

  //Display Options
  public bool UpdateSceneDisplay;
  public bool DisplayMeshVertexHandles;

  //Bools and buttons.
  public bool AttachToBaseObject;
  public bool AttachToColliderHolders;

  public bool VertexSelectEnabled;
  public bool ColliderSelectEnabled;
  private bool _includeMeshesFromChildren = true;
  public bool IncludeMeshesFromChildren = true;
  private bool _displayPreferences = false;
  public bool IsTrigger = false;



  //PREFERENCES and SETTINGS
  private EasyColliderPreferences _easyColliderPreferences;


  [MenuItem("Window/EasyColliderEditor")]
  static void Init()
  {
    EditorWindow.GetWindow(typeof(EasyColliderEditorWindow));
  }


  void OnEnable()
  {
    if (_easyColliderPreferences == null)
    {
      // Preferences path now calculated by getting the asset of the window script.
      string path = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
      string preferencesPath = path.Remove(path.Length - 34) + "EasyColliderPreferences.asset";
      _easyColliderPreferences =
          AssetDatabase.LoadAssetAtPath(preferencesPath,
              typeof(EasyColliderPreferences)) as EasyColliderPreferences;
      if (_easyColliderPreferences == null)
      {
        _easyColliderPreferences = CreateInstance<EasyColliderPreferences>();
        _easyColliderPreferences.SetDefaultValues();
        AssetDatabase.CreateAsset(_easyColliderPreferences, preferencesPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.LogWarning("No preferences file found, new preferences asset created with default settings at" + preferencesPath);
      }
    }
    if (_easyColliderGameObject == null) //also helps some script save errors.
    {
      _easyColliderGameObject = new GameObject("EasyColliderEditor");
      Undo.RegisterCreatedObjectUndo(_easyColliderGameObject, "Created EasyColliderEditorObject");
      _easyColliderEditor = Undo.AddComponent<EasyColliderEditor>(_easyColliderGameObject);
      _easyColliderEditor.editorWindow = this;
    }
  }

  void OnDisable()
  {
    RestoreComponents();
    //Remove error when trying to enter play mode.
    if (_easyColliderGameObject != null && !EditorApplication.isPlayingOrWillChangePlaymode)
    {
      Undo.DestroyObjectImmediate(_easyColliderGameObject);
    }
  }

  void OnGUI()
  {
    if (!EditorApplication.isPlaying)
    {
      if (_easyColliderEditor == null)
      { //if the ECE gameobject gets deleted by the user, the object gets cleared, components restored, and everything is reset.
        //does sameactions as clicking button "Finish current gameobject".
        if (SelectedGameObject != null)
        {
          OnDisable();
          SelectedGameObject = null;
        }
        OnEnable();
      }
      EditorGUI.BeginChangeCheck(); //Check if selected game object has changed to update mesh vertices if it has.
      SelectedGameObject =
          (GameObject)
              EditorGUILayout.ObjectField("Selected Game Object:", SelectedGameObject, typeof(GameObject), true);
      EditorGUI.EndChangeCheck();
      if (GUI.changed)
      {
        if (SelectedGameObject != null)
        {
          if (!EditorUtility.IsPersistent(SelectedGameObject))
          {
            GameObjectSelectedHasChanged();
          }
          else
          {
            Debug.LogWarning("Selected Gameobject: " + SelectedGameObject.name +
                        " is not active in the scene. Remember to use game objects from the scene heirarchy.");
            SelectedGameObject = null;
          }
        }
        else
        {
          GameObjectSelectedHasChanged(); //Object has been reset to none by the user
        }
      }
      if (GameObjectIsActiveAndFromScene(ref SelectedGameObject))
      {
        if (MeshTransformList == null)//Preserves mesh transform list between script saves / updates.
        {
          GameObjectSelectedHasChanged();
        }
        EditorGUI.BeginChangeCheck();
        VertexSelectEnabled = EditorGUILayout.ToggleLeft(new GUIContent("Enable Vertex Select", "Enables viewport vertex selection by raycast"), VertexSelectEnabled);
        EditorGUI.EndChangeCheck();
        _easyColliderEditor.SelectVertByRaycast = VertexSelectEnabled;
        if (VertexSelectEnabled && GUI.changed)
        {
          _easyColliderEditor.SelectedVertices.Clear();
          if (EditorWindow.focusedWindow != SceneView.currentDrawingSceneView)
          {//Focuses the editor scene window if it is not currently focused.
            if (SceneView.currentDrawingSceneView != null)
            {
              SceneView.currentDrawingSceneView.Focus();
            }
          }
        }

        EditorGUI.BeginChangeCheck();
        ColliderSelectEnabled = EditorGUILayout.ToggleLeft(new GUIContent("Enable Collider Select", "Enables viewport selection of colliders for removal"), ColliderSelectEnabled);
        EditorGUI.EndChangeCheck();
        _easyColliderEditor.SelectColliderByRaycast = ColliderSelectEnabled;
        if (ColliderSelectEnabled && GUI.changed)
        {
          _easyColliderEditor.SelectedCollider = null;
          if (EditorWindow.focusedWindow != SceneView.currentDrawingSceneView)
          {//Focuses the editor scene window if it is not currently focused.
            if (SceneView.currentDrawingSceneView != null)
            {
              SceneView.currentDrawingSceneView.Focus();
            }
          }
        }
        if (_easyColliderEditor.SelectedCollider != null)
        {
          if (GUILayout.Button(new GUIContent("Remove Selected Collider", "Removes selected collider indicated by black cube at center of selected collider")))
          {
            Undo.DestroyObjectImmediate(_easyColliderEditor.SelectedCollider);
            _easyColliderEditor.SelectedCollider = null;
          }
        }

        //Which object to attach created colliders to. Changed to enum popup to improve UI/confusion by having multiple checkboxes where one has to be checked, enum works better for this.
        attachmentStyle = (AttachmentEnum)EditorGUILayout.EnumPopup(new GUIContent("Attach To:",
            "Method to use to attach colliders, either attach to the selected gameobject (if possible), or a separate created child collider holder gameobject"), attachmentStyle);
        if (attachmentStyle == AttachmentEnum.ColliderHolders)
        {
          AttachToBaseObject = false;
          AttachToColliderHolders = true;
          TransformToAttachCollidersTo = null;
        }
        else if (attachmentStyle == AttachmentEnum.SelectedGameObject)
        {
          AttachToBaseObject = true;
          AttachToColliderHolders = false;
          TransformToAttachCollidersTo = SelectedGameObject.transform;
        }

        //collider creation buttons on editor window show up only after vertices are selected.
        if (_easyColliderEditor.SelectedVertices.Count > 0)
        {
          if (GUILayout.Button(new GUIContent("Create Box Collider",
                  "Creates a Box Collider that contains the currently selected vertices. See documentation for vertex selection guide.")))
          {
            if (_easyColliderEditor.SelectedVertices.Count <= 1)
            {
              Debug.LogWarning("To create a box collider correctly at least vertices should be selected. See documentation for for more info.");
            }
            CreateOrSetObjectToAttachColliderTo();
            _easyColliderEditor.CreateBoxCollider(TransformToAttachCollidersTo);
          }
          if (GUILayout.Button(new GUIContent("Create Sphere Collider",
                  "Creates a Sphere Collider that contains the currently selected vertices. See documentation for vertex selection guide.")))
          {
            if (_easyColliderEditor.SelectedVertices.Count <= 1)
            {
              Debug.LogWarning("To create a sphere collider at least 2 vertices should be selected. See documentation for more info.");
            }
            CreateOrSetObjectToAttachColliderTo();
            _easyColliderEditor.CreateSphereCollider(TransformToAttachCollidersTo);
          }
          if (GUILayout.Button(new GUIContent("Create Capsule Collider",
                  "Creates a Capsule Collider that contains the currently selected vertices. See documentation for vertex selection guide.")))
          {
            if (!(_easyColliderEditor.SelectedVertices.Count >= 3))
            {
              Debug.LogWarning("To create a capsule collider correctly using this method at least 3 vertices should be selected. See documentation for more info.");
            }
            CreateOrSetObjectToAttachColliderTo();
            _easyColliderEditor.CreateCapsuleColliderAlternate(TransformToAttachCollidersTo);
          }
          if (GUILayout.Button(new GUIContent("Create Rotated Box Collider")))
          {
            if (!(_easyColliderEditor.SelectedVertices.Count >= 3))
            {
              Debug.LogWarning("To create a rotated box collider using reate Rotate Box Collider Alternate, at least 3 vertices must be selected. See documentation for more info.");
            }
            else
            {
              _easyColliderEditor.CreateRotatedBoxColliderAlternate(SelectedGameObject.transform);
            }
          }
          if (GUILayout.Button(new GUIContent("Create Capsule Collider (Old)",
                  "Creates a Capsule Collider that contains the currently selected vertices. See documentation for vertex selection guide.")))
          {
            if (!(_easyColliderEditor.SelectedVertices.Count == 3))
            {
              Debug.LogWarning("To create a capsule collider correctly 3 vertices should be selected. See documentation for more info.");
            }
            CreateOrSetObjectToAttachColliderTo();
            _easyColliderEditor.CreateCapsuleCollider(TransformToAttachCollidersTo);
          }
          if (GUILayout.Button(new GUIContent("Create Rotated Box Collider (Old)",
                  "Tries to create a Rotated Box Collider that contains the currently selected vertices. See documentation for vertex selection guide.")))
          {
            if (!(_easyColliderEditor.SelectedVertices.Count == 4))
            {
              Debug.LogWarning("To create a rotated box collider correctly 4 vertices should be selected. See documentation for more info.");
            }
            //Rotated Colliders create their own object to attach to.
            _easyColliderEditor.CreateRotatedBoxCollider(SelectedGameObject.transform);
          }

        }
        EditorGUILayout.LabelField("-Additional Toggles");
        //Preferences and Extras
        DisplayMeshVertexHandles = EditorGUILayout.ToggleLeft(new GUIContent("Display Mesh Vertices", "Draws a gizmo over all currently selectable vertices."), DisplayMeshVertexHandles);
        _easyColliderEditor.DrawVertices = DisplayMeshVertexHandles;
        IncludeMeshesFromChildren = EditorGUILayout.ToggleLeft(new GUIContent("Include Child Meshes", "Includes child meshes as possible vertex selections."), IncludeMeshesFromChildren);
        if (IncludeMeshesFromChildren != _includeMeshesFromChildren)
        {
          _includeMeshesFromChildren = IncludeMeshesFromChildren;
          GameObjectSelectedHasChanged(); //allows regeneration of mesh lists to include/uninclude children.
        }
        EditorGUI.BeginChangeCheck();
        IsTrigger = EditorGUILayout.ToggleLeft(new GUIContent("Create Collider as Trigger",
            "Sets the created colliders 'Is Trigger' field to this value on creation"), IsTrigger);
        EditorGUI.EndChangeCheck();
        if (GUI.changed && _easyColliderEditor != null)
        {
          _easyColliderEditor.IsTrigger = IsTrigger;
        }
        if (GUILayout.Button(new GUIContent("Remove all Colliders on Selected GameObject",
            "Removes all colliders on selected gameobject, including ones that were present before editing. Removes colliders on ALL children if include child meshes is enabled")))
        {
          RemoveAllCollidersOnSelectedGameObject(_includeMeshesFromChildren);
        }
        if (_easyColliderPreferences != null)
        {
          _displayPreferences = EditorGUILayout.ToggleLeft(new GUIContent("Edit Preferences", "Enables editing of preferences"), _displayPreferences);
          if (_displayPreferences)
          {
            _easyColliderPreferences.DisplayVerticesColour = EditorGUILayout.ColorField("Vertex Display Colour:",
    _easyColliderPreferences.DisplayVerticesColour);
            _easyColliderPreferences.DisplayVerticesScaling = EditorGUILayout.FloatField("Vertex Display Scaling:",
                _easyColliderPreferences.DisplayVerticesScaling);

            _easyColliderPreferences.HoverVertColour = EditorGUILayout.ColorField("Hover Vertex Colour:",
                _easyColliderPreferences.HoverVertColour);
            _easyColliderPreferences.HoverVertScaling = EditorGUILayout.FloatField("Hover Vert Scaling:",
                _easyColliderPreferences.HoverVertScaling);

            _easyColliderPreferences.SelectedVertexColour = EditorGUILayout.ColorField("Selected Vertex Colour:",
                _easyColliderPreferences.SelectedVertexColour);
            _easyColliderPreferences.SelectedVertScaling = EditorGUILayout.FloatField("Selected Vertex Scaling:",
                _easyColliderPreferences.SelectedVertScaling);

            _easyColliderPreferences.OverlapSelectedVertColour =
                EditorGUILayout.ColorField("Overlap Selected Vert Colour:",
                    _easyColliderPreferences.OverlapSelectedVertColour);
            _easyColliderPreferences.OverlapSelectedVertScale =
                EditorGUILayout.FloatField("Overlap Selected Vert Scale:",
                    _easyColliderPreferences.OverlapSelectedVertScale);

            _easyColliderPreferences.VertSelectKeyCode = (KeyCode)EditorGUILayout.EnumPopup(new GUIContent("Vert Select KeyCode:", "Shortcut to use for selecting vertices."),
                    _easyColliderPreferences.VertSelectKeyCode);
            _easyColliderPreferences.SelectedColliderColour = EditorGUILayout.ColorField("Selected Collider Colour:",
                _easyColliderPreferences.SelectedColliderColour);

            _easyColliderPreferences.RotatedColliderLayer = EditorGUILayout.LayerField(new GUIContent("Col Holder/Rotated Layer:", "Layer to apply to when using Collider Holders, or creating a Rotated Collider"),
                _easyColliderPreferences.RotatedColliderLayer);

            if (GUILayout.Button("Reset Preferences to Defaults"))
            {
              _easyColliderPreferences.SetDefaultValues();
            }
          }
        }
        if (GUILayout.Button(new GUIContent("Finish Current GameObject",
            "Removes any required components that were added, Restores any disabled components, and resets for new gameobject selection.")))
        {
          OnDisable();
          SelectedGameObject = null;
        }

      }
    }
    if (EditorApplication.isPlaying)
    {
      EditorGUILayout.LabelField("Easy Collider Editor is only useable in Edit mode. Exit play mode to begin.");
    }
    EditorGUILayout.LabelField("Quickstart:");
    EditorGUILayout.LabelField("1. Select object with mesh, or parent with children meshes, from scene hierarchy using field at the top of this window.");
    string key = "V"; //default 
    if (_easyColliderPreferences != null)
    {
      key = _easyColliderPreferences.VertSelectKeyCode.ToString();
    }
    EditorGUILayout.LabelField("2. Enable vertex selection with the toggle beside it.");
    EditorGUILayout.LabelField("3. Select vertices by hovering over mesh in scene and pressing " + key + ".");
    EditorGUILayout.LabelField("4. Click buttons that appear after selection to create colliders.");
    EditorGUILayout.LabelField("5. Click Finish Current GameObject button when done creating colliders on an object.");
    EditorGUILayout.LabelField("5. For info on proper vertex selection see documentation .pdf in Assets/EasyColliderEditor");

    if (GUI.changed)
    {
      SceneView.RepaintAll();
    }

  }


  /// <summary>
  /// removes all colliders on selected gameobject and collider holders, also includes children if include child meshes is enabled.
  /// </summary>
  void RemoveAllCollidersOnSelectedGameObject(bool includeChildren)
  {
    if (SelectedGameObject != null)
    {
      if (includeChildren)
      {
        RemoveCollidersOnTransformAndAllChildren(SelectedGameObject.transform);
      }

      else
      {
        foreach (MeshTransform mt in MeshTransformList)
        {
          Collider[] collider = mt.transform.GetComponents<Collider>();
          if (collider != null)
          {
            for (int i = 0; i < collider.Length; i++)
            {
              if (!(collider[i] is MeshCollider))
              {
                Undo.DestroyObjectImmediate(collider[i]);
              }
            }
          }
          int childrenCount = mt.transform.childCount;
          for (int k = 0; k < childrenCount; k++)
          {
            Transform t = mt.transform.GetChild(k);
            if (t.name == "Collider Holder" || t.name == "RotatedCollider")
            {
              Collider[] cols = t.GetComponents<Collider>();
              if (cols != null)
              {
                for (int p = 0; p < cols.Length; p++)
                {
                  Undo.DestroyObjectImmediate(cols[p]);
                }
              }
            }
          }
        }
      }
    }
  }

  /// <summary>
  /// Removes all colliders on the transform, and all of it's children, and all of their children, etc.
  /// </summary>
  /// <param name="t"></param>
  void RemoveCollidersOnTransformAndAllChildren(Transform t)
  {
    Collider[] colliders = t.GetComponents<Collider>();
    if (colliders != null)
    {
      for (int j = 0; j < colliders.Length; j++)
      {
        if (!(colliders[j] is MeshCollider))
        {
          Undo.DestroyObjectImmediate(colliders[j]);
        }
      }
    }
    int children = t.childCount;
    for (int i = 0; i < children; i++)
    {
      RemoveCollidersOnTransformAndAllChildren(t.GetChild(i));
    }
  }


  /// <summary>
  /// Sets the transform the created collider should be attached to, or creates a new collider holder if none found.
  /// </summary>
  void CreateOrSetObjectToAttachColliderTo()
  {
    if (AttachToBaseObject)
    {
      TransformToAttachCollidersTo = SelectedGameObject.transform;
    }
    else if (TransformToAttachCollidersTo == null)
    {
      TransformToAttachCollidersTo = SelectedGameObject.transform.Find("Collider Holder");
      if (TransformToAttachCollidersTo == null)
      {
        TransformToAttachCollidersTo = new GameObject("Collider Holder").transform;
        Undo.RegisterCreatedObjectUndo(TransformToAttachCollidersTo.gameObject, "Created Collider Holder"); //Registers created holder for undo.
        TransformToAttachCollidersTo.parent = SelectedGameObject.transform;
        TransformToAttachCollidersTo.position = SelectedGameObject.transform.position;
        TransformToAttachCollidersTo.rotation = SelectedGameObject.transform.rotation;
        TransformToAttachCollidersTo.localScale = SelectedGameObject.transform.localScale;
        TransformToAttachCollidersTo.gameObject.layer = _easyColliderPreferences.RotatedColliderLayer;
      }
    }
  }

  /// <summary>
  /// Removes any added components by EasyColliderEditor, and restores any disabled components (colliders & rigidbody settings are modified)
  /// </summary>
  void RestoreComponents()
  {
    if (AddedComponents != null)
    {
      foreach (Component component in AddedComponents)
      {
        if (component != null && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
          Undo.DestroyObjectImmediate(component);
        }
      }
      if (!EditorApplication.isPlayingOrWillChangePlaymode)
      {
        AddedComponents.Clear();
      }
    }
    if (DisabledComponents != null)
    {
      foreach (Component component in DisabledComponents)
      {
        if (component != null)
        {
          if (component is Rigidbody)
          {
            Rigidbody rb = component as Rigidbody;
            rb.isKinematic = false; //only rb's whos setting was false originally were added to disabled components.
          }
          else if (component is Collider)
          {
            Collider col = component as Collider;
            col.enabled = true;
          }
        }
      }
      DisabledComponents.Clear();
    }
  }



  /// <summary>
  /// Restores previously removed components, and removes added components,
  /// then creates a new mesh transform list.
  /// then ensures required components are added.
  /// </summary>
  void GameObjectSelectedHasChanged()
  {
    RestoreComponents();
    CreateNewMeshTransformList();
    foreach (MeshTransform mt in MeshTransformList)
    {
      EnsureRequiredComponents(mt);
    }
  }



  /// <summary>
  /// Clears the current list of mesh transforms and creates a new one
  /// that includes the selected gameobject and it's children if include meshes from children is selected.
  /// </summary>
  private void CreateNewMeshTransformList()
  {
    if (MeshTransformList != null)
    {
      MeshTransformList.Clear();
    }
    var meshListBuilder = new MeshListBuilder();
    MeshTransformList = meshListBuilder.GetMeshList(SelectedGameObject, IncludeMeshesFromChildren);
    _easyColliderEditor.MTList = MeshTransformList;
    _easyColliderEditor.SelectedGameObject = SelectedGameObject;
  }


  /// <summary>
  /// Ensures required components are attached, like mesh colliders.
  /// Also ensures components that cause errors in functionality are disabled like rigidbodies, and other colliders.
  /// </summary>
  /// <param name="mt"></param>
  private void EnsureRequiredComponents(MeshTransform mt)
  {
    if (AddedComponents == null)
    {
      AddedComponents = new List<Component>();
    }
    if (DisabledComponents == null)
    {
      DisabledComponents = new List<Component>();
    }
    Collider[] colliders = mt.transform.GetComponents<Collider>();
    foreach (Collider item in colliders)
    {
      MeshCollider testCast = item as MeshCollider; //if already has a mesh collider doesn't disable it.
      if (testCast == null)
      {
        Debug.LogWarning("Collider already on " + mt.transform.name + ". Disabling while creating colliders.");
        DisabledComponents.Add(item);
        item.enabled = false;
      }
    }
    MeshCollider meshCollider = mt.transform.GetComponent<MeshCollider>();
    if (meshCollider == null)
    {
      meshCollider = Undo.AddComponent<MeshCollider>(mt.transform.gameObject);
      if (meshCollider.sharedMesh == null)
      {
        meshCollider.sharedMesh = mt.mesh;
      }
      AddedComponents.Add(meshCollider);
    }
    Rigidbody rb = mt.transform.GetComponent<Rigidbody>();
    if (rb != null)
    {
      if (!rb.isKinematic) //checks to see if rb is kinematic, non-kinematic rigidbodies do not allow for vertex selection.
      {
        Debug.LogWarning("Rigidbody attached to " + mt.transform.name + ". Setting to kinematic temporarily to enable functionality");
        DisabledComponents.Add(rb); //only rigibodies with kinematic not set are added to disabled components.
        rb.isKinematic = true;
      }
    }

  }

  /// <summary>
  /// Checks to see if the gameobject is active in the scene heirarchy. Ensures people don't select objects from the assets foldout of the object selection.
  /// Logs warning to remind user if they accidentally do select a gameobject not currently in the scene.
  /// </summary>
  /// <param name="obj"></param>
  /// <returns></returns>
  bool GameObjectIsActiveAndFromScene(ref GameObject obj)
  {
    if (obj != null)
    {
      if (!obj.activeInHierarchy)
      {
        Debug.LogWarning("Selected Gameobject: " + obj.name +
                         " is not active in the scene. Remember to use game objects that are enabled/active in the current scene.");
        obj = null;
        return false;
      }
      else return true;
    }
    return false;
  }





}
