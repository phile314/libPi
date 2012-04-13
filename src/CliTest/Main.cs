using System;

namespace CliTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			using(TestWindow w = new TestWindow()) {
				w.Run(60);
			}
		}
	}
}
