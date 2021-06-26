using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualNovel.Service
{
	public interface IHistory
	{
		string Name { get; set; }
		string Text { get; set; }
	}
}
