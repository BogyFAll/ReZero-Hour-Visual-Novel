using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
	/// <summary>
	/// Список команд для UI Button
	/// </summary>
	/// <param name="commandName">Название команды</param>
	void CommandHandler(string commandName);
}
