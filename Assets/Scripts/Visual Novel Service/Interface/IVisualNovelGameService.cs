using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace VisualNovel.Service
{
	/// <summary>
	/// ������ ������ ����� �������
	/// </summary>
	public interface IVisualNovelGameService
	{
		/// <summary>
		/// ������� ������
		/// </summary>
		public int Index { get; }

		/// <summary>
		/// ������������ ������
		/// </summary>
		public int GetMaxIndex { get; }

		/// <summary>
		/// ��������� ���� �����
		/// </summary>
		void Start();

		/// <summary>
		/// ������� �� ����������� �����
		/// </summary>
		/// <param name="index">����� ������</param>
		void SetIndex(int index);

		/// <summary>
		/// ��������� �����
		/// </summary>
		void NextIndex();

		/// <summary>
		/// ���������� �����
		/// </summary>
		void LastIndex();


		/// <summary>
		/// ������� �� ��������� �����
		/// </summary>
		void MaxIndex();

		/// <summary>
		/// �������� ������
		/// </summary>
		/// <param name="speed">�������� � ��������</param>
		void SetSpeed(float speed);

		/// <summary>
		/// �������� ������� ������� �� ����� �����
		/// </summary>
		/// <returns></returns>
		IEnumerable<IHistory> GetListFromIndex();

		/// <summary>
		/// ����� ������ � ����� ���������
		/// </summary>
		Action<string, string> ActionUI { get; set; }

		/// <summary>
		/// ����� �������� ����������� � ������� ���������� ���������
		/// </summary>
		Action<GameObject, Sprite> ActionNewFrame { get; set; }

		/// <summary>
		/// ������ �������� �����
		/// </summary>
		Action<AudioClip> ActionBackground { get; set; }

		/// <summary>
		/// ������ �����
		/// </summary>
		Action<VideoClip> ActionStartVideo { get; set; }

		/// <summary>
		/// ����� ����� � �������� ������
		/// </summary>
		Action<string, string> ActionStartPreview { get; set; }

		/// <summary>
		/// ������ ����� �����
		/// </summary>
		Action ActionStart { get; set; }

		/// <summary>
		/// ����� ����� �����
		/// </summary>
		Action ActionExit { get; set; }

		/// <summary>
		/// ������� ��� ����� ����� �����
		/// </summary>
		Action ActionExitEvents { get; set; }
	}
}