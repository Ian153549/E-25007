using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRLibrary
{
	public class AlarmItem
	{
		public string ID;

		public System.DateTime DateTime = System.DateTime.Now;

		public int Level = 0;

		public string Message = "";

		public AlarmItem.AlarmStatus Status = AlarmItem.AlarmStatus.Posted;

		public AlarmItem()
		{
		}

		public AlarmItem(int level, string msg)
		{
			this.ID = Guid.NewGuid().ToString();
			this.Level = level;
			this.Message = msg;
		}

		public enum AlarmStatus
		{
			Posted,
			Acknowledged,
			Dismissed
		}
	}
}
