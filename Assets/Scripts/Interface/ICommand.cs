using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
	/// <summary>
	/// ������ ������ ��� UI Button
	/// </summary>
	/// <param name="commandName">�������� �������</param>
	void CommandHandler(string commandName);
}
