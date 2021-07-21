using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace VisualNovel.Service
{
	public static class ExportToFile
	{
		/// <summary>
		/// ������� � ����
		/// </summary>
		/// <param name="context">���������� ��������������� ����� �����</param>
		public static void Export(GameContext context)
		{
			string path = Environment.GetFolderPath( Environment.SpecialFolder.DesktopDirectory ) + string.Format(@"\{0}.txt", context.GameContextStartItem.Header);

			using ( StreamWriter sw = new StreamWriter( GetFile( path ) ) )
			{
				sw.WriteLine( context.GameContextStartItem.Header );
				sw.WriteLine( context.GameContextStartItem.Name );

				context.GameContextItems.ForEach( e =>
				 {
					 string name = e.PersonName.Length > 0 ? e.PersonName + ": " : string.Empty;
					 string text = e.Text;

					 sw.WriteLine( name + text + "\n\n" );
				 } );
			}

			Debug.LogWarning( "������� ��������! " + path );
		}

		/// <summary>
		/// �������� ��������� ���� ��� �������, ���� �� �����������
		/// </summary>
		/// <param name="path">���� � �����</param>
		/// <returns></returns>
		public static Stream GetFile(string path)
		{
			return File.Open( path, FileMode.OpenOrCreate );
		}
	}
}
