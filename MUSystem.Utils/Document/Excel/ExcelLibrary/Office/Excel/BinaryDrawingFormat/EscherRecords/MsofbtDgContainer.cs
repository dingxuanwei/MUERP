using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace MUSystem.Utils.ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtDgContainer : MsofbtContainer
	{
		public MsofbtDgContainer(EscherRecord record) : base(record) { }

		public MsofbtDgContainer()
		{
			this.Type = EscherRecordType.MsofbtDgContainer;
		}

	}
}
