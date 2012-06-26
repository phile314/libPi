using System;

namespace CliTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			using(FboTestWindow w = new FboTestWindow()) {
				w.Run(60);
			}
		}
	}
}
