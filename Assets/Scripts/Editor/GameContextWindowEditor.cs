using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using VisualNovel.Service;

namespace VisualNovel.Editor
{
	public class GameContextWindowEditor : EditorWindow
	{
		private GameContext _currentContext;

		[OnOpenAsset(1)]
		public static bool OnOpenAsset(int id, int line)
		{
			var obj = EditorUtility.InstanceIDToObject(id);

			if (obj as GameContext)
			{
				GameContextWindowEditor window = (GameContextWindowEditor)EditorWindow.GetWindow(typeof(GameContextWindowEditor));
				window.Show((GameContext)obj);
			}

			return false;
		}

		public void Show(GameContext context)
		{
			Show();

			_currentContext = context;
		}

		private void OnEnable()
		{
			var view = new TextEditorGraphView
			{
				name = "TextEditorGraphView Name"
			};

			view.StretchToParentSize();
			rootVisualElement.Add(view);
		}
	}

	public class TextEditorGraphView : GraphView
	{
		public TextEditorGraphView()
		{
			var style = Resources.Load<StyleSheet>("DialogueGraph");

			styleSheets.Add(style);
			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());

			var grid = new GridBackground();
			Insert(0, grid);
			grid.StretchToParentSize();
		}
	}
}
