namespace RSTools
{
	using UnityEditor;
	using UnityEngine;

	[ExecuteInEditMode]
	public class Screenshot : EditorWindow
	{
		int _resolutionWidth = Screen.width;
		int _resolutionHeight = Screen.height;
		int _scale = 1;

		RenderTexture _renderTexture;
		string _destinationFolder = "";
		bool _isTransparent = false;
		bool _takeScreenshot = false;

		public Camera TargetCamera;
		public string LastScreenshot = "";

		[MenuItem ("Tools/Screenshot")]
		public static void ShowWindow ()
		{
			EditorWindow editorWindow = GetWindow (typeof (Screenshot));
			editorWindow.autoRepaintOnSceneChange = true;
			editorWindow.Show ();
		}

		void OnGUI ()
		{
			EditorGUILayout.LabelField ("Resolution", EditorStyles.boldLabel);
			_resolutionWidth = EditorGUILayout.IntField ("Width", _resolutionWidth);
			_resolutionHeight = EditorGUILayout.IntField ("Height", _resolutionHeight);

			EditorGUILayout.Space ();
			EditorGUILayout.BeginVertical ();

			EditorGUILayout.LabelField ("Default Options", EditorStyles.boldLabel);

			if (GUILayout.Button ("Set to game screen size"))
			{
				_resolutionHeight = (int)Handles.GetMainGameViewSize ().y;
				_resolutionWidth = (int)Handles.GetMainGameViewSize ().x;
			}

			if (GUILayout.Button ("Set to default size"))
			{
				_resolutionHeight = 1080;
				_resolutionWidth = 1920;
				_scale = 1;
			}

			EditorGUILayout.EndVertical ();

			_scale = EditorGUILayout.IntSlider ("Screenshot scale", _scale, 1, 4);

			EditorGUILayout.Space ();

			GUILayout.Label ("Select the rendering camera (scene's main camera by default)", EditorStyles.boldLabel);
			TargetCamera = EditorGUILayout.ObjectField (TargetCamera, typeof (Camera), true, null) as Camera;
			if (TargetCamera == null) TargetCamera = Camera.main;

			_isTransparent = EditorGUILayout.Toggle ("Transparent Background", _isTransparent);

			EditorGUILayout.Space ();

			GUILayout.Label ("Destination folder", EditorStyles.boldLabel);

			EditorGUILayout.BeginHorizontal ();

			EditorGUILayout.TextField (_destinationFolder, GUILayout.ExpandWidth (false));
			if (GUILayout.Button ("Browse", GUILayout.ExpandWidth (false)))
				_destinationFolder = EditorUtility.SaveFolderPanel ("Choose destination folder", _destinationFolder, Application.dataPath);

			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.Space ();

			EditorGUILayout.LabelField ("Screenshot resolution : " + _resolutionWidth * _scale + " x " + _resolutionHeight * _scale + " px", EditorStyles.boldLabel);

			if (GUILayout.Button ("Take Screenshot", GUILayout.MinHeight (60)))
			{
				if (_destinationFolder == "")
					_destinationFolder = EditorUtility.SaveFolderPanel ("Path to Save Images", _destinationFolder, Application.dataPath);
				TakeScreenshot ();
			}

			EditorGUILayout.Space ();
			EditorGUILayout.BeginHorizontal ();

			if (GUILayout.Button ("Open last screenshot", GUILayout.MaxWidth (160), GUILayout.MinHeight (40)))
			{
				if (LastScreenshot != "")
					Application.OpenURL ("file://" + LastScreenshot);
			}

			if (GUILayout.Button ("Open folder", GUILayout.MaxWidth (100), GUILayout.MinHeight (40)))
				Application.OpenURL ("file://" + _destinationFolder);

			EditorGUILayout.EndHorizontal ();

			if (_takeScreenshot)
			{
				int resolutionWidth = _resolutionWidth * _scale;
				int resolutionHeight = _resolutionHeight * _scale;
				RenderTexture renderTexture = new RenderTexture (resolutionWidth, resolutionHeight, 24);
				TargetCamera.targetTexture = renderTexture;

				TextureFormat textureFormat;
				textureFormat = _isTransparent ? TextureFormat.ARGB32 : TextureFormat.RGB24;

				Texture2D screenshot = new Texture2D (resolutionWidth, resolutionHeight, textureFormat, false);
				TargetCamera.Render ();
				RenderTexture.active = renderTexture;
				screenshot.ReadPixels (new Rect (0, 0, resolutionWidth, resolutionHeight), 0, 0);
				TargetCamera.targetTexture = null;
				RenderTexture.active = null;
				byte[] bytes = screenshot.EncodeToPNG ();
				string filename = SetScreenshotName (resolutionWidth, resolutionHeight);

				System.IO.File.WriteAllBytes (filename, bytes);
				Debug.Log (string.Format ("Took screenshot to: {0}", filename));
				Application.OpenURL (filename);
				_takeScreenshot = false;
			}
		}

		public string SetScreenshotName (int width, int height)
		{
			string name = "";
			name = string.Format ("{0}/screen_{1}x{2}_{3}.png", _destinationFolder, width, height, System.DateTime.Now.ToString ("yyyy-MM-dd_HH-mm-ss"));
			LastScreenshot = name;
			return name;
		}

		public void TakeScreenshot ()
		{
			Debug.Log ("Taking Screenshot");
			_takeScreenshot = true;
		}
	}
}