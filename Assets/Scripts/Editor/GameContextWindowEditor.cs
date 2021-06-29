using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using VisualNovel.Service;
using System;
using UnityEditor.UIElements;

namespace VisualNovel.Editor
{
	public class GameContextWindowEditor : EditorWindow
	{
		private DialogGraphView _graphView;
		private string _fileName = "New Narative";

		[OnOpenAsset(1)]
		public static bool OnOpenAsset(int id, int line)
		{
			var obj = EditorUtility.InstanceIDToObject(id);

			if (obj as GameContext)
			{
				GameContextWindowEditor window = (GameContextWindowEditor)EditorWindow.GetWindow( typeof( GameContextWindowEditor ) );
				window.Show((GameContext)obj);
			}

			return false;
		}

		public void Show(GameContext context)
		{
			Show();
		}

		private void OnEnable()
		{
			ConstructGraphView();
			GenerateToolBar();
		}

		private void ConstructGraphView()
		{
			_graphView = new DialogGraphView
			{
				name = "TextEditorGraphView Name"
			};

			_graphView.StretchToParentSize();
			rootVisualElement.Add( _graphView );
		}

		private void GenerateToolBar()
		{
			var toolBar = new Toolbar();

			var fileNametextField = new TextField( "File Name:" );
			fileNametextField.SetValueWithoutNotify( _fileName );
			fileNametextField.MarkDirtyRepaint();
			fileNametextField.RegisterValueChangedCallback( evt => _fileName = evt.newValue );

			toolBar.Add( fileNametextField );

			toolBar.Add( new Button( () => SaveData() )
			{
				text = "Save Data"
			} );

			toolBar.Add( new Button( () => LoadData() )
			{
				text = "Load Data"
			} ); ;

			var nodeCreateButton = new Button( () =>
			 {
				 _graphView.CreateDialoguNode( "Ad" );
			 } );

			nodeCreateButton.text = "Создать";

			toolBar.Add( nodeCreateButton );

			rootVisualElement.Add( toolBar );
		}

		private void SaveData()
		{

		}

		private void LoadData()
		{

		}

		private void OnDisable()
		{
			rootVisualElement.Remove( _graphView );
		}
	}

	public class DialogGraphView : GraphView
	{
		public DialogGraphView()
		{
			var style = Resources.Load<StyleSheet>("DialogueGraph");

			styleSheets.Add(style);
			SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

			this.AddManipulator(new ContentDragger());
			this.AddManipulator(new SelectionDragger());
			this.AddManipulator(new RectangleSelector());

			//Добавление ноды
			AddElement( GenerateEntryPointNode() );

			var grid = new GridBackground();
			Insert(0, grid);
			grid.StretchToParentSize();
		}

		private Port GeneratePort(DialogNode node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
		{
			return node.InstantiatePort( Orientation.Horizontal, portDirection, capacity, typeof( float ) );
		}

		/// <summary>
		/// Создание новой ноды
		/// </summary>
		/// <returns></returns>
		private DialogNode GenerateEntryPointNode()
		{
			var node = new DialogNode
			{
				title = "Start",
				GUID = Guid.NewGuid().ToString(),
				DialogText = "DialogText",
				EntryPoint = true
			};

			var generatorPort = GeneratePort( node, Direction.Output );
			generatorPort.portName = "Next";
			node.outputContainer.Add( generatorPort );
			node.RefreshExpandedState();
			node.RefreshPorts();

			node.SetPosition( new Rect( 100, 200, 100, 150 ) );

			return node;
		}

		public DialogNode CreateDialoguNode(string nodeName)
		{
			var dialogNode = new DialogNode
			{
				title = nodeName,
				DialogText = nodeName,
				GUID = Guid.NewGuid().ToString()
			};

			var inputPort = GeneratePort( dialogNode, Direction.Input, Port.Capacity.Multi );
			inputPort.name = "Input";

			var button = new Button( () => AddChoicePort( dialogNode ) );
			button.text = "New Choice";
			dialogNode.titleContainer.Add( button );

			dialogNode.inputContainer.Add( inputPort );
			dialogNode.RefreshExpandedState();
			dialogNode.RefreshPorts();

			dialogNode.SetPosition( new Rect( Vector2.zero, new Vector2( 150, 200 ) ) );

			AddElement( dialogNode );

			return dialogNode;
		}

		public override List<Port> GetCompatiblePorts( Port startPort, NodeAdapter nodeAdapter )
		{
			var compatiblePorts = new List<Port>();

			ports.ForEach( port =>
			 {
				 if ( startPort != port && startPort.node != port.node )
					 compatiblePorts.Add(port);
			 } );

			return compatiblePorts;
		}

		private void AddChoicePort(DialogNode dialogNode)
		{
			var generatePort = GeneratePort( dialogNode, Direction.Output );

			var outPortCount = dialogNode.outputContainer.Query( "connector" ).ToList().Count;
			generatePort.portName = $"Choise {outPortCount}";

			dialogNode.outputContainer.Add( generatePort );
			dialogNode.RefreshPorts();
			dialogNode.RefreshExpandedState();
		}
	}

	public class DialogNode : Node
	{
		public string GUID;
		public string DialogText;
		public bool EntryPoint = false;
	}
}
