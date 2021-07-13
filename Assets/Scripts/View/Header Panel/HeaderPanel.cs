using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using VisualNovel.Service;

namespace VisualNovel.UI
{
	public class HeaderPanel : MonoBehaviour, ICommand, IBeginDragHandler, IDragHandler
	{
		#region Fields

		[SerializeField] private float _minDeltaSwipte = 3f;
		
		[Space]
		[SerializeField] private List<HeaderInfo> _headerInfos = new List<HeaderInfo>();
		[SerializeField] private Transform _footerPanel;
		[SerializeField] private Transform _leftArrow;
		[SerializeField] private Transform _rightArrow;
		[SerializeField] private HeaderContext _headerContext;

		[Space]
		[Header( "Prefabs" )]
		[SerializeField] private GameObject _footerItemPref;

		private int _index = 0;
		private List<CircleUI> _circleUIList = new List<CircleUI>();

		#endregion

		#region Properties

		public GameContext SelectedGameContext { get => _headerInfos[_index].Context; }

		#endregion

		#region Default Methods

		private void Start()
		{
			HideArrow();
			InstateFooter();

			SetHeaderInfo();
			SetFooterIndex();
		}

		#endregion

		#region Methods

		public void CommandHandler( string commandName )
		{
			switch(commandName)
			{
				case "nextCommand":
					NextContentHandler();
					break;
				case "backCommand":
					BackContentHandler();
					break;
				default:
					Debug.LogError( "Нет команды" );
					break;
			}
		}

		private void NextContentHandler()
		{
			int maxIndex = _headerInfos.Count - 1;
			_index = _index >= maxIndex ? 0 : _index + 1;

			SetHeaderInfo();
			SetFooterIndex();
		}

		private void BackContentHandler()
		{
			_index = _index == 0 ? _headerInfos.Count - 1 : _index - 1;

			SetHeaderInfo();
			SetFooterIndex();
		}

		private void SetHeaderInfo()
		{
			_headerContext.BackgroundImage.sprite = _headerInfos[_index].Background;
			_headerContext.NameText.text = _headerInfos[_index].Name;
			_headerContext.PathText.text = _headerInfos[_index].Path;
		}

		private void InstateFooter()
		{
			_headerInfos.ForEach( e =>
			 {
				 var newItem = Instantiate( _footerItemPref, _footerPanel ).GetComponent<CircleUI>();
				 newItem.SetColor( e.name.StartsWith( "Bonus" ) );

				 _circleUIList.Add( newItem );
			 } );
		}

		private void SetFooterIndex()
		{
			_circleUIList.ForEach( e => e.SetMin() );
			_circleUIList[_index].SetMax();
		}

		private void HideArrow()
		{
#if UNITY_ANDROID
			_leftArrow.gameObject.SetActive( false );
			_rightArrow.gameObject.SetActive( false );
#endif
		}

		#endregion

		#region UI Methods

		public void OnDrag( PointerEventData eventData )
		{
			
		}

		public void OnBeginDrag( PointerEventData eventData )
		{
			if ( eventData.delta.x >= _minDeltaSwipte )
				BackContentHandler();

			if ( eventData.delta.x <= _minDeltaSwipte )
				NextContentHandler();
		}

		#endregion
	}
}
