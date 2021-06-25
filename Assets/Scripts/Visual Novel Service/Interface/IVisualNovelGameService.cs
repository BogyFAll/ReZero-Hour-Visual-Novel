using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace VisualNovel.Service
{
	/// <summary>
	/// Сервис работы сцены новеллы
	/// </summary>
	public interface IVisualNovelGameService
	{
		/// <summary>
		/// Запускает цикл сцены
		/// </summary>
		void Start();

		/// <summary>
		/// Перейти на определённый фрейм
		/// </summary>
		/// <param name="index">Номер фрейма</param>
		void SetIndex(int index);

		/// <summary>
		/// Следующий фрейм
		/// </summary>
		void NextIndex();

		/// <summary>
		/// Предыдущий фрейм
		/// </summary>
		void LastIndex();

		/// <summary>
		/// Скорость текста
		/// </summary>
		/// <param name="speed">Скорость в секундах</param>
		void SetSpeed(float speed);

		/// <summary>
		/// Вывод текста и имени персонажа
		/// </summary>
		Action<string, string> ActionUI { get; set; }

		/// <summary>
		/// Вывод фонового изображения и спрайта говорящего персонажа
		/// </summary>
		Action<Sprite, Sprite> ActionNewFrame { get; set; }

		/// <summary>
		/// Запуск фонового звука
		/// </summary>
		Action<AudioClip> ActionBackground { get; set; }

		/// <summary>
		/// Запуск видео
		/// </summary>
		Action<VideoClip> ActionStartVideo { get; set; }

		/// <summary>
		/// Вывод главы и названия текста
		/// </summary>
		Action<string> ActionStartPreview { get; set; }

		/// <summary>
		/// Запоск цикла сцены
		/// </summary>
		Action ActionStart { get; set; }

		/// <summary>
		/// Конец цикла сцены
		/// </summary>
		Action ActionExit { get; set; }
	}
}